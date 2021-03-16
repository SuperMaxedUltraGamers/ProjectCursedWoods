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

            damageDelayTimer = gameObject.AddComponent<Timer>();
            damageTimer = gameObject.AddComponent<Timer>();
            cooldownTimer = gameObject.AddComponent<Timer>();

            damageDelayTimer.Set(damageDelay);
            damageTimer.Set(damageTime);
            cooldownTimer.Set(cooldownTime);
        }

        private void OnEnable()
        {
            damageDelayTimer.TimerCompleted += Attack;
            damageTimer.TimerCompleted += CloseDamageWindow;
            cooldownTimer.TimerCompleted += CoolDownFinished;
        }

        private void OnDisable()
        {
            damageDelayTimer.TimerCompleted -= Attack;
            damageTimer.TimerCompleted -= CloseDamageWindow;
            cooldownTimer.TimerCompleted -= CoolDownFinished;
        }

        public void StartAttack()
        {
            if (!IsAttacking)
            {
                CharController.PlayerAnim.SetInteger(GlobalVariables.PLAYER_ANIM_TORSO_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_MELEE);
                IsAttacking = true;
                damageDelayTimer.Run();
            }
        }

        private void Attack()
        {
            // TODO: anims etc.
            meleeWeapon.ClearHitColliderList();
            meleeWeapon.ToggleHitBox(true);
            damageTimer.Run();
        }

        private void CloseDamageWindow()
        {
            meleeWeapon.ToggleHitBox(false);
            cooldownTimer.Run();
        }

        private void CoolDownFinished()
        {
            CharController.PlayerAnim.SetInteger(GlobalVariables.PLAYER_ANIM_TORSO_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
            IsAttacking = false;
        }
    }
}