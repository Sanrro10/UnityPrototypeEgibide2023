using System.Collections;
using System.Collections.Generic;
using General.Scripts;
using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates 
{
    public class SceneChangeState : IState
    {

        private PlayerController player;
        private GameController.SPlayerSpawnData playerSpawnData;
        private bool facingRight;

        public SceneChangeState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.GetComponent<CapsuleCollider2D>().enabled = false;
            player.GetComponent<Rigidbody2D>().simulated= false;
            player.DisablePlayerControls();
            playerSpawnData = player.GetSPlayerSpawnData();
            player.animator.SetBool("IsWalk", true);
            //check if going right or left
            if (playerSpawnData.Position.x > playerSpawnData.GoToPosition.x)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }

        public void Update()
        {
            if ((facingRight && player.transform.position.x >= playerSpawnData.GoToPosition.x)
                || (!facingRight && player.transform.position.x <= playerSpawnData.GoToPosition.x))
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
            
            player.ForceMove(facingRight);
        }

        public void Exit()
        {
            player.GetComponent<CapsuleCollider2D>().enabled = true;
            player.GetComponent<Rigidbody2D>().simulated= true;
            player.EnablePlayerControls();
            player.animator.SetBool("IsWalk", false);
        }
    }
}
