using UnityEngine;

namespace CursedWoods
{
    public class HitscanBase : PoolObjectBase, IHitscan
    {
        protected LineRenderer lineRenderer = null;
        protected float fadeOffSpeed;
        protected float rayMaxDistance;
        protected float currentFadeOffAmount = 1f;

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
            //lineRenderer.enabled = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (IsHoldingType && Input.GetButtonUp(CharController.SPELLCAST))
            {
                IsFading = true;
            }

            if (IsFading)
            {
                //print("fading");
                Fade();
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            currentFadeOffAmount = 1f;
            IsFading = false;
        }

        public void OnHit()
        {
            //print("NOTHING HAPPENED, HUH!");
        }

        public void ShootRay(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
            //lineRenderer.enabled = true;
            DoRayCast();
            IsFading = true;
        }

        public void HoldRay(Vector3 pos, Quaternion rot)
        {
            //lineRenderer.enabled = true;
            transform.position = pos;
            transform.rotation = rot;
            // TODO: reduce the raycasting amount for holding, doesnt need to happen every frame!
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
                // TODO: Do stuff
                OnHit();
                //Debug.DrawLine(transform.position, hit.point, Color.red, 1.0f, false);
                lineRenderer.SetPosition(1, new Vector3(0f, 0f, hit.distance));
            }
            else
            {
                //Debug.DrawLine(transform.position, transform.position + (transform.forward * rayMaxDistance), Color.green, 1f, false);
                lineRenderer.SetPosition(1, new Vector3(0f, 0f, rayMaxDistance));
            }
        }

        private void Fade()
        {
            currentFadeOffAmount -= fadeOffSpeed * Time.deltaTime;
            if (currentFadeOffAmount <= 0f)
            {
                Deactivate();
            }
            else
            {
                // TODO: fade alpha channel
            }
        }
    }
}