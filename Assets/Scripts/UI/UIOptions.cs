using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WJ
{
    public class UIOptions : MonoBehaviour
    {
        [Header("UI Element")]
        [SerializeField] private Slider sliderVolume = null;
        [SerializeField] private TMP_Dropdown resolution = null;
        [SerializeField] private TMP_Dropdown format = null;
        [SerializeField] private Toggle fullScreen = null;
        private readonly float[] ratio =
        {
            4.0f/3.0f,
            16/9.0f,
            16/10.0f
        };

        private readonly int[] resolutionPixel =
        {
            240,
            360,
            480,
            720,
            1080,
            1440,
            2160
        };

        public void Start()
        {
            //Application.targetFrameRate = 60;
            resolution.value = PlayerPrefs.GetInt("resolutionId",4);
            format.value = PlayerPrefs.GetInt("RatioId",1);
            fullScreen.isOn = PlayerPrefs.GetInt("FullScreen",1) == 1;
            sliderVolume.value = AudioManager.Instance.Volume;
            ScreenResolution();
        }

        public void ScreenResolution()
        {
            float width = ratio[format.value]*resolutionPixel[resolution.value];
            Screen.SetResolution((int)width, resolutionPixel[resolution.value], fullScreen.isOn);
            //Debug.Log(width + " " + resolutionPixel[resolution.value] + " " + fullScreen.isOn);
        }

        public void OnChangeVolume()
        {
            AudioManager.Instance.Volume = sliderVolume.value;
        }

        public void SaveOption()
        {
            PlayerPrefs.SetFloat("VolumeGlobal",sliderVolume.value);
            PlayerPrefs.SetInt("resolutionId",resolution.value);
            PlayerPrefs.SetInt("RatioId",format.value);
            PlayerPrefs.SetInt("FullScreen",fullScreen.isOn ? 1 : 0);
        }
    }
}