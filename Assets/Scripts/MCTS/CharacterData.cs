using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WJ_MCTS
{
    [Serializable]
    public class CharacterData
    {
        public Vector3S position;
        public Vector3S currentDirection;
        public float turn;
        public bool canMove = false;
        public bool handObject = false;
    }
}