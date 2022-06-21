using System;
using Autodesk.Aec.PropertyData.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using BoD_OTLToolBox.OTLObjects;

namespace BoD_OTLToolBox
{
    public class PropertySetConnector
    {
        private const string AEC_PROPERTY_SETS = "AEC_PROPERTY_SETS";
        private readonly Database acadDb;
        private ObjectId _dictPropertySet;
        private Dictionary<string, int> acadPropDictionary;
        private DictionaryPropertySetDefinitions acadPropDefinitionsDictionary;

        private StringCollection appliesto = new StringCollection() { "AecDb2dSection", "AcDbFace", "AcDbJigPreviewEntity", "AcDbJigPreviewEntityForMultiEnts", "AcDbPointCloud", "AcDbPointCloudEx", "AcMapGisGridSurfaceEntity", "CAdePreview", "CAdeTopView", "AecDbDimensionGroup", "AecDbPolygon", "AecDbContour", "AecDbCurveText", "AeccDbAlignmentLabeling", "AeccDbCSVGrid", "AeccDbEntity", "AeccDbRehabNoteLabel", "AeccDbRehabWarningMarker", "AeccDbGeo", "AeccDbGraphProfileNetworkPartBase", "AeccDbNetworkPart", "AeccDbPartProfileLabel", "AeccDbPartSectionLabel", "AeccDbPipeProfileLabelBase", "AeccDbPressureBlockPart", "AeccDbPressurePartLabel", "AeccDbPressurePartProfileLabel", "AeccDbPressurePartSectionLabel", "AeccDbPressurePipeProfileLabelBase", "AeccDbProfileBandLabeling", "AeccDbRuntimeState", "AeccDbSectionLabeling", "AeccDbSectionBandLabeling", "AeccDbTesselatedText", "AeccDbVAlignmentLabeling", "AeccDbAlignment", "AeccDbAlignmentCantLabeling", "AeccDbAlignmentCurveLabel", "AeccDbAlignmentDesignSpeedLabeling", "AeccDbAlignmentGeomPointLabeling", "AeccDbAlignmentMinorStationLabeling", "AeccDbAlignmentPILabel", "AeccDbAlignmentPILabeling", "AeccDbAlignmentIndexedPILabel", "AeccDbAlignmentSpiralLabel", "AeccDbAlignmentStaEquLabeling", "AeccDbAlignmentStationLabeling", "AeccDbStaOffsetLabel", "AeccDbAlignmentSuperelevationLabeling", "AeccDbAlignmentTable", "AeccDbAlignmentTangentLabel", "AeccDbAlignmentVAGeomPointLabeling", "AeccDbGeo_aec", "AeccDbAppurtenance", "AeccDbAppurtenanceLabel", "AeccDbAppurtenanceProfileLabel", "AcDbArc", "AcDbArcDimension", "AeccDbAssembly", "AcDbAttributeDefinition", "AeccDbAutoCorridorFeatureLine", "AeccDbAutoFeatureLine", "atraSavoyGeneralEntity", "AecDbBdgElevLine", "AecDbBdgSection", "AecDbBdgSectionLine", "AcDbBlockReference", "AcDbBody", "AeccDbBuildingSite", "AeccDbCantDiagram", "AeccDbCatchmentArea", "AeccDbCatchmentAreaLabel", "AecDbCeilingGrid", "AcDbCircle", "AeccDbContour", "AeccDbGrade", "AeccDbPoint", "AecCvDbSection", "AecDbClipVol", "AecDbClipVolRes", "AeccDbCogoPoint", "AeccuDbColModAdd", "AeccuDbColDimAngle", "AeccuDbColCircArc2d", "AeccuDbColConCoincident", "AeccuDbColConConcentric", "AeccuDbColConnector", "AeccuDbColConstraint", "AeccuDbColModCutPlane", "AeccuDbColBlock", "AeccuDbColDimDiameter", "AeccuDbColDimension", "AeccuDbColDimDistance", "AeccuDbColEllipArc2d", "AeccuDbColConEqualDistance", "AeccuDbColConEqualRadius", "AeccuDbColModExtrusion", "AeccuDbColGeometry", "AeccuDbColLayoutData", "AeccuDbColLine2d", "AeccuDbColDimMajorRadius", "AeccuDbColConMidpoint", "AeccuDbColDimMinorRadius", "AeccuDbColModelDimension", "AeccuDbColModelLimit", "AeccuDbColModifier", "AeccuDbColModGroup", "AeccuDbColDimLength", "AeccuDbColConNormal", "AeccuDbColObject", "AeccuDbColConParallel", "AeccuDbColModPath", "AeccuDbColConPerpendicular", "AeccuDbColPoint2d", "AeccuDbColProfile", "AeccuDbColDimRadius", "AeccuDbColModSubtract", "AeccuDbColConSymmetric", "AeccuDbColConTangent", "AeccuDbColModTransition", "AeccuDbColWorkPlane", "AeccuDbColWorkPlaneLimit", "AeccuDbColWorkPlaneRef", "AecDbColumnGrid", "AeccDbCorridor", "AeccDbSectionCorridor", "AeccDbCrossingPipeProfileLabel", "AeccDbCrossingPressurePipeProfileLabel", "AeccDbSectionCrossingLabel", "AeccDbCSVStationSlider", "AecDbCurtainWallLayout", "AecDbCurtainWallUnit", "AeccDbCurveScheduleTable", "AcDbDimension", "AecDbDisplayTheme", "AcMapDMLegend", "AecDbDoor", "AecDbWindowAssembly", "AcDbEllipse", "AecDbEntRef", "AeccDbFaceScheduleTable", "AeccDbFeature", "AeccDbFeatureLabel", "AeccDbAutoFeatureLabeling", "AeccDbFeatureLine", "AeccDbFitting", "AeccDbFittingLabel", "AeccDbFittingProfileLabel", "AeccDbFlowSegmentLabel", "AeccDbNoteLabel", "AeccDbGeneralLabel", "AeccDbGrading", "AeccDbGraph", "AeccDbSurfaceGrid", "AcMapGridLegendEntity", "AcDbHatch", "AcDbHelix", "AeccDbHorizontalGeometryBandLabeling", "AeccDbHydroRegion", "AeccDbIDFData", "AcDbRasterImage", "AeccDbInterference", "AeccDbInterferenceCheck", "AeccDbIntersection", "AeccDbIntersectionLocationLabel", "AeccDbIntersectionBase", "AeccDbLabel", "AeccDbLabeling", "AecDbLayoutCurve", "AecDbLayoutGrid2d", "AecDbLayoutGrid3d", "AcDbLeader", "AcMapVectorLegendEntity", "AcDbLight", "AcDbLine", "AeccDbLineBetweenPointsLabel", "AeccDbLotLineScheduleTable", "AcMapBulkFeature", "AcMapCurveFeature", "AcMapFeatureEntity", "AcMapLabels", "AcMapGisLayerBitmap", "AcMapLineString", "AcMapMultiLineString", "AcMapMultiPoint", "AcMapMultiPolygon", "AcMapPointFeature", "AcMapPointTypeFeature", "AcMapPolygon", "AcMapPolygonAsPolylineFeature", "AcMapPolylineFeature", "AcMapPolylineTypeFeature", "AcMapPreviewFeatureEntity", "AecDbMaskBlockRef", "AecDbMassElem", "AecDbMassGroup", "AeccDbMassHaulLine", "AeccDbGraphMassHaulView", "AeccDbMatchLine", "AeccDbMatchLineLabeling", "AeccDbMaterialSection", "AeccDbQuantityTakeoffAggregateTable", "AcDbMInsertBlock", "AcDbMLeader", "AcDbMline", "AcDbMPolygon", "AcDbMText", "AecDbMvBlockRef", "AeccDbNetwork", "AeccDbNetworkBase", "AeccDbNetworkPartBase", "AeccDbNetworkPartConnector", "AeccDbNetworkState", "AeccDbOffsetElevLabel", "AecDbOpening", "AeccDbFace", "AeccDbParcelBoundary", "AeccDbParcelFaceLabel", "AeccDbLotLine", "AeccDbParcelSegmentLabel", "AeccDbParcelSegmentTable", "AcDbPdfReference", "AeccDbPipe", "AeccDbPipeAndStructureTable", "AeccDbPipeLabel", "AeccDbSectionPipeNetwork", "AeccDbGraphProfileNetworkPart", "AeccDbSectionProjectionNetworkPart", "AeccDbPipeProfileLabel", "AeccDbPipeSectionLabel", "AeccDbPipeNetworkBandLabeling", "CaptureRectangle", "AcDbPoint", "AeccDbPointEnt", "AeccDbPointGroup", "AeccDbPointScheduleTable", "AcDbPolyFaceMesh", "AcDbPolygonMesh", "AcDbPolyline", "AcDb2dPolyline", "AcDb3dPolyline", "AeccDbSectionPressurePipeNetwork", "AeccDbPressurePart", "AeccDbGraphProfilePressurePart", "AeccDbPressurePartTable", "AeccDbPressurePipe", "AeccDbPressurePipeLabel", "AeccDbPressurePipeNetwork", "AeccDbPressurePipeProfileLabel", "AeccDbPressurePipeSectionLabel", "AeccDbVAlignment", "AeccDbProfileDataBandLabeling", "AeccDbGraphProfile", "AeccDbProfileViewDepthLabel", "AeccDbProfileProjection", "AeccDbProfileProjectionLabel", "AeccDbSectionProjectionLabel", "AecDbRailing", "AcDbRay", "AcDbRegion", "AeccDbRightOfWay", "AeccDbRoadwayState", "AecDbRoof", "AecDbRoofSlab", "AeccDbSampleLine", "AeccDbSampleLineGroup", "AeccDbSampleLineLabeling", "AecDbScheduleTable", "SdskCurvedText", "AeccDbSection", "AeccDbSectionCorridorPointLabeling", "AeccDbSectionDataBandLabeling", "AeccDbSectionGradeBreakLabeling", "AeccDbSectionMinorOffsetLabeling", "AeccDbSectionOffsetLabeling", "AeccDbSectionSegmentBandLabeling", "AeccDbSectionSegmentLabeling", "AeccDbGraphCrossSection", "AeccDbSectionViewDepthLabel", "AeccDbSectionViewQuantityTakeoffTable", "AeccDbSectionProjection", "AeccDbSectionalDataBandLabeling", "AeccDbSheet", "AecDbSlab", "AecDbSlice", "AcDbSolid", "AcDb3dSolid", "AecDbSpace", "AeccDbSpanningPipeLabel", "AeccDbSpanningPipeProfileLabel", "AcDbSpline", "AecDbStair", "AeccDbStaElevLabel", "AecsDbMember", "AeccDbStructure", "AeccDbStructureLabel", "AeccDbStructureProfileLabel", "AeccDbStructureSectionLabel", "AeccDbSubassembly", "AeccDbSuperElevationBandLabeling", "AeccDbSuperelevationDiagram", "AcDbSurface", "AeccDbSurface", "AcDbExtrudedSurface", "AcDbPlaneSurface", "AcDbRevolvedSurface", "AcDbSweptSurface", "AeccDbSurfaceContourLabeling", "AeccDbSurfaceElevLabel", "AeccDbLegendScheduleTable", "AeccDbSurfaceSlopeLabel", "AeccDbSvFigure", "AeccDbSvFigureLabel", "AeccDbSvFigureSegmentLabel", "AeccDbSvNetwork", "AeccDbScheduleTable", "AcDbTable", "AeccDbTangentIntersectionTable", "AcDbText", "AeccDbSurfaceTin", "AcDbFcf", "AcDbTrace", "atraAirportStand", "atraJunction", "atraParkingData", "atraPathData", "AeccDbVAlignmentCrestCurveLabeling", "AeccDbVAlignmentHAGeomPointLabeling", "AeccDbVAlignmentLineLabeling", "AeccDbVAlignmentMinorStationLabeling", "AeccDbVAlignmentPVILabeling", "AeccDbVAlignmentSagCurveLabeling", "AeccDbVAlignmentStationLabeling", "AeccDbVerticalGeometryBandLabeling", "AeccDbViewFrame", "AeccDbViewFrameGroup", "AeccDbViewFrameLabeling", "AcDbViewport", "AecDbWall", "AecDbWindow", "AcDbXline", "AecDbZone" };

