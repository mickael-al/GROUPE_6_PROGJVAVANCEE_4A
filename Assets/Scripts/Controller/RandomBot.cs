using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_Controller
{
    public class RandomBot : CharacterIA
    {
        public override void Action1()
        {

        }

        public override void Action2()
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateBehaviour()
        {
            moveDesired.x = Random.Range(-1.0f,1.0f);
            moveDesired.y = Random.Range(-1.0f,1.0f);
            if(handObject && Random.Range(0,2) == 0)
            {
                frisbie.Throw((Faction.Left == faction ? Vector3.right:Vector3.left) + new Vector3(0,0,Random.Range(-1.0f,1.0f)),Strength);
                handObject = false;
            }
        }
    }
}