using UnityEngine;

namespace CursedWoods
{
    public class SpellShockwave : SpellProjectileBase<ProjectileShockwave>
    {
        private void Start()
        {
            Init(Spells.Shockwave, DamageType.Shock, PlayerMoveType.Hold, 0.5f, 5f, new Vector3(0f, -1f, 0f));
        }
    }
}