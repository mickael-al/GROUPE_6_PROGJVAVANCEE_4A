using UnityEngine;

namespace WJ_MCTS
{
    public class CharacterData
    {
        public Vector3 position;
        public Vector3 currentDirection;
        public float turn;
        public bool canMove = false;
        public bool handObject = false;
    }
}