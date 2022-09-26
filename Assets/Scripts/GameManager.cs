using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class GameManager : MonoBehaviour
    {
        public GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        private static GameManager instance = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {   
            
        }

        void Update()
        {
            
        }
    }
}