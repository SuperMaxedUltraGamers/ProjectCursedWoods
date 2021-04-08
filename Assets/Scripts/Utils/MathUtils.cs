using UnityEngine;

namespace CursedWoods.Utils
{
    public static class MathUtils
    {
        public static Quaternion GetLookRotationYAxis(Vector3 pos1, Vector3 pos2, Vector3 up)
        {
            Quaternion wantedRot = Quaternion.LookRotation(pos1 - pos2, up);
            wantedRot.x = 0f;
            wantedRot.z = 0f;
            return wantedRot;
        }

        public static float GetDistanceToPlayer(Vector3 myPos)
        {
            Vector3 toPlayer = GameMan.Instance.PlayerT.position - myPos;
            return Vector3.SqrMagnitude(toPlayer);
        }

        public static float GetDistanceToPos(Vector3 pos, Vector3 myPos)
        {
            Vector3 toPos = pos - myPos;
            return Vector3.SqrMagnitude(toPos);
        }
    }
}