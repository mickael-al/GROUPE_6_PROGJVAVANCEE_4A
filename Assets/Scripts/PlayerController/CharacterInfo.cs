using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObjects/CharacterInfo", order = 1)]
    public class CharacterInfo : ScriptableObject
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private float characterPercent = 0.5f; //pourcentage Speed - Strength

        public Texture2D Texture{
            get 
            {
                return texture;
            }
        }

        public float CharacterPercent
        {
            get{
                return characterPercent;
            }
        }
    }        
}