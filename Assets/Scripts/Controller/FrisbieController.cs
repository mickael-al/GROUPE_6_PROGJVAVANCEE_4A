using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public class FrisbieController : Entity
    {
        [SerializeField] private float rayon = 0.5f;
        [SerializeField] private float baseHeight = 4.0f;  
        [SerializeField] private Vector2 TerrainSize = new Vector2(20.0f,10.0f);
        private Vector3 currentDirection;
        private ThrowMode throwMode = ThrowMode.Throw;

        public Vector3 CurrentDirection
        {
            get
            {
                return currentDirection;
            }
        } 

        public float Rayon
        {
            get
            {
                return rayon;
            }
        }

        public void Stop()
        {
            currentDirection = Vector3.zero;
            transform.position = new Vector3(transform.position.x,baseHeight,transform.position.z);
        }

        public void Update()
        {
            if(throwMode == ThrowMode.Throw)
            {
                transform.Translate(currentDirection*Time.deltaTime);
            }
            BoardCollision();
        }

        private void BoardCollision()
        {
            if((TerrainSize.y/2.0f)-rayon < transform.position.z)
            {
                currentDirection.z = -currentDirection.z;                
                transform.position = new Vector3(transform.position.x,transform.position.y,(TerrainSize.y/2.0f)-rayon);
            }
            if(-(TerrainSize.y/2.0f)+rayon > transform.position.z)
            {
                currentDirection.z = -currentDirection.z; 
                transform.position = new Vector3(transform.position.x,transform.position.y,-(TerrainSize.y/2.0f)+rayon);
            }
            if((TerrainSize.x/2.0f)-rayon < transform.position.x)
            {
                currentDirection.x = -currentDirection.x;
                transform.position = new Vector3((TerrainSize.x/2.0f)-rayon,transform.position.y,transform.position.z);
                GameManager.Instance.AddScorePoint(Faction.Left, (((TerrainSize.y/2.0f)*(1/2.5f))-rayon < transform.position.z || (-((TerrainSize.y/2.0f)*(1/2.5f))+rayon > transform.position.z)) ? 3 : 5);
            }
            if(-(TerrainSize.x/2.0f)+rayon > transform.position.x)
            {
                currentDirection.x = -currentDirection.x;
                transform.position = new Vector3(-(TerrainSize.x/2.0f)+rayon,transform.position.y,transform.position.z);
                GameManager.Instance.AddScorePoint(Faction.Right, (((TerrainSize.y/2.0f)*(1/2.5f))-rayon < transform.position.z || (-((TerrainSize.y/2.0f)*(1/2.5f))+rayon > transform.position.z)) ? 3 : 5);
            }
        }

        public void Reset()
        {
            Stop();
            transform.position = new Vector3(0,baseHeight,-(TerrainSize.y/2.0f)+rayon);
        }

        public void Throw(Vector3 dir,float force = 1.0f)
        {
            currentDirection = dir * force;
            throwMode = ThrowMode.Throw;
        }

        public void Lobs(Vector3 dir,float force = 1.0f)
        {
            currentDirection = dir * force * 0.5f;
            throwMode =ThrowMode.Lobs;
        }
    }
}