        public PropertySetConnector(Database db)
        {
            acadDb = db;
            acadPropDefinitionsDictionary = new DictionaryPropertySetDefinitions(db);
        }


        public void SetPropertySetName(string propertySetName)
        {
            using (var tr = acadDb.TransactionManager.StartOpenCloseTransaction())
            {
                if (acadPropDefinitionsDictionary.Has(propertySetName, tr))
                {
                    _dictPropertySet = acadPropDefinitionsDictionary.GetAt(propertySetName);
                    var psd = (PropertySetDefinition)tr.GetObject(_dictPropertySet, OpenMode.ForRead);
                    acadPropDictionary = psd.Definitions.Cast<PropertyDefinition>().ToDictionary(pd => pd.Name, pd => pd.Id);
                }
                tr.Commit();  //Because its faster than aborting
            }
        }

        public string GetEntityPropertySet(Entity ent, string searchContains, List<string> SearchOmitsList)
        {
            string result = "";
            
            using (var tr = acadDb.TransactionManager.StartOpenCloseTransaction())
            {
                ObjectIdCollection setIds = PropertyDataServices.GetPropertySets(ent);
                foreach (ObjectId psId in setIds)
                {
                    PropertySet pset = tr.GetObject(psId, OpenMode.ForRead, false, false) as PropertySet;
                    if(pset.PropertySetDefinitionName.Contains(searchContains))
                    {
                        Boolean containsOmitList = true;
                        foreach (string tester in SearchOmitsList)
                        {
                            if (pset.PropertySetDefinitionName.Contains(tester))
                                containsOmitList = false;
                        }

                        if(containsOmitList)
                            result = pset.PropertySetDefinitionName;
                    }              
                }
                    tr.Commit();  //Because its faster than aborting
            }
            return result;
        }

