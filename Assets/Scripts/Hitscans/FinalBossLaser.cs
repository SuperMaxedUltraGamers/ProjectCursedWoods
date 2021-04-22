using UnityEngine;

namespace CursedWoods
{
    public class FinalBossLaser : MonoBehaviour
    {
        private int dmgAmount;
        private DamageType dmgType;

        private LineRenderer lineRenderer;
        private float rayMaxDistance = 40f;
        private int raycastMask;
        private bool isHit;

        private Transform playerT;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();

            // All the integer presentations of layers we want the raycast to collide with.
            int[] layers = new int[2];
            layers[0] = 0;
            layers[1] = 8;

            // Create layermask from all the layers.
            foreach (int i in layers)
            {
                int layer = 1 << i;
                raycastMask |= layer;
            }

            gameObject.SetActive(false);
        }

        private void Start()
        {
            playerT = GameMan.Instance.PlayerT;
        }

        // Update is called once per frame
        private void Update()
        {
            ShootLaser();
        }

        public void Initialize(int damageAmount, DamageType damageType)
        {
            dmgAmount = damageAmount;
            dmgType = damageType;
            isHit = false;
        }

        private void ShootLaser()
        {
            Vector3 transPos = transform.position;
            Vector3 lookRot = Quaternion.LookRotation(playerT.position - transPos, Vector3.up).eulerAngles;
            Vector3 ogRot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(lookRot.x, ogRot.y, ogRot.z);

            RaycastHit hit;
            if (Physics.Raycast(transPos, transform.forward, out hit, rayMaxDistance, raycastMask))
            {
                if (!isHit)
                {
                    GameObject hitGO = hit.collider.gameObject;
                    if (hitGO.layer == GlobalVariables.PLAYER_LAYER)
                    {
                        IHealth otherHealth = hitGO.GetComponent<IHealth>();
                        if (otherHealth == null)
                        {
                            otherHealth = hitGO.GetComponentInParent<IHealth>();
                        }

                        otherHealth.DecreaseHealth(dmgAmount, dmgType);
                        isHit = true;
                    }
                }

                ParticleEffectBase hitParticles = (ParticleEffectBase)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MeleeHitParticles);
                hitParticles.Activate(hit.point, Quaternion.identity);

                float hitDistance = hit.distance;
                //AfterRay(transPos, hit.point, wasHit: true);
                //Debug.DrawLine(transform.position, hit.point, Color.red, 1.0f, false);
                lineRenderer.SetPosition(1, new Vector3(0f, 0f, hitDistance));
            }
            else
            {
                //AfterRay(transPos, transPos + transform.forward * rayMaxDistance, wasHit: false);
                //Debug.DrawLine(transform.position, transform.position + (transform.forward * rayMaxDistance), Color.green, 1f, false);
                lineRenderer.SetPosition(1, new Vector3(0f, 0f, rayMaxDistance));
            }
        }
    }
}