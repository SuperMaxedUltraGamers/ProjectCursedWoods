using UnityEngine;

namespace CursedWoods
{
    public class SpellIceRay : SpellHitscanBase<HitscanIceSingle>
    {
        private void Start()
        {
            Init(Spells.IceRay, SpellType.Ice, SpellMoveType.Free, 0f, 1.5f, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}