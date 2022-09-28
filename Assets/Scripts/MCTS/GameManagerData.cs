using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_MCTS
{
    public class GameManagerData
    {
        public bool endGame = false;
        public bool endSet = false;
        public float gameTime = 0.0f;
        public int scoreRight = 0;
        public int scoreLeft = 0;
        public int SetLeftCount = 0;
        public int SetRightCount = 0;
        public float timeNextSet = 0.0f;
        public int round = 0;
        public bool isCurrentGame = false;

    }
}
