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

        SpellType SpellType
        {
            get;
        }

        SpellMoveType SpellMoveType
        {
            get;
        }

        void CastSpell();
    }
}