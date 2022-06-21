using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using BoD_OTLToolBox.Connectors;
using BoD_OTLToolBox.OTLObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BoD_OTLToolBox.Plugin
{

    public static class DataHandler
    {
        // Autocad Application Handles     

        public static Database AutocadDatabase { get; set; }

        // External Connector Handles    
        public static SQLiteConnector SQLiteConnector;
        public static ApplicationSettings settings;

        private static List<OTL_ObjectType> OTL_List_ObjectTypes; // list with all possible OTL classes. Contains parameters and main classes as well as default values.

        /// <summary>
        /// Initialize all objects
        /// </summary>
        public static void Init()
        {
            settings = new ApplicationSettings();
            // init application settings
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings.dat";
            settings.Init(path);
            Refresh_DB();
            OTL_List_ObjectTypes = new List<OTL_ObjectType>();
        }


        /// <summary>
        /// Reset the acad database handle
        /// </summary>
        public static void Refresh_DB()
        {
            // update Autocad database
            AutocadDatabase = Autodesk.AutoCAD.DatabaseServices.HostApplicationServices.WorkingDatabase;
        }

        /// <summary>
        /// Import OTL data classes from an SQL
        /// This will fill ALL OTL_LIST_(...)Type instances with data
        /// </summary>
        public static void Import_OTLDB()
        {
            Refresh_DB();
            // reset the lists
            OTL_List_ObjectTypes = new List<OTL_ObjectType>();
            // get all OTL objects
            List<OTL_ObjectType> allObjects = SQLiteConnector.GetAllOTLObjects();
            foreach (OTL_ObjectType item in allObjects)
            {
                OTL_List_ObjectTypes.Add(SQLiteConnector.GetParameters(item));
            }
            PropertySetConnector conn = new PropertySetConnector(AutocadDatabase);
            foreach (OTL_ObjectType item in OTL_List_ObjectTypes)
            {
                conn.CreatePropertySetDefinition(item);
            }
        }

        /// <summary>
        /// Return a list of all used OTL objectTypes
        /// </summary>
        /// <returns></returns>
        public static List<OTL_ObjectType> GetOTLObjectTypes()
        {
            return OTL_List_ObjectTypes;
        }

        /// <summary>
        /// initialize the SQLite connector
        /// </summary>
        /// <param name="path"></param>
        public static void InitDatabase(string SqlPath)
        {
            SQLiteConnector = new SQLiteConnector(SqlPath);
        }

    }
}