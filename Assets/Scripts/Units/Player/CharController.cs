using System;
using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class CharController : UnitBase
    {
        private GroundCheck groundCheck;

        [SerializeField]
        private LayerMask interactableMask;
        private Timer interActionTimer;

        public event Action ControlTypeChanged;

        public bool IsGrounded { get; private set; }

        public bool CanMoveToDash { get; set; } = true;

        public bool IgnoreCameraControl { get; set; }

        public bool IgnoreControl { get; set; }

        public bool IsInSpellMenu { get; set; }

        public bool CanInteract { get; private set; }

        public float InteractRadius { get; private set; } = 1f;

        public LayerMask InteractableMask { get { return interactableMask; } }

        public Animator PlayerAnim { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            groundCheck = GetComponent<GroundCheck>();
            PlayerAnim = GetComponentInChildren<Animator>();
            interActionTimer = gameObject.AddComponent<Timer>();
            interActionTimer.Set(0.25f);
        }

        private void OnEnable()
        {
            interActionTimer.TimerCompleted += CheckForInterActions;
        }

        private void OnDisable()
        {
            interActionTimer.TimerCompleted -= CheckForInterActions;
        }

        private void Start()
        {
            interActionTimer.Run();
        }

        private void Update()
        {
            IsGrounded = groundCheck.RayCastGround();

            if (!IgnoreControl)
            {
                if (Input.GetButtonDown(GlobalVariables.CHANGE_CONTROL_TYPE))
                {
                    ControlTypeChanged?.Invoke();
                }

                if (CanInteract && Input.GetButtonDown(GlobalVariables.INTERACT))
                {

                }
            }
        }

        protected override void Die()
        {
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_DEATH);
            IgnoreControl = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void CheckForInterActions()
        {
            if (Physics.CheckSphere(transform.position, InteractRadius, interactableMask))
            {
                CanInteract = true;
                GameMan.Instance.LevelUIManager.SetInteractPromtVisibility(visible: true);
            }
            else
            {
                CanInteract = false;
                GameMan.Instance.LevelUIManager.SetInteractPromtVisibility(visible: false);
            }

            interActionTimer.Run();
        }
    }
}