using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class HitscanBase : PoolObjectBase, IHitscan, ICauseDamage
    {
        private Timer holdRayTimer;
        private int raycastMask;
        private bool IsHoldRayIntervalRunning = false;
        protected float holdRayInterval = 0.1f;

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

            // All the integer presentations of layers we want the raycast to collide with.
            int[] layers = new int[6];
            layers[0] = 0;
            layers[1] = 8;
            layers[2] = 9;
            layers[3] = 10;
            layers[4] = 11;
            layers[5] = 14;

            /*
            int defaultLayer = 1 << layers[0];
            int layer6 = 1 << layers[1];
            int layer7 = 1 << layers[2];
            int layer8 = 1 << layers[3];
            int layer9 = 1 << layers[4];
            int layer12 = 1 << layers[5];

            raycastMask = defaultLayer | layer6 | layer7 | layer8 | layer9 | layer12;
            */
            // Create layermask from all the layers.
            
            foreach (int i in layers)
            {
                int layer = 1 << i;
                raycastMask |= layer;
            }
            

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
            if (IsHoldingType && (Input.GetButtonUp(GlobalVariables.SPELLCAST) || GameMan.Instance.CharController.IsInSpellMenu))
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
            transform.position = pos;
            transform.rotation = rot;
            DoRayCast();
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
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayMaxDistance, raycastMask))
            {
                if (IsHoldingType && !IsHoldRayIntervalRunning)
                {
                    IsHoldRayIntervalRunning = true;
                    if (hit.collider.gameObject.CompareTag(GlobalVariables.ENEMY_TAG))
                    {
                        hit.collider.gameObject.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    }

                    holdRayTimer.Run();
                }
                else if (!IsHoldingType)
                {
                    if (hit.collider.gameObject.CompareTag(GlobalVariables.ENEMY_TAG))
                    {
                        hit.collider.gameObject.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    }
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