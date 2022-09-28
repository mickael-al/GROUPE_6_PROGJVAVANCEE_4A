using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_MCTS
{
    public class GameState 
    {
        private GameManagerData gameManagerData = new GameManagerData();
        private FrisbiData frisbiData = new FrisbiData();
        private CharacterData[] characterData = new CharacterData[2];

        #region Getter
        public  CharacterData[] characterDatas { get {return characterData;} }
        public GameManagerData GameManagerData { get {return gameManagerData;} }
        public FrisbiData FrisbiData { get {return frisbiData;} }
        #endregion
    }
}
