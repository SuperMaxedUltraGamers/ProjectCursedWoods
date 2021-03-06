using System;
using UnityEngine;

namespace CursedWoods
{
    public class CharController : UnitBase
    {
        // TODO: Create and move these to some static and constant class or struct.
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
        public const string HORIZONTAL_RS = "HorizontalRS";
        public const string VERTICAL_RS = "VerticalRS";
        public const string DASH = "Dash";
        public const string ATTACK = "Attack";
        public const string SPELLCAST = "Spellcast";
        public const string INTERACT = "Interact";
        public const string OPEN_SPELLMENU = "OpenSpellMenu";
        public const string CHANGE_CONTROL_TYPE = "ChangeControlType";

        private GroundCheck groundCheck;

        public static event Action ControlTypeChanged;

        public bool IsGrounded { get; private set; }

        public static bool CanMoveToDash { get; set; } = true;

        public static bool IgnoreCameraControl { get; set; }

        public static bool IgnoreControl { get; set; }

        public static bool IsInSpellMenu { get; set; }

        protected override void Awake()
        {
            groundCheck = GetComponent<GroundCheck>();
        }

        private void Update()
        {
            IsGrounded = groundCheck.RayCastGround();

            if (Input.GetButtonDown(CHANGE_CONTROL_TYPE))
            {
                ControlTypeChanged?.Invoke();
            }
        }

        protected override void Die()
        {
        }
    }
}