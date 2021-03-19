namespace CursedWoods
{
    public interface ISpell
    {
        float CoolDownTime
        {
            get;
        }
        float CastTime
        {
            get;
        }

        bool IsInCoolDown
        {
            get;
        }

        bool IsCasting
        {
            get;
        }

        Spells SpellType
        {
            get;
        }

        DamageType DamageType
        {
            get;
        }

        PlayerMoveType SpellMoveType
        {
            get;
        }

        void CastSpell();
    }
}