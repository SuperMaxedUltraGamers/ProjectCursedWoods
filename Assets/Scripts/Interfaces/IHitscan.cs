﻿using UnityEngine;

namespace CursedWoods
{
    public interface IHitscan
    {
        bool IsHoldingType { get; }

        bool IsFading { get; }

        void ShootRay(Vector3 pos, Quaternion rot);

        void HoldRay(Vector3 pos, Quaternion rot);

        void OnHit();

        void AfterRay(Vector3 startPos, Vector3 endPos, bool wasHit);
    }
}