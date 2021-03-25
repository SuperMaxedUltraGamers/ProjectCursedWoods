using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class HealthPickUp : InteractablePoolable
    {
        [SerializeField]
        private int healthIncreaseAmount = 300;

        private RotateAndBounce rotateAndBounce;

        protected override void Awake()
        {
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            base.Awake();
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            rotateAndBounce.SetOrigin(pos);
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseHealth(healthIncreaseAmount);
            Deactivate();
        }
    }
}