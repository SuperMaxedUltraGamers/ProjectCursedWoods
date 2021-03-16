using System;
using UnityEngine;

namespace CursedWoods
{
    public class CharController : UnitBase
    {
        private GroundCheck groundCheck;
        private static Animator playerAnim;

        public static event Action ControlTypeChanged;

        public bool IsGrounded { get; private set; }

        public static bool CanMoveToDash { get; set; } = true;

        public static bool IgnoreCameraControl { get; set; }

        public static bool IgnoreControl { get; set; }

        public static bool IsInSpellMenu { get; set; }

        public static Animator PlayerAnim { get { return playerAnim; } }

        protected override void Awake()
        {
            base.Awake();
            groundCheck = GetComponent<GroundCheck>();
            playerAnim = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            IsGrounded = groundCheck.RayCastGround();

            if (Input.GetButtonDown(GlobalVariables.CHANGE_CONTROL_TYPE))
            {
                ControlTypeChanged?.Invoke();
            }
        }

        protected override void Die()
        {
        }
    }
}