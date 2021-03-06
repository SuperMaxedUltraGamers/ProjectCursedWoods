using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class MeleeWeaponBase : MonoBehaviour
    {
        protected Collider hitbox;
        protected List<Collider> hitColliders = new List<Collider>();

        [SerializeField]
        protected int damageAmount = 0;

        [SerializeField]
        private DamageType damageType = DamageType.Melee;

        public int DamageAmount
        {
            get { return damageAmount; }
            set { damageAmount = value; }
        }

        public void Initialize()
        {
            hitbox = GetComponent<Collider>();
            hitbox.enabled = false;
        }

        public void ToggleHitBox(bool isOn)
        {
            hitbox.enabled = isOn;
            print(hitbox.enabled);
        }

        public void ClearHitColliderList()
        {
            hitColliders.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hitColliders.Contains(other))
            {
                hitColliders.Add(other);
                print("Enemy hit!");
                other.GetComponent<IHealth>().DecreaseHealth(damageAmount, damageType);
                // TODO: do stuff to the thing we collided
            }
        }
    }
}