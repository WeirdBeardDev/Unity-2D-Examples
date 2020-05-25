using System.Collections.Generic;

namespace SpaceMonkeys.IO
{
    [System.Serializable]
    public class SaveData
    {
        /* This represents all the data needed for saving/loading the game
        ** Need to save:
        */
        #region Members
        public float SaveFormat = 0.1f;
        // TODO add data for saving
        #endregion Members

        #region Ctors
        public SaveData() { } // for loading a game
        // TODO update saving constructor to take parameters for saving
        public SaveData(int x) // for saving a game
        {
            // TODO add code to save game state
        }
        #endregion Ctors
    }
}