using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace BoD_OTLToolBox
{
    public class Commands : IExtensionApplication
    {

        public void Initialize()
        {
            ActionHandler.Init();
        }
        public void Terminate()
        {
         // nothing here
        }

        [CommandMethod("OTLIMPORTDB")]
        public void OTLAssignNode()
        {
            ActionHandler.UI_Import_OTLDB();
        }
        [CommandMethod("OTLUPDATELISTS")]
        public void OTLUpdateLists()
        {
            // ActionHandler.UI_Import_OTLDB();
        }
        [CommandMethod("OTLUPDATEDB")]
        public void OTLUpdateNode()
        {
            // ActionHandler.UI_Import_OTLDB();
        }
    }
}
