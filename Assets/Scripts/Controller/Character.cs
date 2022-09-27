using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public abstract class Character : Entity
    {
        protected CharacterInfo characterInfo;
        protected Animator animator = null;
        protected CharacterMode characterMode;
        protected Faction faction;
        protected bool canMove = false;
        protected bool isInit = false;
        private Vector3 spawnPosition = Vector3.zero;

        public bool CanMove
        {
            get{return canMove;}
            set{canMove = true;}
        }

        public CharacterMode CharacterMode
        {
            get{return characterMode;}
        }

        public void InitCharacter(CharacterInfo ci,Faction f)
        {
            this.characterInfo = ci;
            isInit = true;
            faction = f;
            transform.eulerAngles = new Vector3(0,Faction.Left == faction ? 90 : -90,0);
            animator = transform.GetChild(0).GetComponent<Animator>();
            transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap",ci.Texture);
        }

        public void ResetSpawnPosition()
        {
            transform.position = spawnPosition;
            transform.eulerAngles = new Vector3(0,Faction.Left == faction ? 90 : -90,0);
        }

        public void Start()
        {
            spawnPosition = transform.position;
        }

        public void Update()
        {
            if(!canMove || !isInit)
            {
                return;
            }

        }
    }
}