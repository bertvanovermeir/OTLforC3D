using BoD_OTLToolBox.OTLObjects;
using BoD_OTLToolBox.Plugin;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace BoD_OTLToolBox
{
    public class SQLiteConnector
    {
        private SQLiteConnection sqlite_conn_OTL; // OTL objects
        
        /// <summary>
        /// create a new database connection. Should not be referenced more than once.
        /// </summary>
        /// <param name="path"></param>
        public SQLiteConnector(string path) 
        { 
        // create a new database connection:
        sqlite_conn_OTL = new SQLiteConnection("Data Source = " + path + "; Version = 3; Read Only = True;");           
        }

        /// <summary>
        /// Get all possible OTL classes in the SQL file .
        /// </summary>
        /// <returns></returns>
        public List<OTL_ObjectType> GetAllOTLObjects()
        {
            List<OTL_ObjectType> tempList = new List<OTL_ObjectType>();
            // define the query
            string query = "select * from OSLOClass as cla left join OSLORelaties as rel on rel.uri = cla.uri where rel.uri is NULL and cla.uri like '%onderdeel%' or cla.uri like '%installatie%'";
            // open the connection:
            sqlite_conn_OTL.Open();
            var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
            sqlite_cmd.CommandText = query;
            var sqlite_datareader = sqlite_cmd.ExecuteReader();
            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // check columns in query to know what to transfer, 
                OTL_ObjectType temp = new OTL_ObjectType((string) sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3), (string)sqlite_datareader.GetValue(2));
                tempList.Add(temp);
            }
            sqlite_conn_OTL.Close();
            return tempList;
        }

        /// <summary>
        /// Get all possible parameters for a certain OTL class
        /// </summary>
        /// <param name="OTLClass"></param>
        /// <returns></returns>
        public OTL_ObjectType GetParameters(OTL_ObjectType OTLClass)
        {
            List<OTL_Parameter> tempParams = new List<OTL_Parameter>();
            // define the query to get all parameters per object
            string query = "select osloAttributen.name as MainAttributeName, osloAttributen.label_nl as MainAttributeFriendlyName, osloAttributen.definition_nl as MainAttributeDescription, osloAttributen.kardinaliteit_max as MainCardinality, osloAttributen.type as MainDataType, osloAttributenType1.name as SubAttributeName, osloAttributenType1.label_nl as SubAttributeFriendlyName, osloAttributenType1.definition_nl as SubAttributeDescription, osloAttributenType1.uri as URI, osloAttributenType1.kardinaliteit_max as SubCardinality, osloAttributenType1.type as SubDataType, osloAttributenType2.name as SubSubAttributeName, osloAttributenType2.label_nl as SubSubAttributeFriendlyName, osloAttributenType2.definition_nl as SubSubAttributeDescription, osloAttributenType2.uri as SubURI, osloAttributenType2.type as SubSubDataType, osloAttributenType3.name as UnionSubAttributeName, osloAttributenType3.type as UnionSubAttributeType, osloAttributenType4.name as UnionSubSubAttributeName, osloAttributenType4.type as UnionSubSubAttributeType , osloAttributenType3.definition_nl as UnionSubAttributeDescription, osloAttributenType4.definition_nl as UnionSubSubAttributeDescription " +
                "from osloAttributen as osloAttributen left outer join osloDatatypeComplexAttributen as osloAttributenType1 on osloAttributen.type = osloAttributenType1.class_uri " +
                "left outer join osloDatatypeComplexAttributen  as osloAttributenType2 on osloAttributenType1.type = osloAttributenType2.class_uri " +
                "left outer join osloDatatypeUnionAttributen as osloAttributenType3 on osloAttributen.type = osloAttributenType3.class_uri " +
                "left outer join osloDatatypeComplexAttributen  as osloAttributenType4 on osloAttributenType3.type = osloAttributenType4.class_uri where osloAttributen.class_uri like '%" + OTLClass.Uri + "%'";
            // open the connection:
            sqlite_conn_OTL.Open();
            var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
            
            sqlite_cmd.CommandText = query;
            var sqlite_datareader = sqlite_cmd.ExecuteReader();
            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // check columns in query to know what to transfer, 
                OTL_Parameter p = new OTL_Parameter(
                    new List<object> {sqlite_datareader.GetValue(0), sqlite_datareader.GetValue(5), sqlite_datareader.GetValue(11), sqlite_datareader.GetValue(16), sqlite_datareader.GetValue(18) },
                    new List<object> { sqlite_datareader.GetValue(3), sqlite_datareader.GetValue(9), "1","1","1" }, // incorrect for now, i guess??
                    new List<object> { sqlite_datareader.GetValue(2), sqlite_datareader.GetValue(7), sqlite_datareader.GetValue(13), sqlite_datareader.GetValue(20), sqlite_datareader.GetValue(21) },
                    new List<object> { sqlite_datareader.GetValue(4), sqlite_datareader.GetValue(10), sqlite_datareader.GetValue(15),sqlite_datareader.GetValue(17), sqlite_datareader.GetValue(19) });
                tempParams.Add(p);  
            }
            sqlite_conn_OTL.Close();

                // check data type again of every object in otlclass and change accordingly
                for (int i = 0; i < tempParams.Count; i++)
            {
                OTL_Parameter parameter = tempParams[i];
                // parse the datatype               
                parameter = ParseDataType(parameter);
                // override default value if name is typeURI
                if(parameter.friendlyName.Contains(DataHandler.settings.ReadSetting("OTL_PROPERTY_SET_URL_COLUMN")))
                {
                   parameter.Defaultvalue = OTLClass.Uri;
                } 
                    OTLClass.AddParameter(parameter);
            }          
            return OTLClass;
        }

        /// <summary>
        /// parse each parameter for data type, to define default value and list if necessary
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private OTL_Parameter ParseDataType(OTL_Parameter p)
        {
            string DataType = p.dataTypeString;
            // primitives
            // very primitive
            if(DataType.Contains("XMLSchema#") || DataType.Contains("rdf-schema#") || DataType.Contains("generiek#Getal") ||DataType.Contains("#Dte"))
            {
                string temp = DataType.Split('#')[1];
                switch (temp)
                {
                    case "Getal":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        p.Defaultvalue = -99999.99d;
                        break;
                    case "Integer":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Integer;
                        p.Defaultvalue = 99999;
                        break;
                    case "Decimal":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        p.Defaultvalue = -99999.99d;
                        break;
                    case "DateTime":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        break;
                    case "Date":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        break;
                    case "Time":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        break;
                    case "String":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        p.Defaultvalue = "-";
                        break;
                    case "Boolean":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.List;
                        p.DropdownValues = new List<string> {"- ","True","False"};
                        p.Defaultvalue = "-";
                        break;
                    case "Literal":
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                        p.Defaultvalue = -99999.99d;
                        break;
                    default:
                        p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                        p.Defaultvalue = "-";
                        break;
                }
            }
            // kwantWaarde
            else if(DataType.Contains("#KwantWrdIn"))
            {
                p.DataType = Autodesk.Aec.PropertyData.DataType.Real;
                p.Defaultvalue = -99999.99d;
            }
            // Unions (lists in acad)
            else if (DataType.Contains("#Dtu"))
            {
                // sql query needed here
                p.DataType = Autodesk.Aec.PropertyData.DataType.List;
                p.Defaultvalue = "-";
                p.DropdownValues = new List<string> { "-" };
                //query
                string temp = DataType.Split('#')[1];
                string query = "select name from oslodatatypeunionAttributen where class_uri like '%" + temp + "%'";
                // open the connection:
                sqlite_conn_OTL.Open();
                var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                sqlite_cmd.CommandText = query;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    p.DropdownValues.Add((string)sqlite_datareader.GetValue(0));
                }
                sqlite_conn_OTL.Close();
            }
            // Enums TTL (lists in acad)
            else if (DataType.Contains("#NOTUSEDFORNOW")) // block list creation for now, should be: DataType.Contains("#Kl")
            {
                //webrequest needed here or internal files
                p.DataType = Autodesk.Aec.PropertyData.DataType.List;
                p.DropdownValues = new List<string> {"-"};               
                // query the files in attachment.
                // find the correct file in folder
                string filename = DataType.Split('#')[1] + ".ttl";
                if(File.Exists(DataHandler.settings.ReadSetting("TTL_PATH") + "\\" + filename))
                {
                    string[] lines = File.ReadAllLines(DataHandler.settings.ReadSetting("TTL_PATH") + "\\" + filename, System.Text.Encoding.UTF8);
                    foreach (string item in lines)
                    {
                        if(item.Contains("skos:Concept;"))
                        {
                            string listText = item.Split('>')[0];
                            string sublistText = listText.Split('/')[listText.Split('/').Length-1];                                         
                            p.DropdownValues.Add(sublistText);
                        }
                    }
                }
                p.Defaultvalue = "-";
            }
            else
            {               
                    p.DataType = Autodesk.Aec.PropertyData.DataType.Text;
                    p.Defaultvalue = "-";                               
            }
            return p;
        }
    }
}

