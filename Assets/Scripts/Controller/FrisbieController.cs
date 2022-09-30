using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ_MCTS;

namespace WJ
{
    public class FrisbieController : Entity
    {
        [SerializeField] private float rayon = 0.5f;
        [SerializeField] private float baseHeight = 4.0f;  
        private Vector2 terrainCalculeFrisbie;
        private GameState gameState = null;

        public float Rayon
        {
            get{return rayon;}
        }

        public void InitFrisbie(Vector2 terrainSize,GameState data)
        {
            terrainCalculeFrisbie = terrainSize/2.0f;
            gameState = data;
            Reset(data.FrisbiData);
        }

        public void Stop(FrisbiData data)
        {
            data.move = false;
            data.currentDirection = Vector3.zero;
            data.position.y = baseHeight; 
        }
        public void Update()
        {
            transform.position = gameState.FrisbiData.position;
        }

        public void TranslatePosition(GameState data,float dt)
        {
            if(data.FrisbiData.throwMode == ThrowMode.Throw)
            {
                data.FrisbiData.position += data.FrisbiData.currentDirection*dt;
            }
            BoardCollisionFrisbie(data.FrisbiData,data);
        }

        public void BoardCollisionFrisbie(FrisbiData data,GameState gs)
        {
            if(terrainCalculeFrisbie.y-rayon < data.position.z)
            {
                data.currentDirection.z = -data.currentDirection.z;                
                data.position.z = terrainCalculeFrisbie.y-rayon;
            }
            if(-terrainCalculeFrisbie.y+rayon > data.position.z)
            {
                data.currentDirection.z = -data.currentDirection.z; 
                data.position.z = -terrainCalculeFrisbie.y+rayon;
            }
            if(terrainCalculeFrisbie.x-rayon < data.position.x)
            {
                data.currentDirection.x = -data.currentDirection.x;
                data.position.x = (terrainCalculeFrisbie.x/2.0f)-rayon;
                GameManager.Instance.AddScorePoint(gs,1, ((terrainCalculeFrisbie.y*0.4f)-rayon < data.position.z || (-(terrainCalculeFrisbie.y*0.4f)+rayon > transform.position.z)) ? 3 : 5);
            }
            if(-(terrainCalculeFrisbie.x)+rayon > data.position.x)
            {
                data.currentDirection.x = -data.currentDirection.x;
                data.position.x = -(terrainCalculeFrisbie.x)+rayon;
                GameManager.Instance.AddScorePoint(gs,0, ((terrainCalculeFrisbie.y*0.4f)-rayon < data.position.z || (-(terrainCalculeFrisbie.y*0.4f)+rayon > transform.position.z)) ? 3 : 5);
            }
        }

        public void Reset(FrisbiData data)
        {
            Stop(data);
            data.position.x = 0;
            data.position.y = baseHeight;
            data.position.z = -(terrainCalculeFrisbie.y)+rayon;

        }

        public void Throw(FrisbiData data,Vector3 dir,float force = 1.0f)
        {
            data.move = true;
            data.currentDirection = dir * force;
            data.throwMode = ThrowMode.Throw;
        }

        public void Lobs(FrisbiData data,Vector3 dir,float force = 1.0f)
        {
            data.move = true;
            data.currentDirection = dir * force * 0.5f;
            data.throwMode =ThrowMode.Lobs;
        }
    }
}
