using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_Controller
{
    public class RandomBot : CharacterIA
    {
        public override void UpdateBehaviour()
        {
            base.gameState.characterDatas[factionId].currentDirection = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
            if(Random.Range(0,2) == 0)
            {
               Action1(base.gameState,(Faction.Left == faction ? Vector3.right:Vector3.left) + new Vector3(0,0,Random.Range(-1.0f,1.0f)),factionId); 
            }
        }
    }
}