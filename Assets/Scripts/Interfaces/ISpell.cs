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

        DamageType SpellType
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