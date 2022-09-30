using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;

namespace WJ_Controller
{
    public class RandomBot : CharacterIA
    {
        private float deltaMultiplayer = 5.0f;
        public override void UpdateBehaviour()
        {
            timeExecuteState = GameManager.Instance.DeltaBehaviour*deltaMultiplayer;
            base.gameState.characterDatas[factionId].currentDirection = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
            if(Random.Range(0,2) == 0)
            {
               Action1(base.gameState,(Faction.Left == faction ? Vector3.right:Vector3.left) + new Vector3(0,0,Random.Range(-1.0f,1.0f)),factionId); 
            }
        }
    }
}