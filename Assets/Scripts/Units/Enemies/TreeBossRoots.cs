using UnityEngine;

namespace CursedWoods
{
    public class TreeBossRoots : MonoBehaviour
    {
        private enum MoveType
        {
            Null, // Nothing happens
            Static, // Doesn't move, but keeps time
            Track, // Tracks players position underground
            Raise, // Moving up
            Lower // Moving down
        }

        private const float MAX_ROT_SPEED = 300f;
        private const float ROT_ACCEL = 300f;

        private MoveType moveType = MoveType.Static;
        private Vector3 downPos;
        private Vector3 upPos;
        private float raiseTime = 0.2f;
        private float lowerTime = 1f;
        private float trackTime = 1f;
        private float stayUpTime = 0.8f;
        private float elapsedTime;
        private float trackSpeed = 6f;
        private float rotSpeed;
        private Transform playerT;

        private int damageAmount;
        private DamageType damageType;

        private bool canHit;

        [SerializeField]
        private ParticleSystem trackParticles;
        [SerializeField]
        private ParticleSystem raiseParticles;

        private void Awake()
        {
            playerT = GameMan.Instance.PlayerT;
        }

        private void Update()
        {
            switch (moveType)
            {
                case MoveType.Static:
                    Static();
                    rotSpeed -= ROT_ACCEL * Time.deltaTime;
                    if (rotSpeed < 0f)
                    {
                        rotSpeed = 0f;
                    }
                    break;
                case MoveType.Track:
                    Track();
                    break;
                case MoveType.Raise:
                    Raise();
                    rotSpeed -= ROT_ACCEL * Time.deltaTime;
                    break;
                case MoveType.Lower:
                    Lower();
                    rotSpeed -= ROT_ACCEL * Time.deltaTime;
                    break;
            }

            Rotate();
        }

        public void StartAttack(int dmgAmount, DamageType dmgType)
        {
            damageAmount = dmgAmount;
            damageType = dmgType;
            Vector3 startPos = playerT.position;
            startPos.y = -6f;
            transform.position = startPos;

            elapsedTime = 0f;
            moveType = MoveType.Track;
            rotSpeed = 0f;

            Vector3 particlePos = startPos;
            particlePos.y = 0f;
            trackParticles.transform.position = particlePos;
            trackParticles.Play();
        }

        private void Rotate()
        {
            transform.rotation *= Quaternion.Euler(0f, rotSpeed * Time.deltaTime, 0f);
        }

        private void Static()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stayUpTime)
            {
                moveType = MoveType.Lower;
                elapsedTime = 0f;
            }
        }

        private void Track()
        {
            Vector3 trackPos = playerT.position;
            trackPos.y = -6f;
            transform.position = Vector3.Lerp(transform.position, trackPos, trackSpeed * Time.deltaTime);

            Vector3 particlePos = transform.position;
            particlePos.y = 0f;
            trackParticles.transform.position = particlePos;

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= trackTime)
            {
                downPos = transform.position;
                upPos = downPos;
                upPos.y = 0f;
                moveType = MoveType.Raise;
                canHit = true;
                elapsedTime = 0f;
                rotSpeed = MAX_ROT_SPEED;
                raiseParticles.transform.position = particlePos;
                raiseParticles.Play();
            }
        }

        private void Raise()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > raiseTime)
            {
                moveType = MoveType.Static;
                canHit = false;
                elapsedTime = 0f;
            }
            else
            {
                transform.position = Vector3.Lerp(downPos, upPos, elapsedTime / raiseTime);
            }
        }

        private void Lower()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > lowerTime)
            {
                moveType = MoveType.Null;
                elapsedTime = 0f;
            }
            else
            {
                transform.position = Vector3.Lerp(upPos, downPos, elapsedTime / raiseTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canHit)
            {
                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.PLAYER_LAYER)
                {
                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(damageAmount, damageType);
                    canHit = false;
                }

                ParticleEffectBase hitParticles = (ParticleEffectBase)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MeleeHitParticles);
                hitParticles.Activate(playerT.position, Quaternion.identity);
            }
        }
    }
}