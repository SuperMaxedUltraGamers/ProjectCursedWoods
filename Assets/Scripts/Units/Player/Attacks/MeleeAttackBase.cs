using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class MeleeAttackBase : MonoBehaviour, IMeleeAttack
    {
        [SerializeField, Tooltip("How long is the window when we can cause damage.")]
        private float damageTime = 0.2f;

        [SerializeField, Tooltip("How long before we start causing damage, aka how long until the animation has played far enough.")]
        private float damageDelay = 0.2f;

        [SerializeField]
        private float cooldownTime = 1f;

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
            IsAttacking = false;
        }
    }
}