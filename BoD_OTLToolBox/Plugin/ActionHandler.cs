using BoD_OTLToolBox.Plugin;
using System.IO;
using BoD_OTLToolBox.OTLObjects;

namespace BoD_OTLToolBox
{
    public static class ActionHandler
    {
        /// <summary>
        /// create a new ActionHandler instance. init all the data associated.
        /// </summary>
        public static void Init()
        {
            DataHandler.Init();
        }

        /// <summary>
        /// UI handle for loading a new OTL SQL database
        /// </summary>
        public static void UI_Import_OTLDB()
        {
            // refresh the active document 
            DataHandler.Refresh_DB();
            // request database path from userfile
            string sqlpath = DataHandler.settings.ReadSetting("SQL_PATH");
            string ttlpath = DataHandler.settings.ReadSetting("TTL_PATH");

            if (File.Exists(sqlpath) && Directory.Exists(ttlpath) && !IsFileLocked(sqlpath))
            {
                Import_OTLDB();
            }
            else
            {
                // files not found
            }
        }


        /// <summary>
        /// NON UI handle for directly loading OTL DB, does not provide user feedback
        /// </summary>
        private static void Import_OTLDB()
        {
            // refresh the active document 
            DataHandler.Refresh_DB();

            DataHandler.InitDatabase(DataHandler.settings.ReadSetting("SQL_PATH"));
            DataHandler.Import_OTLDB();
        }

        /// <summary>
        /// check if a file is locked by quickly trying to open it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsFileLocked(string path)
        {
            FileInfo file = new FileInfo(path);
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            //file is not locked
            return false;
        }
    }
}
