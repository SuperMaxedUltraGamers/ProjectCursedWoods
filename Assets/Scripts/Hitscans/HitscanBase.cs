using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class HitscanBase : PoolObjectBase, IHitscan, ICauseDamage
    {
        private Timer holdRayTimer;
        protected float holdRayInterval = 0.02f;
        private bool IsHoldRayIntervalRunning = false;

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

        public DamageType DamageType { get; protected set; }

        public int DamageAmount { get; protected set; }

        protected virtual void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            holdRayTimer = gameObject.AddComponent<Timer>();
            holdRayTimer.Set(holdRayInterval);

            //lineRenderer.enabled = false;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            //print("enabled");
            holdRayTimer.TimerCompleted += HoldRayInterval;
        }

        private void OnDisable()
        {
            //print("disabled");
            holdRayTimer.TimerCompleted -= HoldRayInterval;
        }

        private void Update()
        {
            if (IsHoldingType && (Input.GetButtonUp(GlobalVariables.SPELLCAST) || CharController.IsInSpellMenu))
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

            // TODO: only reduce the cast amount to damaging etc. so line renderer gets updated every frame so it doesnt look jittery
            //print("called");
            if (!IsHoldRayIntervalRunning)
            {
                IsHoldRayIntervalRunning = true;
                //print("Holdray casted!");
                transform.position = pos;
                transform.rotation = rot;
                DoRayCast();
                holdRayTimer.Run();
            }
        }

        public void InitDamageInfo(int damageAmount, DamageType damageType)
        {
            DamageAmount = damageAmount;
            DamageType = damageType;
        }

        // TODO: Maybe refactor and get rid of this init or InitDamageInfo, kinda confusing when initialising values from multiple places.
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
                if (hit.collider.gameObject.CompareTag(GlobalVariables.ENEMY_TAG))
                {
                    hit.collider.gameObject.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                }
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
                // TODO: fade out
            }
        }

        private void HoldRayInterval()
        {
            IsHoldRayIntervalRunning = false;
        }
    }
}