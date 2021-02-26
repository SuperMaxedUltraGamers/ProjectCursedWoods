using UnityEngine;

namespace CursedWoods
{
    public class SpellMagicBeam : SpellHitscanBase<HitscanMagicBeam>
    {
        private void Start()
        {
            Init(Spells.IceRay, SpellType.Ice, SpellMoveType.Free, 0f, 0f, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}