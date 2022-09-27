using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace WJ
{
    public class UIPersonnage : MonoBehaviour
    {
        [SerializeField] private GameObject LeftBoard = null;
        [SerializeField] private GameObject RightBoard = null;
        [SerializeField] private GameObject prefabsLoadUnitCharacter = null;
        [SerializeField] private GameObject prefabCameraRenderer = null;
        [SerializeField] private TMP_Dropdown characterMode = null;
        private List<GameObject> cameraObject = new List<GameObject>();
        private List<RenderTexture> renderTextures = new List<RenderTexture>();
        private List<Image> choixLeft = new List<Image>();
        private List<Image> choixRight = new List<Image>();

        public void Start()
        {
            float decalage = 0.0f;
            GameObject obj = null;
            RenderTexture rt;
            int id = 0;
            foreach(CharacterInfo ci in RessourceManager.Instance.CharacterInfos)
            {
               cameraObject.Add(obj = Instantiate(prefabCameraRenderer,new Vector3(decalage,1000.0f,0.0f),Quaternion.identity));
               renderTextures.Add(rt = new RenderTexture(256,256,16,RenderTextureFormat.ARGB32));
               obj.GetComponent<Camera>().targetTexture = rt;
               obj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap",ci.Texture);
               for(int i = 0 ; i < 2;i++)
               {
                    obj = Instantiate(prefabsLoadUnitCharacter,Vector3.zero,Quaternion.identity, i == 0 ? LeftBoard.transform : RightBoard.transform);
                    obj.transform.GetChild(0).GetComponent<RawImage>().texture = rt;
                    obj.transform.GetChild(2).GetComponent<Slider>().value = ci.CharacterPercent;
                    EventTrigger et = obj.AddComponent<EventTrigger>();
                    if(i==0)
                    {
                        choixLeft.Add(obj.transform.GetChild(3).GetComponent<Image>());
                    }
                    else
                    {
                        choixRight.Add(obj.transform.GetChild(3).GetComponent<Image>());
                    }
                    EventTrigger.Entry entryClick = new EventTrigger.Entry();
                    entryClick.eventID = EventTriggerType.PointerClick;
                    int nid = id;
                    Faction faction = i == 0 ? Faction.Left : Faction.Right;
                    entryClick.callback.AddListener((data) =>
                    {
                        Choix(nid,faction);
                    });
                    et.triggers.Add(entryClick);
               }
               decalage+=100.0f;
               id++;
            }
        }

        public void Choix(int idCharacter,Faction type)
        {
            if(Faction.Left == type)
            {
                foreach (var item in choixLeft)
                {
                    item.enabled = false;
                }
                choixLeft[idCharacter].enabled = true;
                GameManager.IndicePlayerLeft = idCharacter;
            }
            else
            {
                foreach (var item in choixRight)
                {
                    item.enabled = false;
                }
                choixRight[idCharacter].enabled = true;
                GameManager.IndicePlayerRight = idCharacter;
            }
        }

        public void OnCharacterModeChange()
        {
            GameManager.CharacterModeRight = (CharacterMode)characterMode.value;
            Debug.Log((CharacterMode)characterMode.value);
        }
    }   
}
