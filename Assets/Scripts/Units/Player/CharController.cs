using System;
using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class CharController : UnitBase
    {
        private GroundCheck groundCheck;
        [SerializeField]
        private GameObject spellBook;
        [SerializeField]
        private GameObject sword;

        [SerializeField]
        private LayerMask interactableMask;
        private Timer interActionTimer;

        [SerializeField]
        private AudioSource damageDeathAudioSource;

        private Collider interactCollider;

        private Vector2 lastMousePos;

        public static event Action ControlTypeChanged;

        public static Vector2 mousePos;
        public static bool hasMouseMoved;

        public bool IsGrounded { get; private set; }

        public bool CanMoveToDash { get; set; } = true;

        public bool IgnoreCameraControl { get; set; }

        public bool IgnoreControl { get; set; }

        public bool IsInSpellMenu { get; set; }

        public bool CanInteract { get; private set; }

        public float InteractRadius { get; private set; } = 1.5f;

        public LayerMask InteractableMask { get { return interactableMask; } }

        public Animator PlayerAnim { get; private set; }

        public AudioSource AudioSource { get; private set; }

        public GameObject SpellBook { get { return spellBook; } }

        public GameObject Sword { get { return sword; } }

        protected override void Awake()
        {
            base.Awake();
            groundCheck = GetComponent<GroundCheck>();
            PlayerAnim = GetComponentInChildren<Animator>();
            AudioSource = damageDeathAudioSource;

            interActionTimer = gameObject.AddComponent<Timer>();
            interActionTimer.Set(0.25f);
            interactCollider = null;

            dmgNumberColor = Color.red;
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
                    if (ControlTypeChanged != null)
                    {
                        ControlTypeChanged();
                    }
                }

                mousePos = Input.mousePosition;
                hasMouseMoved = lastMousePos != mousePos;
                lastMousePos = mousePos;
            }

#if (UNITY_EDITOR)

            if (Input.GetKeyUp(KeyCode.K))
            {
                GameMan.Instance.PlayerManager.UnlockSpellByType(Spells.Fireball);
                GameMan.Instance.PlayerManager.UnlockSpellByType(Spells.Shockwave);
                GameMan.Instance.PlayerManager.UnlockSpellByType(Spells.IceRay);
                GameMan.Instance.PlayerManager.UnlockSpellByType(Spells.MagicBeam);
                for (int i=0; i < Enum.GetNames(typeof(KeyType)).Length; i++)
                {
                    GameMan.Instance.PlayerManager.CollectedKey((KeyType)i);
                }
            }

#endif
        }

        protected override void Die()
        {
            PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_DEATH);
            IgnoreControl = true;
            GetComponent<CapsuleCollider>().enabled = false;
            //GetComponent<Rigidbody>().isKinematic = true;
        }

        private void CheckForInterActions()
        {
            if (Physics.CheckSphere(transform.position, InteractRadius, interactableMask))
            {
                interactCollider = Physics.OverlapSphere(transform.position, InteractRadius, InteractableMask)[0];
                CanInteract = true;
                GameMan.Instance.LevelUIManager.SetInteractPromtVisibility(visible: true, interactCollider.gameObject.GetComponent<Interactable>().InteractionText);
            }
            else
            {
                CanInteract = false;
                GameMan.Instance.LevelUIManager.SetInteractPromtVisibility(visible: false, "");
            }
            /*
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
            */

            interActionTimer.Run();
        }
    }
}