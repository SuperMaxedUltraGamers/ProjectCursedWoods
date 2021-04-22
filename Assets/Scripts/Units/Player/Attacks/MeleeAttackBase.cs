using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class MeleeAttackBase : MonoBehaviour, IMeleeAttack
    {
        private float damageTime = 0.2f;
        private float damageDelay = 0.6f;
        private float cooldownTime = 0.2f;

        private Timer damageTimer;
        private Timer damageDelayTimer;
        private Timer cooldownTimer;

        [SerializeField]
        private MeleeWeaponBase meleeWeapon;
        private TrailRenderer weaponTrail;

        public bool IsAttacking
        {
            get;
            protected set;
        }

        [SerializeField]
        private PlayerMoveType moveType = PlayerMoveType.Free;

        public PlayerMoveType MoveType
        {
            get
            {
                return moveType;
            }
        }

        private void Awake()
        {
            meleeWeapon.Initialize();
            weaponTrail = meleeWeapon.GetComponentInChildren<TrailRenderer>();

            damageDelayTimer = gameObject.AddComponent<Timer>();
            damageTimer = gameObject.AddComponent<Timer>();
            cooldownTimer = gameObject.AddComponent<Timer>();

            damageDelayTimer.Set(damageDelay);
            damageTimer.Set(damageTime);
            cooldownTimer.Set(cooldownTime);
        }

        private void OnEnable()
        {
            //damageDelayTimer.TimerCompleted += Attack;
            //damageTimer.TimerCompleted += CloseDamageWindow;
            //cooldownTimer.TimerCompleted += CoolDownFinished;
        }

        private void OnDisable()
        {
            //damageDelayTimer.TimerCompleted -= Attack;
            //damageTimer.TimerCompleted -= CloseDamageWindow;
            //cooldownTimer.TimerCompleted -= CoolDownFinished;
        }

        public void StartAttack()
        {
            if (!IsAttacking)
            {
                GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_MELEE);
                IsAttacking = true;
                //damageDelayTimer.Run();
            }
        }

        private void OpenDamageWindowAnimEvent()
        {
            // TODO: anims etc.
            meleeWeapon.ClearHitColliderList();
            meleeWeapon.ToggleHitBox(true);
            weaponTrail.enabled = true;
            //damageTimer.Run();
        }

        private void CloseDamageWindowAnimEvent()
        {
            meleeWeapon.ToggleHitBox(false);
            weaponTrail.enabled = false;
            //cooldownTimer.Run();
        }

        private void EndAttackAnimEvent()
        {
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
            IsAttacking = false;
        }
    }
}