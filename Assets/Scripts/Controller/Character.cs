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
        protected float Speed;
        protected float Strength;
        protected bool canMove = false;
        protected bool isInit = false;
        protected bool handObject = false;
        protected FrisbieController frisbie = null;
        private Vector3 spawnPosition = Vector3.zero;
        protected float rayon = 1.0f;
        protected Vector2 terrainCalcule = Vector2.zero;

        public bool CanMove
        {
            get{return canMove;}
            set{canMove = true;}
        }

        public CharacterMode CharacterMode
        {
            get{return characterMode;}
        }

        public virtual void InitCharacter(CharacterInfo ci,Faction f)
        {
            this.characterInfo = ci;
            isInit = true;
            faction = f;
            transform.eulerAngles = new Vector3(0,Faction.Left == faction ? 90 : -90,0);
            animator = transform.GetChild(0).GetComponent<Animator>();
            transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap",ci.Texture);
            Speed = ci.CharacterPercent*25.0f;
            Strength = (1.0f-ci.CharacterPercent)*25.0f;
            frisbie = GameManager.Instance.Frisbie;
        }

        public void ResetSpawnPosition()
        {
            transform.position = spawnPosition;
            transform.eulerAngles = new Vector3(0,Faction.Left == faction ? 90 : -90,0);
            handObject = false;
        }

        public void Start()
        {
            spawnPosition = transform.position;
            terrainCalcule = GameManager.Instance.TerrainSize/2.0f;
        }

        public void TakeFrisbie()
        {
            if(frisbie.Moves && !handObject && Vector3.Distance(transform.position,frisbie.transform.position) < rayon+frisbie.Rayon)
            {
                handObject = true;
                frisbie.transform.position = transform.position + (Faction.Left == faction ? Vector3.right*(rayon+frisbie.Rayon):Vector3.left*(rayon+frisbie.Rayon));
                frisbie.Stop();
            }
        }

        public void BoardCollision()
        {
            if(Faction.Left == faction)
            {
                if(transform.position.x < -terrainCalcule.x)
                {
                    transform.position = new Vector3(-terrainCalcule.x,transform.position.y,transform.position.z);
                }
                if(transform.position.x > 0)
                {
                    transform.position = new Vector3(0.0f,transform.position.y,transform.position.z);
                }
            }
            else
            {
                if(transform.position.x > terrainCalcule.x)
                {
                    transform.position = new Vector3(terrainCalcule.x,transform.position.y,transform.position.z);
                }
                if(transform.position.x < 0)
                {
                    transform.position = new Vector3(0.0f,transform.position.y,transform.position.z);
                }
            }
            if(transform.position.z > terrainCalcule.y)
            {
                transform.position = new Vector3(transform.position.x,transform.position.y,terrainCalcule.y);
            }
            if(transform.position.z < -terrainCalcule.y)
            {
                transform.position = new Vector3(transform.position.x,transform.position.y,-terrainCalcule.y);
            }
        }

        public virtual void Update()
        {
            if(!canMove || !isInit)
            {
                return;
            }

        }
    }
}