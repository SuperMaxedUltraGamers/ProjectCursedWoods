using UnityEngine;

namespace CursedWoods
{
    public class CharController : MonoBehaviour
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

        private GroundCheck groundCheck;

        public bool IsGrounded
        {
            get;
            private set;
        }

        public static bool IgnoreCameraControl
        {
            get;
            set;
        }

        public static bool IgnoreControl
        {
            get;
            set;
        }

        private void Awake()
        {
            groundCheck = GetComponent<GroundCheck>();
        }

        private void Update()
        {
            IsGrounded = groundCheck.RayCastGround();
        }
    }
}