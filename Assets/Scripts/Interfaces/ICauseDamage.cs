namespace CursedWoods
{
    public interface ICauseDamage
    {
        DamageType DamageType { get; }
        int DamageAmount { get; }

        void InitDamageInfo(int damageAmount, DamageType damageType);
    }
}