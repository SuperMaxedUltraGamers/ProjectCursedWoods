using UnityEngine;

namespace CursedWoods
{
    public class SpellFireBall : SpellProjectileBase<ProjectileFireBall>
    {
        private void Start()
        {
            Init(Spells.Fireball, SpellType.Fire, SpellMoveType.Free, 0f, 0.25f, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}