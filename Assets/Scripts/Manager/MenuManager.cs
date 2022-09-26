using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;

        void Start()
        {
        
        }
        void Update()
        {

        }

        public void Option()
        {
            animator.SetTrigger("Option");
        }

    }
}