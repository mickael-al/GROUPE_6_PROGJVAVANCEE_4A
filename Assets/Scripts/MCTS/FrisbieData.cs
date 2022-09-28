using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_MCTS
{
    public class FrisbiData
    {
        public Vector3 position;
        public Vector3 currentDirection;
        public ThrowMode throwMode = ThrowMode.Throw;
        public bool move = false;
    }
}
