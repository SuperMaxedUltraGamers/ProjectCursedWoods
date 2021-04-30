using UnityEngine;

namespace CursedWoods
{
    public class MeleeAttackBase : MonoBehaviour, IMeleeAttack
    {
        [SerializeField]
        private MeleeWeaponBase meleeWeapon;
        private TrailRenderer weaponTrail;
        private AudioSource audioSource;

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
            audioSource = GetComponent<AudioSource>();
        }

        public void StartAttack()
        {
            if (!IsAttacking)
            {
                GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_MELEE);
                IsAttacking = true;
            }
        }

        private void OpenDamageWindowAnimEvent()
        {
            // TODO: anims etc.
            meleeWeapon.ClearHitColliderList();
            meleeWeapon.ToggleHitBox(true);
            weaponTrail.enabled = true;
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.SwordSwoosh);
        }

        private void CloseDamageWindowAnimEvent()
        {
            meleeWeapon.ToggleHitBox(false);
            weaponTrail.enabled = false;
        }

        private void EndAttackAnimEvent()
        {
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
            IsAttacking = false;
        }
    }
}