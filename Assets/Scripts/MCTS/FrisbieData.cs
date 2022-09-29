using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;

namespace WJ_MCTS
{
    [Serializable]
    public class FrisbiData
    {
        public Vector3S position;
        public Vector3S currentDirection;
        public ThrowMode throwMode = ThrowMode.Throw;
        public bool move = false;
    }
}
