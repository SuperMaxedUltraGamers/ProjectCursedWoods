using UnityEngine;

namespace CursedWoods
{
    public class HitscanBase : MonoBehaviour, IHitscan
    {
        protected LineRenderer lineRenderer = null;
        protected float fadeOffSpeed;
        protected float rayMaxDistance;

        public bool IsHoldingType
        {
            get;
            protected set;
        } = false;

        public bool IsFading
        {
            get;
            protected set;
        } = false;

        protected virtual void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

        private void Update()
        {
            if (IsHoldingType && Input.GetButtonUp(CharController.SPELLCAST))
            {
                IsFading = true;
            }

            if (IsFading)
            {
                Fade();
            }
        }

        public void OnHit()
        {
            print("NOTHING HAPPENED, HUH!");
        }

        public void ShootRay()
        {
            lineRenderer.enabled = true;
            DoRayCast();
            IsFading = true;
        }

        public void HoldRay(Vector3 pos, Quaternion rot)
        {
            lineRenderer.enabled = true;
            transform.position = pos;
            transform.rotation = rot;
            //TODO: reduce the raycasting amount for holding shit, doesnt need to happen every frame!
            DoRayCast();
        }

        public void Init(bool isHoldingType, float fadeOffSpeed, float rayMaxDistance)
        {
            IsHoldingType = isHoldingType;
            this.fadeOffSpeed = fadeOffSpeed;
            this.rayMaxDistance = rayMaxDistance;
        }

        private void DoRayCast()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayMaxDistance))
            {
                // TODO: Do shit
                OnHit();
                Debug.DrawLine(transform.position, hit.point, Color.red, 1.0f, false);
            }
            else Debug.DrawLine(transform.position, transform.position + (transform.forward * rayMaxDistance), Color.green, 1f, false);
        }

        private void Fade()
        {
            Color color = lineRenderer.material.color;
            color.a -= fadeOffSpeed * Time.deltaTime;
            if (color.a <= 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                lineRenderer.material.color = color;
            }
        }
    }

}