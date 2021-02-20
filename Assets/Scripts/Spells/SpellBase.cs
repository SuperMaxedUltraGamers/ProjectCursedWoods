using UnityEngine;

namespace CursedWoods
{
    public class SpellBase : MonoBehaviour, ISpell
    {
        protected Vector3 startOffset;

        public float CoolDownTime { get; protected set; }
        public float CastTime { get; protected set; }

        public bool IsInCoolDown
        {
            get;
            protected set;
        }

        public bool IsCasting
        {
            get;
            protected set;
        }

        public SpellType SpellType
        {
            get;
            protected set;
        }

        public Spells Spell
        {
            get;
            protected set;
        }

        public SpellMoveType SpellMoveType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Setups some variables yo.
        /// </summary>
        /// <param name="spell">Which spell this is.</param>
        /// <param name="spellType">What type of spell this is.</param>
        /// <param name="spellMoveType">Determinates how player is allowed to move while casting</param>
        /// <param name="castTime">Takes the casting animation's start downtime.</param>
        /// <param name="coolDownTime">Time until spell can be casted again after casting.</param>
        /// <param name="spellStartOffset">Offset vector from player position where the spell is spawned.</param>
        protected virtual void Init(Spells spell, SpellType spellType, SpellMoveType spellMoveType, float castTime, float coolDownTime, Vector3 spellStartOffset)
        {
            Spell = spell;
            SpellType = spellType;
            SpellMoveType = spellMoveType;
            CastTime = castTime;
            CoolDownTime = coolDownTime;
            startOffset = spellStartOffset;
        }

        public virtual void CastSpell() { }
    }
}