using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ_MCTS;

namespace WJ
{
    public abstract class Character : Entity
    {
        protected CharacterInfo characterInfo;//Scriptable Data
        protected Animator animator = null;
        protected CharacterMode characterMode;
        protected Faction faction;
        protected int factionId = 0;
        protected float Speed;
        protected float Strength;
        protected GameState gameState = null;
        private Vector3 spawnPosition = Vector3.zero;
        protected float rayon = 1.0f;
        protected Vector2 terrainCalcule = Vector2.zero;
        protected Vector3 workPosition;

        public CharacterMode CharacterMode
        {
            get{return characterMode;}
        }

        public virtual void InitCharacter(CharacterInfo ci,GameState gs,Faction f,Vector2 terrainSize,Vector3 spawnPos)
        {
            this.terrainCalcule = terrainSize/2.0f;
            this.characterInfo = ci;
            this.faction = f;
            this.factionId = (int)f;
            gs.characterDatas[factionId].turn = Faction.Left == faction ? 90 : -90;
            this.animator = transform.GetChild(0).GetComponent<Animator>();
            this.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap",ci.Texture);
            this.Speed = (ci.CharacterPercent*10.0f)+10.0f;
            this.Strength = ((1.0f-ci.CharacterPercent)*10.0f)+10.0f;
            this.gameState = gs;
            this.spawnPosition = spawnPos;
            gs.characterDatas[factionId].position = this.spawnPosition;
        }

        public void ResetSpawnPosition(GameState gs)
        {
            gs.characterDatas[factionId].position = spawnPosition;
            gs.characterDatas[factionId].turn = Faction.Left == faction ? 90 : -90;
            gs.characterDatas[factionId].handObject = false;
        }

        public void TakeFrisbie(GameState gs)
        {
            float r = GameManager.Instance.Frisbie.Rayon; 
            if(gs.FrisbiData.move && !gs.characterDatas[factionId].handObject && Vector3.Distance(gs.characterDatas[factionId].position,gs.FrisbiData.position) < rayon+r)
            {
                gs.characterDatas[factionId].handObject = true;
                gs.FrisbiData.position = gs.characterDatas[factionId].position + (Faction.Left == faction ? Vector3.right*(rayon+r):Vector3.left*(rayon+r));
                GameManager.Instance.Frisbie.Stop(gs.FrisbiData);
            }
        }

        public void Action1(GameState state,Vector3 dir)
        {
            if(state.characterDatas[factionId].handObject)
            {
                GameManager.Instance.Frisbie.Throw(state.FrisbiData,dir,Strength);
                state.characterDatas[factionId].handObject = false;
            }
        }

        public void BoardCollision(GameState gs)
        {
            workPosition = gs.characterDatas[factionId].position;
            if(Faction.Left == faction)
            {
                if(workPosition.x < -terrainCalcule.x)
                {
                    workPosition = new Vector3(-terrainCalcule.x,workPosition.y,workPosition.z);
                }
                if(workPosition.x > 0)
                {
                    workPosition = new Vector3(0.0f,workPosition.y,workPosition.z);
                }
            }
            else
            {
                if(workPosition.x > terrainCalcule.x)
                {
                    workPosition = new Vector3(terrainCalcule.x,workPosition.y,workPosition.z);
                }
                if(workPosition.x < 0)
                {
                    workPosition = new Vector3(0.0f,workPosition.y,workPosition.z);
                }
            }
            if(workPosition.z > terrainCalcule.y)
            {
                workPosition = new Vector3(workPosition.x,workPosition.y,terrainCalcule.y);
            }
            if(workPosition.z < -terrainCalcule.y)
            {
                workPosition = new Vector3(workPosition.x,workPosition.y,-terrainCalcule.y);
            }
            gs.characterDatas[factionId].position = workPosition;
        }

        public void TranslatePosition(GameState gs,float dt)
        {
            TakeFrisbie(gs);
            if(!gs.characterDatas[factionId].canMove || gs.characterDatas[factionId].handObject)
            {
                return;
            }
            gs.characterDatas[factionId].position += new Vector3(-gs.characterDatas[factionId].currentDirection.y,0,gs.characterDatas[factionId].currentDirection.x)*dt*Speed;
            BoardCollision(gs);
        }

        public virtual void Update()
        {
            transform.position = gameState.characterDatas[factionId].position;
            transform.eulerAngles = new Vector3(0,gameState.characterDatas[factionId].turn,0);
        }
    }
}