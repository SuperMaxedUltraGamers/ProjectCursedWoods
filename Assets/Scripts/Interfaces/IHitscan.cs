using UnityEngine;

namespace CursedWoods
{
    public interface IHitscan
    {
        bool IsHoldingType { get; }
        bool IsFading { get; }
        void ShootRay();
        void HoldRay(Vector3 pos, Quaternion rot);
        void OnHit();
    }
}