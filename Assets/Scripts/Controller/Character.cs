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
        private float frisbieRayon = 1.0f;
        protected Vector2 terrainCalcule = Vector2.zero;
        protected Vector3 workPosition;
        protected int numberAction = 12;

        public int NumberAction
        {
            get{return numberAction;}
        }

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
            frisbieRayon = GameManager.Instance.Frisbie.Rayon;
        }

        public void ResetSpawnPosition(GameState gs)
        {
            gs.characterDatas[factionId].position = spawnPosition;
            gs.characterDatas[factionId].turn = Faction.Left == faction ? 90 : -90;
            gs.characterDatas[factionId].handObject = false;
        }

        public void TakeFrisbie(GameState gs)
        { 
            if(gs.FrisbiData.move && !gs.characterDatas[factionId].handObject && Vector3.Distance(gs.characterDatas[factionId].position,gs.FrisbiData.position) < rayon+frisbieRayon)
            {
                gs.characterDatas[factionId].handObject = true;
                gs.FrisbiData.position = gs.characterDatas[factionId].position + (factionId== 0 ? Vector3.right*(rayon+frisbieRayon):Vector3.left*(rayon+frisbieRayon));
                GameManager.Instance.Frisbie.Stop(gs.FrisbiData);
            }
        }

        public void Action1(GameState state,Vector3 dir,int fid)
        {
            if(state.characterDatas[fid].handObject)
            {
                GameManager.Instance.Frisbie.Throw(state.FrisbiData,dir,Strength);
                state.characterDatas[fid].handObject = false;
            }
        }

        public void BoardCollision(GameState gs)
        {
            workPosition = gs.characterDatas[factionId].position;
            if(factionId == 0)
            {
                if(workPosition.x < -terrainCalcule.x)
                {
                    workPosition.x = -terrainCalcule.x;
                }
                if(workPosition.x > 0)
                {
                    workPosition.x = 0.0f;                    
                }
            }
            else
            {
                if(workPosition.x > terrainCalcule.x)
                {
                    workPosition.x = terrainCalcule.x;
                }
                if(workPosition.x < 0)
                {
                    workPosition.x = 0.0f;
                }
            }
            if(workPosition.z > terrainCalcule.y)
            {
                workPosition.z = terrainCalcule.y;
            }
            if(workPosition.z < -terrainCalcule.y)
            {
                workPosition.z = -terrainCalcule.y;
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
            gs.characterDatas[factionId].position.x += -gs.characterDatas[factionId].currentDirection.y * dt * Speed;
            gs.characterDatas[factionId].position.z += gs.characterDatas[factionId].currentDirection.x * dt * Speed;
            BoardCollision(gs);
        }

        public void Actions(int id,GameState gs,int fid)
        {
            int count = 0;
            if(id < 9)
            {
                for(int i = -1 ; i <= 1;i++)
                {
                    for(int j = -1 ; j <= 1;j++)
                    {
                        if(count == id)
                        {
                            gs.characterDatas[fid].currentDirection = new Vector3(i,j);                            
                            return;
                        }
                        count++;
                    }   
                }
            }
            count = 9;
            for(int i = -1 ; i <= 1;i++)
            {
                if(id==count)
                {
                    Action1(gs,fid == 0 ? Vector3.right + new Vector3(0,0,i): Vector3.left+ new Vector3(0,0,-i),fid);
                    return;
                }
                count++;
            }
        } 

        public virtual void Update()
        {
            transform.position = gameState.characterDatas[factionId].position;
            transform.eulerAngles = new Vector3(0,gameState.characterDatas[factionId].turn,0);
        }
    }
}