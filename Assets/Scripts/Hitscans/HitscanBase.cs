using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class HitscanBase : PoolObjectBase, IHitscan, ICauseDamage
    {
        private Timer holdRayTimer;
        private int raycastMask;
        private bool IsHoldRayIntervalRunning = false;
        protected float holdRayInterval = 0.2f;

        protected LineRenderer lineRenderer = null;
        protected float fadeOffSpeed;
        protected float rayMaxDistance;
        protected float currentFadeOffAmount = 1f;

        //[SerializeField]
        //protected AudioSource audioSource;

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
            //layers[5] = 14; interactable
            layers[5] = 20; //barrier

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
                Fade();
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            currentFadeOffAmount = 1f;
            lineRenderer.material.SetFloat("_Transparency", 1f);
            IsFading = false;
        }

        public virtual void OnHit()
        {
            // TODO: spawn particles at hit point.
        }

        public virtual void AfterRay(Vector3 startPos, Vector3 endPos, bool wasHit)
        {
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
            Vector3 transPos = transform.position;
            if (Physics.Raycast(transPos, transform.forward, out hit, rayMaxDistance, raycastMask))
            {
                if (IsHoldingType && !IsHoldRayIntervalRunning)
                {
                    IsHoldRayIntervalRunning = true;
                    GameObject hitGO = hit.collider.gameObject;
                    if (hitGO.layer == GlobalVariables.ENEMY_LAYER)
                    {
                        IHealth otherHealth = hitGO.GetComponent<IHealth>();
                        if (otherHealth == null)
                        {
                            otherHealth = hitGO.GetComponentInParent<IHealth>();
                        }

                        otherHealth.DecreaseHealth(DamageAmount, DamageType);
                    }
                    /*
                    if (hit.collider.gameObject.CompareTag(GlobalVariables.ENEMY_TAG))
                    {
                        hit.collider.gameObject.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    }
                    */

                    // TODO: MAKE GLOBAL ENUM FOR AUDIOEFFECTS AND MAKE VARIABLE OUT OF THAT AND DO SWITCH CHECK HERE WHICH AUDIO TO PLAY!
                    //if (!audioSource.isPlaying)
                    //{
                    //    Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.MagicBeam);
                    //}

                    holdRayTimer.Run();
                }
                else if (!IsHoldingType)
                {
                    /*
                    if (hit.collider.gameObject.CompareTag(GlobalVariables.ENEMY_TAG))
                    {
                        hit.collider.gameObject.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    }
                    */
                    GameObject hitGO = hit.collider.gameObject;
                    if (hitGO.layer == GlobalVariables.ENEMY_LAYER)
                    {
                        IHealth otherHealth = hitGO.GetComponent<IHealth>();
                        if (otherHealth == null)
                        {
                            otherHealth = hitGO.GetComponentInParent<IHealth>();
                        }

                        otherHealth.DecreaseHealth(DamageAmount, DamageType);
                    }
                    else if (hitGO.layer == GlobalVariables.ENEMY_PROJECTILE_LAYER)
                    {
                        hitGO.GetComponent<ProjectileBase>().OnHit();
                    }

                    // TODO: MAKE GLOBAL ENUM FOR AUDIOEFFECTS AND MAKE VARIABLE OUT OF THAT AND DO SWITCH CHECK HERE WHICH AUDIO TO PLAY!
                    //Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.IceRay);
                }

                float hitDistance = hit.distance;
                AfterRay(transPos, hit.point, wasHit: true);
                //Debug.DrawLine(transform.position, hit.point, Color.red, 1.0f, false);
                lineRenderer.SetPosition(1, new Vector3(0f, 0f, hitDistance));
            }
            else
            {
                AfterRay(transPos, transPos + transform.forward * rayMaxDistance, wasHit: false);
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
                lineRenderer.material.SetFloat("_Transparency", currentFadeOffAmount);
            }
        }

        private void HoldRayInterval()
        {
            IsHoldRayIntervalRunning = false;
        }
    }
}