        public List<string> GetPropertyListing(Entity entity)
        {
            if (_dictPropertySet == ObjectId.Null || entity.Database != acadDb ||
                entity.ExtensionDictionary == ObjectId.Null)
                return null;

            if (acadPropDictionary == null)
                return null;

            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, int> item in acadPropDictionary)
            {
                temp.Add(item.Key);
            }

            return temp;
        }

        public string GetProperty(Entity entity, string propertyName)
        {
            if (_dictPropertySet == ObjectId.Null || entity.Database != acadDb ||
                entity.ExtensionDictionary == ObjectId.Null || string.IsNullOrEmpty(propertyName))
                return default(string);

            if (acadPropDictionary == null || !acadPropDictionary.ContainsKey(propertyName))
                return default(string);

            var index = acadPropDictionary[propertyName];
            var result = default(string);
            using (var tr = acadDb.TransactionManager.StartOpenCloseTransaction())
            {
                var xDict = (DBDictionary)tr.GetObject(entity.ExtensionDictionary, OpenMode.ForRead);
                if (xDict.Contains(AEC_PROPERTY_SETS))
                {
                    var psDict = (DBDictionary)tr.GetObject(xDict.GetAt(AEC_PROPERTY_SETS), OpenMode.ForRead);
                    var propertySet = psDict.Cast<DictionaryEntry>()
                        .Select(e => (PropertySet)((ObjectId)e.Value).GetObject(OpenMode.ForRead))
                        .FirstOrDefault(ps => ps.PropertySetDefinition == _dictPropertySet);
                    if (propertySet != null)
                    {
                        var propData = propertySet.PropertySetData.Cast<PropertySetData>().FirstOrDefault(d => d.Id == index);
                        if (propData != null)
                        {
                            var temp = propData.GetData();
                            result = Convert.ToString(temp); // all property values get converted to string here, it is easier, casting can be done later.
                        }                           
                    }
                }
                tr.Commit();
            }
            return result;
        }

