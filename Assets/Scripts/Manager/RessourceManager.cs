using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public class RessourceManager : MonoBehaviour
    {
        [SerializeField] private List<CharacterInfo> characterInfos = new List<CharacterInfo>();
        public static RessourceManager Instance { get; private set; }

        public List<CharacterInfo> CharacterInfos
        {
            get{
                return characterInfos;
            }
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
