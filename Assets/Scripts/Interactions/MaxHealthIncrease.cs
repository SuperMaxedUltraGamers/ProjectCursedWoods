using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class MaxHealthIncrease : InteractablePoolable
    {
        [SerializeField, Range(0.01f, 1), Tooltip("The percent of the current max health, which then will be added to the max health.")]
        private float maxHealthIncreasePercent = 0.1f;
        private int spawnerID = 1;

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

        public void SetSpawnerID(int id)
        {
            spawnerID = id;
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseMaxHealth((int) (GameMan.Instance.CharController.MaxHealth * maxHealthIncreasePercent));
            GameMan.Instance.GraveyardManager.DisableSpawMaxHealthIncrease(spawnerID);
            Deactivate();
        }
    }
}