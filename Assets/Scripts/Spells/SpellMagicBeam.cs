using UnityEngine;

namespace CursedWoods
{
    public class SpellMagicBeam : SpellHitscanBase<HitscanMagicBeam>
    {
        private void Start()
        {
            Init(Spells.MagicBeam, DamageType.Magic, PlayerMoveType.HalfSpeed, 0f, 0f, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}