using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;

namespace WJ_MCTS
{
    [Serializable]
    public class GameState 
    {
        [SerializeField] private GameManagerData gameManagerData = new GameManagerData();
        [SerializeField] private FrisbiData frisbiData = new FrisbiData();
        [SerializeField] private CharacterData[] characterData = new CharacterData[2];

        #region Getter
        public  CharacterData[] characterDatas { get {return characterData;} }
        public GameManagerData GameManagerData { get {return gameManagerData;} }
        public FrisbiData FrisbiData { get {return frisbiData;} }
        #endregion
    }
}
