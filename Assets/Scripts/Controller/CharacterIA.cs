using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_Controller
{
    public abstract class CharacterIA : Character
    {
        protected Vector2 moveDesired = Vector2.zero;
        protected float timeExecuteState = 0.2f;
        public abstract void Action1();
        public abstract void Action2();

        public override void Update()
        {
            if(!canMove || !isInit)
            {
                return;
            }
            timeExecuteState-=Time.deltaTime;
            if(timeExecuteState < 0.0f)
            {
                timeExecuteState = 0.2f;
                UpdateBehaviour();
            }
            if(handObject)
            {
                return;
            }
            transform.Translate(new Vector3(-moveDesired.y,0,moveDesired.x)*Time.deltaTime*Speed);
            BoardCollision();
            TakeFrisbie();
        }

        public abstract void UpdateBehaviour();
    }
}