        // sets a property on an entity in the drawing
        public void SetProperty(Entity entity, string propertyName, object value)
        {
            if (_dictPropertySet == ObjectId.Null || entity.Database != acadDb || string.IsNullOrEmpty(propertyName))
                return;

            if (acadPropDictionary == null || !acadPropDictionary.ContainsKey(propertyName))
                return;

            var index = acadPropDictionary[propertyName];
            using (var tr = acadDb.TransactionManager.StartOpenCloseTransaction())
            {
                if (entity.ExtensionDictionary == ObjectId.Null)
                {
                    entity.UpgradeOpen();
                    PropertyDataServices.AddPropertySet(entity, _dictPropertySet);
                }

                var xDict = (DBDictionary)tr.GetObject(entity.ExtensionDictionary, OpenMode.ForRead);
                if (!xDict.Contains(AEC_PROPERTY_SETS))
                {
                    entity.UpgradeOpen();
                    PropertyDataServices.AddPropertySet(entity, _dictPropertySet);
                }
                var psDict = (DBDictionary)tr.GetObject(xDict.GetAt(AEC_PROPERTY_SETS), OpenMode.ForRead);
                var propertySet = psDict.Cast<DictionaryEntry>()
                    .Select(e => (PropertySet)((ObjectId)e.Value).GetObject(OpenMode.ForRead))
                    .FirstOrDefault(ps => ps.PropertySetDefinition == _dictPropertySet);
                if (propertySet == null)
                {
                    entity.UpgradeOpen();
                    PropertyDataServices.AddPropertySet(entity, _dictPropertySet);
                }

                propertySet = psDict.Cast<DictionaryEntry>()
                    .Select(e => (PropertySet)((ObjectId)e.Value).GetObject(OpenMode.ForRead))
                    .FirstOrDefault(ps => ps.PropertySetDefinition == _dictPropertySet);
                if (propertySet != null)
                {
                    propertySet.UpgradeOpen();
                    var propData = propertySet.PropertySetData.Cast<PropertySetData>().FirstOrDefault(d => d.Id == index);
                    if (propData != null)
                        propData.SetData(value);
                }

                tr.Commit();
            }
        }

