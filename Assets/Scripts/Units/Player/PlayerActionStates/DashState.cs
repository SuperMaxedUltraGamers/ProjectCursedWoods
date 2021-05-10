using System;
using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class DashState : PlayerActionStateBase
    {
        // Rigidbody movement dashVel
        //private float dashVel = 750f;
        private float dashVel = 15f;
        private float dashHoldTime = 0.2f;
        private PlayerInputType nextState = PlayerInputType.None;
        private float dashCoolDownTime = 1f;
        private Vector3 dashDir;

        private PlayerParticleManager particleManager;
        private AudioSource audioSource;

        // Used to notify UI
        public static event Action<float> Dashed;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Dash;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);

            if (mover == null)
            {
                mover = GetComponent<NewPlayerMover>();
            }

            particleManager = GetComponent<PlayerParticleManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            // Null check inside the method so no worries.
            mover.Initialize(actionStateManager);
        }

        public override void DaUpdate()
        {
            Vector3 moveAmount = dashDir * dashVel * Time.deltaTime;
            mover.DashMovement(moveAmount);
        }

        /*
        public override void DaFixedUpdate()
        {
            actionStateManager.PlayerRb.velocity = dashDir * dashVel * Time.fixedDeltaTime;
        }
        */

        public override void HandleInput()
        {
            inputDir = mover.InputDir();
            if (Input.GetButtonDown(GlobalVariables.ATTACK))
            {
                nextState = PlayerInputType.Attack;
            }
            else if (Input.GetButtonDown(GlobalVariables.SPELLCAST))
            {
                nextState = PlayerInputType.Spellcast;
            }
            else if (inputDir.magnitude != 0f)
            {
                nextState = PlayerInputType.Move;
            }
            else if (inputDir.magnitude == 0f)
            {
                nextState = PlayerInputType.None;
            }
        }

        public override void TransitionIn()
        {
            nextState = PlayerInputType.None;
            GameMan.Instance.CharController.CanMoveToDash = false;
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_DASH);
            //GetComponent<IHealth>().IsImmortal = true;
            dashDir = mover.GetCorrectMoverDir().normalized;
            if (dashDir == Vector3.zero)
            {
                dashDir = transform.forward;
            }

            particleManager.DashParticles.Play();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.Dash, 5f); // 1.5f if using Dash2, 5f if using DashEdited

            if (Dashed != null)
            {
                Dashed(dashCoolDownTime + dashHoldTime);
            }

            StartCoroutine(DashHoldTimer());
        }

        private IEnumerator DashHoldTimer()
        {
            yield return new WaitForSeconds(dashHoldTime);
            //GetComponent<IHealth>().IsImmortal = false;
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
            // Bad practice to call coroutine inside other coroutine
            StartCoroutine(DashCoolDownTimer());
            actionStateManager.ChangeState(nextState);
        }

        private IEnumerator DashCoolDownTimer()
        {
            yield return new WaitForSeconds(dashCoolDownTime);
            GameMan.Instance.CharController.CanMoveToDash = true;
        }
    }
}