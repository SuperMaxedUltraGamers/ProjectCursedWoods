using UnityEngine;
using TMPro;

namespace CursedWoods
{
    public class DamageNumber : PoolObjectBase
    {
        private TextMeshProUGUI textMeshProUGUI;
        [SerializeField]
        private float moveSpeed = 0.5f;
        private float lifeTime = 4f;
        private float currentLifeTime;

        private void Awake()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            currentLifeTime -= Time.deltaTime;
            if (currentLifeTime < 0f)
            {
                Deactivate();
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            currentLifeTime = lifeTime;
        }

        public void SetDamageNumber(int damageAmount)
        {
            textMeshProUGUI.text = damageAmount.ToString();
        }
    }
}