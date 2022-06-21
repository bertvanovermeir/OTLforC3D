using System;
using System.Collections.Generic;

namespace BoD_OTLToolBox.OTLObjects
{
    [Serializable]
    public class OTL_ObjectType
    {
        public string Name;
        public string FriendlyName;
        public string Description;
        public string OTLName;
        public string Uri;

        public List<OTL_Parameter> Parameters; // a dictionary with parameters

        public OTL_ObjectType()
        {
            // for serialization
        }

        // contains name of object, parameters in correct notation, defaultvalues
        // everything to create a pset element
        public OTL_ObjectType(string Name, string FriendlyName, string Description, string Uri)
        {
            this.Name = Name;
            this.FriendlyName = FriendlyName;
            this.Description = Description;
            this.OTLName = "OTL_" + Name;
            this.Uri = Uri;
            Parameters = new List<OTL_Parameter>();
        }

        public void AddParameter(OTL_Parameter parameter)
        {
            Parameters.Add(parameter);
        }

        public List<OTL_Parameter> GetParameters()
        {
            return Parameters;
        }
    }
}
