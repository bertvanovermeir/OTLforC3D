using Autodesk.AutoCAD.DatabaseServices;
using BoD_OTLToolBox.Plugin;
using System.Collections.Generic;

namespace BoD_OTLToolBox
{
    public class OTL_Entity
    {
        public PropertySetConnector propertyset; // property set helper     
        public Entity entity;
        public string AimId;
        public bool isValidOTLObject = true;        
        public string propertySetName;        
        public Dictionary<string, string> propertyPairs;

        // An OTL entity needs to be constructed within a database transaction.
        public OTL_Entity(Database aidb, Entity entity)
        {
            this.entity = entity;
            propertyPairs = new Dictionary<string, string>();
            // init the property set
            propertyset = new PropertySetConnector(aidb);
            // fill all values if OTL
            FillValues(entity);
        }

        private void FillValues(Entity entity)
        {
            List<string> properties = new List<string>();
            // some exceptions for words template and postnummer which should never be part of the OTL class.
            propertySetName = propertyset.GetEntityPropertySet(entity, DataHandler.settings.ReadSetting("OTL_PROPERTY_SET_PREFIX"), new List<string> { "emplate", "ostnummer" });
            propertyset.SetPropertySetName(propertySetName);
            properties = propertyset.GetPropertyListing(entity);

            if(properties != null)
            {
                foreach (string property in properties)
                {
                    string value = propertyset.GetProperty(entity, property);
                    propertyPairs.Add(property, value);
                }
                // base info, if the info cannot be retrieved, make the object invalid nevertheless.
                AimId = propertyset.GetProperty(entity, DataHandler.settings.ReadSetting("OTL_PROPERTY_ASSET_ID"));
                if (AimId == null)
                {
                    isValidOTLObject = false;
                }
            } 
            else
            {
                isValidOTLObject = false;
            }            
        }
    }
}
