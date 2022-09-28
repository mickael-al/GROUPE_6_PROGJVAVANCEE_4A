using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_Controller
{
    public abstract class CharacterIA : Character
    {
        protected float timeExecuteState = 0.0f;

        public override void Update()
        {
            base.Update();
            timeExecuteState -= Time.deltaTime;
            if(timeExecuteState < 0.0f)
            {
                timeExecuteState = GameManager.Instance.DeltaBehaviour;
                UpdateBehaviour();
            }
        }

        public abstract void UpdateBehaviour();
    }
}
