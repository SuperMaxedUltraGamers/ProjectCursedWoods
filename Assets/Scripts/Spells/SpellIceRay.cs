using UnityEngine;

namespace CursedWoods
{
    public class SpellIceRay : SpellHitscanBase<HitscanIceSingle>
    {
        private void Start()
        {
            Init(Spells.IceRay, DamageType.Ice, PlayerMoveType.Free, 0.25f, 2.5f, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}