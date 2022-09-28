using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_MCTS
{
    public class GameState 
    {
        private GameManagerData gameManagerData;
        private FrisbiData frisbiData;
        private KeyValuePair<Character,CharacterData>[] characterData = new KeyValuePair<Character,CharacterData>[2];

        #region Getter
        public  KeyValuePair<Character,CharacterData>[] CharacterDataDictionary { get {return characterData;} }
        public GameManagerData GameManagerData { get {return gameManagerData;} }
        public FrisbiData FrisbiData { get {return frisbiData;} }
        #endregion
    }
}
