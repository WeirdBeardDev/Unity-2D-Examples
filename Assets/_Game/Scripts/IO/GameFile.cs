using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpaceMonkeys.IO
{
    public static class GameFile
    {
        #region Private Members
        private static readonly string fileName = @"SaveGame.smd"; // Update name as needed
        private static readonly string fullPath = Path.Combine(Application.persistentDataPath, fileName);
        #endregion Private Members

        #region Public Properties
        public static bool SaveFileExists => File.Exists(fullPath);
        #endregion Public Properties

        #region Public Methods
        public static void SaveGame(SaveData data)
        {
            // DebugHelper("Save", data); // * temp method used for testing purposes

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create(fullPath))
            {
                formatter.Serialize(stream, data);
            }
        }
        public static SaveData LoadGame()
        {
            var data = new SaveData();
            if (File.Exists(fullPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    data = (SaveData)formatter.Deserialize(stream);
                }

                DebugHelper("Load", data); // * temp method used for testing purposes
            }
            else
            {
                Debug.Log($"No save file found, reset the game.");
                data = null;
            }

            return data;
        }
        public static void DeleteSave()
        {
            // TODO: make this more robust in the future
            try
            {
                File.Delete(fullPath);
            }
            finally { }
        }
        #endregion Public Methods

        #region Private Methods
        private static void DebugHelper(string methodName, SaveData data)
        {
            // UnityEngine.Debug.Log($"{methodName}:Zones:Count {data.Zones.Count}");
            // foreach (var item in data.Zones)
            // {
            //     UnityEngine.Debug.Log($"{methodName}:Zone Active: {item.ZoneType} - {item.IsZoneActive}");
            //     foreach (var q in item.Quests)
            //     {
            //         UnityEngine.Debug.Log($"  Quest: {q.Name}, Cxp: {q.CurrentExperience}; Axp: {q.AllTimeExperience}");
            //     }
            // }
        }
        #endregion Private Methods
    }
}
