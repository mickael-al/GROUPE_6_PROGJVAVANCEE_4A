using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using UnityEngine.InputSystem;
using WJ_MCTS;

namespace WJ_Controller
{
    public class Player : Character
    {
        private Vector2 moveDesired = Vector2.zero;
        public void Action1(InputAction.CallbackContext ctx)
        {
            if(handObject)
            {
                frisbie.Throw((Faction.Left == faction ? Vector3.right + new Vector3(0,0,InputManager.InputJoueur.Player.Move.ReadValue<Vector2>().y):Vector3.left+ new Vector3(0,0,-InputManager.InputJoueur.Player.MoveP2.ReadValue<Vector2>().y)),Strength);
                handObject = false;
            }
        }

        public void Action2(InputAction.CallbackContext ctx)
        {

        }

        public override void InitCharacter(WJ.CharacterInfo ci,GameState gs,Faction f,Vector2 terrainSize,Vector3 spawnPos)
        {
            base.InitCharacter(ci,gs,f,terrainSize,spawnPos);
            if(faction == Faction.Left)
            {
                InputManager.InputJoueur.Player.Fire.performed += Action1;
                InputManager.InputJoueur.Player.Lobs.performed += Action2;
            }
            else
            {
                InputManager.InputJoueur.Player.FireP2.performed += Action1;
                InputManager.InputJoueur.Player.LobsP2.performed += Action2;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(faction == Faction.Left)
            {
                InputManager.InputJoueur.Player.Fire.performed -= Action1;
                InputManager.InputJoueur.Player.Lobs.performed -= Action2;
            }
            else
            {
                InputManager.InputJoueur.Player.FireP2.performed -= Action1;
                InputManager.InputJoueur.Player.LobsP2.performed -= Action2;
            }
        }

        public override void Update()
        {
            if(!canMove || handObject)
            {
                return;
            }
            if(faction == Faction.Left)
            {
                moveDesired = InputManager.InputJoueur.Player.Move.ReadValue<Vector2>();
            }
            else
            {
                moveDesired = InputManager.InputJoueur.Player.MoveP2.ReadValue<Vector2>();
            }
            transform.Translate(new Vector3(-moveDesired.y,0,moveDesired.x)*Time.deltaTime*Speed);
            BoardCollision();
            TakeFrisbie();
        }
    }
}