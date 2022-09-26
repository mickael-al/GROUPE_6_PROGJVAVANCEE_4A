using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public class AudioManager : MonoBehaviour
    {
       private static AudioManager instance = null;
       private float volumeGlobal = 0.0f;//max 1.0f;

       public float VolumeGlobal
       {
            get
            {
                return volumeGlobal;
            }
            set
            {
                volumeGlobal = value;

            }
       }
       public static AudioManager Instance
       {
            get
            {
                return instance;
            }
       }



       private void Awake()
       {
            instance = this;
            DontDestroyOnLoad(gameObject);
            volumeGlobal = PlayerPrefs.GetFloat("AudioVoulumeGlobal",0.5f);
       }
    }
}