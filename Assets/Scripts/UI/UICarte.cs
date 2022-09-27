using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WJ
{

    public class UICarte : MonoBehaviour
    {
        [SerializeField] private GameObject ContentObj = null;
        [SerializeField] private GameObject prefabUnitCarte = null;
        [SerializeField] private List<Texture2D> carteTexture = new List<Texture2D>();
        private List<Image> chekMark = new List<Image>();
        public void Start() 
        {
            GameObject obj = null;
            int id = 0;
            foreach(Texture2D t in carteTexture)
            {
                obj = Instantiate(prefabUnitCarte,Vector3.zero,Quaternion.identity,ContentObj.transform);
                obj.GetComponent<RawImage>().texture = t;
                chekMark.Add(obj.transform.GetChild(0).GetComponent<Image>());
                EventTrigger et = obj.AddComponent<EventTrigger>();
                EventTrigger.Entry entryClick = new EventTrigger.Entry();
                entryClick.eventID = EventTriggerType.PointerClick;
                int nid = id;
                entryClick.callback.AddListener((data) =>
                {
                    Choix(nid);
                });
                et.triggers.Add(entryClick);
                id++;
            }
        }

        public void Choix(int idCarte)
        {
            foreach(var item in chekMark)
            {
                item.enabled = false;   
            }
            chekMark[idCarte].enabled = true;
            GameManager.IndiceMap = idCarte;
        }
    }
}