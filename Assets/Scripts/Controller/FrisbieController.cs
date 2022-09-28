using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    public class FrisbieController : Entity
    {
        [SerializeField] private float rayon = 0.5f;
        [SerializeField] private float baseHeight = 4.0f;  
        private Vector3 currentDirection;
        private ThrowMode throwMode = ThrowMode.Throw;
        private Vector2 terrainCalcule;

        private bool move = false;

        public Vector3 CurrentDirection
        {
            get
            {
                return currentDirection;
            }
        } 

        public bool Moves
        {
            get
            {
                return move;
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
            move = false;
            currentDirection = Vector3.zero;
            transform.position = new Vector3(transform.position.x,baseHeight,transform.position.z);
        }

        public void Start()
        {
            terrainCalcule = GameManager.Instance.TerrainSize/2.0f;
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
            if((terrainCalcule.y)-rayon < transform.position.z)
            {
                currentDirection.z = -currentDirection.z;                
                transform.position = new Vector3(transform.position.x,transform.position.y,(terrainCalcule.y)-rayon);
            }
            if(-(terrainCalcule.y)+rayon > transform.position.z)
            {
                currentDirection.z = -currentDirection.z; 
                transform.position = new Vector3(transform.position.x,transform.position.y,-(terrainCalcule.y)+rayon);
            }
            if((terrainCalcule.x)-rayon < transform.position.x)
            {
                currentDirection.x = -currentDirection.x;
                transform.position = new Vector3((terrainCalcule.x/2.0f)-rayon,transform.position.y,transform.position.z);
                GameManager.Instance.AddScorePoint(Faction.Left, (((terrainCalcule.y)*(1/2.5f))-rayon < transform.position.z || (-((terrainCalcule.y)*(1/2.5f))+rayon > transform.position.z)) ? 3 : 5);
            }
            if(-(terrainCalcule.x)+rayon > transform.position.x)
            {
                currentDirection.x = -currentDirection.x;
                transform.position = new Vector3(-(terrainCalcule.x)+rayon,transform.position.y,transform.position.z);
                GameManager.Instance.AddScorePoint(Faction.Right, (((terrainCalcule.y)*(1/2.5f))-rayon < transform.position.z || (-((terrainCalcule.y)*(1/2.5f))+rayon > transform.position.z)) ? 3 : 5);
            }
        }

        public void Reset()
        {
            Stop();
            transform.position = new Vector3(0,baseHeight,-(terrainCalcule.y)+rayon);
        }

        public void Throw(Vector3 dir,float force = 1.0f)
        {
            move = true;
            currentDirection = dir * force;
            throwMode = ThrowMode.Throw;
        }

        public void Lobs(Vector3 dir,float force = 1.0f)
        {
            move = true;
            currentDirection = dir * force * 0.5f;
            throwMode =ThrowMode.Lobs;
        }
    }
}