        // get the ID of a property set by name
        public ObjectId GetPropertySetDefinitionIdByName(string PropertySetName)
        {
            // Create objectId to hold the property set objectId and set the default value
            // equal to null.  
            ObjectId PropertySetId = ObjectId.Null;
            Database db = acadDb;
            TransactionManager tm = db.TransactionManager;
            using (Transaction trans = tm.StartTransaction())
            {
                // Create a Property Set Dictionary
                DictionaryPropertySetDefinitions psdDict = new DictionaryPropertySetDefinitions(db);

                // Check to see if the dictionary contains the name of the property set we are looking for
                if (psdDict.Has(PropertySetName, trans))
                {
                    // Assign the return variable the ObjectId of the property set
                    PropertySetId = psdDict.GetAt(PropertySetName);
                } // End If Block

                trans.Commit();
            } 

            return PropertySetId;
        }

        // check if a certain property set is attached to an object, only used to avoid errors when polling properties
        public bool IsPropertySetAttachedToObject(DBObject ent, ObjectId PropertySetId)
        {

            // Create and set a temporary ObjectID to Null
            ObjectId TempId = ObjectId.Null;

            try
            {
                // Attempt to set the temporary ObjectID equal to the objectID of the 
                // of the property set we are attempting to find on the object.
                // If the property set exists on the object then the temporary ObjectId
                // will be assigned a value
                TempId = PropertyDataServices.GetPropertySet(ent, PropertySetId);
            } // End Try Block

            // If the property set does not exist on the object then an error will be raised
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                // Catching an error is not the best way to find out if a property set is
                // Attached to an object. A better way would be to query the objects extension
                // dictionary and see if the property set is in the extension dictionary
            } // End Catch Block

            // If the temp ObjectId is not null then we found the property set on the object
            if (!TempId.IsNull)
                return true;

