﻿using UnityEngine;

namespace CursedWoods
{
    public class SpellBase : MonoBehaviour, ISpell
    {
        #region Private fields

        /// <summary>
        /// How much damage this spell causes before resistance calculations.
        /// </summary>
        [SerializeField, Tooltip("How much damage this spell causes before resistance calculations.")]
        private int damageAmount;

        #endregion Private fields

        #region Protected fields

        /// <summary>
        /// The offset from player where this spell is "spawned" when casted.
        /// </summary>
        protected Vector3 startOffset;

        /// <summary>
        /// The type of object this spell "spawns" from object pool when casting a spell.
        /// </summary>
        [SerializeField]
        protected ObjectPoolType objectPoolType;

        #endregion Protected fields

        #region Properties

        /// <summary>
        /// How long is this spell in cooldown after casting this spell.
        /// </summary>
        public float CoolDownTime { get; protected set; }

        /// <summary>
        /// How long it takes to cast this spell after player input.
        /// </summary>
        public float CastTime { get; protected set; }

        /// <summary>
        /// Is this spell in cooldown currently.
        /// </summary>
        public bool IsInCoolDown { get; protected set; }

        /// <summary>
        /// Is the player casting this spell currently.
        /// </summary>
        public bool IsCasting { get; protected set; }

        /// <summary>
        /// Determines how much damage this spell does.
        /// </summary>
        public int DamageAmount { get { return damageAmount; } set { damageAmount = value; } }

        /// <summary>
        /// Determines what type of damage this spell does.
        /// </summary>
        public DamageType DamageType { get; protected set; }

        /// <summary>
        /// Determines which spell this is.
        /// </summary>
        public Spells SpellType { get; protected set; }

        /// <summary>
        /// Determines how player can move while casting this spell.
        /// </summary>
        public PlayerMoveType SpellMoveType { get; protected set; }

        #endregion Properties

        #region Public API

        /// <summary>
        /// Called in SpellCaster to cast this spell.
        /// </summary>
        public virtual void CastSpell()
        {
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_SPELLCAST);
        }

        #endregion Public API

        #region Protected functionality

        /// <summary>
        /// Initialisation of this spell.
        /// </summary>
        /// <param name="spell">Which spell this is.</param>
        /// <param name="spellType">What type of spell this is.</param>
        /// <param name="spellMoveType">Determinates how player is allowed to move while casting</param>
        /// <param name="castTime">Takes the casting animation's start downtime.</param>
        /// <param name="coolDownTime">Time until spell can be casted again after casting.</param>
        /// <param name="spellStartOffset">Offset vector from player position where the spell is spawned.</param>
        protected virtual void Init(Spells spell, DamageType spellType, PlayerMoveType spellMoveType, float castTime, float coolDownTime, Vector3 spellStartOffset)
        {
            SpellType = spell;
            DamageType = spellType;
            SpellMoveType = spellMoveType;
            CastTime = castTime;
            CoolDownTime = coolDownTime;
            startOffset = spellStartOffset;
        }

        #endregion Protected functionality
    }
}