using UnityEngine;

namespace CursedWoods
{
    public class NoneState : PlayerActionStateBase
    {
        //private Vector3 velocity;
        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.None;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Dash);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);
            AddTargetState(PlayerInputType.Interact);

            if (mover == null)
            {
                mover = GetComponent<NewPlayerMover>();
            }
            
        }

        private void Start()
        {
            // Null check inside the method so no worries.
            mover.Initialize(actionStateManager);
        }

        public override void HandleInput()
        {
            inputDir = mover.InputDir();
            
            //inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL), Input.GetAxisRaw(CharController.VERTICAL));

            if (Input.GetButtonDown(GlobalVariables.DASH) && GameMan.Instance.CharController.CanMoveToDash)
            {
                //actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(GlobalVariables.ATTACK))
            {
                //actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(GlobalVariables.SPELLCAST))
            {
                //actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Spellcast);
            }
            else if (Input.GetButtonDown(GlobalVariables.INTERACT) && GameMan.Instance.CharController.CanInteract)
            {
                //actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Interact);
            }
            else if (inputDir.magnitude != 0f)
            {
                //actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Move);
            }
            
        }

        public override void DaUpdate()
        {
            mover.Movement();
            mover.Move(Time.deltaTime);
        }

        /*
        public override void DaFixedUpdate()
        {
            mover.Move(Time.fixedDeltaTime);
        }
        */
    }
}