            // If we are at this point then the property set was not found on the object
            // and we can return false.
            return false;

        }

        // create a property in a property set definition in the propertysetdefine window
        private PropertyDefinition CreateProperty(OTL_Parameter Parameter)
        {
            // create a manual property
            PropertyDefinition prop = new PropertyDefinition();          
            //set parameter name
            prop.Name = Parameter.dotNotationFullName;
            //set parameter description
            prop.Description = Parameter.description;
            
            if(Parameter.DataType == Autodesk.Aec.PropertyData.DataType.List)
            {
                Autodesk.Aec.DatabaseServices.ListDefinition listdef = new Autodesk.Aec.DatabaseServices.ListDefinition();
                listdef.AppliesToFilter = new StringCollection() { "AecListUserManualPropertyDef" };
                listdef.Description = "defaultdescription";
                listdef.SetToStandard(acadDb);
                listdef.SubSetDatabaseDefaults(acadDb);
                listdef.AlternateName = "alternatename";
                listdef.IsLocked = true;

                Autodesk.Aec.DatabaseServices.DictionaryListDefinition dict0 = new Autodesk.Aec.DatabaseServices.DictionaryListDefinition(acadDb);
                dict0.AddNewRecord(Guid.NewGuid().ToString(), listdef);

                using (OpenCloseTransaction trans = acadDb.TransactionManager.StartOpenCloseTransaction())
                {
                    trans.AddNewlyCreatedDBObject(listdef, true);

                    foreach (string item in Parameter.DropdownValues)
                    {
                        if(item.GetType() != (System.DBNull.Value.GetType()) && item != null)
                            listdef.AddListItem(item);
                    }
                    trans.Commit();
                }
                prop.ListDefinitionId = listdef.Id;            
            }

            prop.SetToStandard(acadDb);
            prop.SubSetDatabaseDefaults(acadDb);
            prop.DataType = Parameter.DataType;
            prop.DefaultData = Parameter.Defaultvalue;

            return prop;
        }

        // create a new property set in propertysetdefine windows
        public void CreatePropertySetDefinition(OTL_ObjectType otlobject)
        {
            TransactionManager tm = acadDb.TransactionManager;

            // create new property set definition
            PropertySetDefinition propSet = new PropertySetDefinition();
            propSet.SetToStandard(acadDb);
            propSet.SubSetDatabaseDefaults(acadDb);

            if(otlobject.Description.Length > 254)
            {
                propSet.Description = otlobject.Description.Substring(0,254);
            } else
            {
                propSet.Description = otlobject.Description;
            }


            // apply to object tab          
            propSet.SetAppliesToFilter(appliesto, false);
            
            // iterate over properties
            foreach (OTL_Parameter parameter in otlobject.GetParameters())
            {
                try
                {
                    propSet.Definitions.Add(CreateProperty(parameter));
                } catch
                {
                    // duplicate parameter exception
                }
                
            }

            // add the property set to the database
            using (Transaction trans = tm.StartTransaction())
            {
                // check the name
                DictionaryPropertySetDefinitions PropDefs = new DictionaryPropertySetDefinitions(acadDb);    
                if(PropDefs.Has(otlobject.OTLName,trans))
                {
                    // cannot create!
                } else
                {
                    PropDefs.AddNewRecord(otlobject.OTLName, propSet);
                    trans.AddNewlyCreatedDBObject(propSet, true);
                    trans.Commit();
                }

            }

        }

        // attach a property set to an object (currently unused)
        public bool CreatePropertySetOnObject(ObjectId ObjectID, ObjectId PropertySetId)
        {
            Database db = acadDb;
            TransactionManager tm = db.TransactionManager;
            using (Transaction trans = tm.StartTransaction())
            {
                // Try to attach the property set to the object.
                try
                {
                    // First check to see if the property set already exists on the object

                    // create a database object based on the ObjectId of the object
                    Entity dbObject = (Entity)tm.GetObject(ObjectID, OpenMode.ForRead, true);

                    // Check to see if the property set is already attached to the object
                    if (!IsPropertySetAttachedToObject(dbObject, PropertySetId))
                    {
                        // Attach the property set to the object
                        DBObject dbobj = tm.GetObject(ObjectID, OpenMode.ForWrite, false, false);
                        PropertyDataServices.AddPropertySet(dbobj, PropertySetId);
                    } // End If Block
                }  // End Try Block

                // If there is an error catch it and return false
                catch (Autodesk.AutoCAD.Runtime.Exception)
                {
                    return false;
                }  // End Catch Block

                // At this point the property set was successfully attached so commit the transaction
                trans.Commit();
            }  // End Try-Catch Block

            // The property set has been attached and the transaction committed
            // So we can finish up and return true
            return true;

        }

    }
}
