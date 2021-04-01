using CursedWoods.Utils;
using UnityEngine;

namespace CursedWoods
{
    public class NewCamController : MonoBehaviour
    {
        private const float MAX_HEIGHT_FROM_PLAYER = 8f;
        private const float MIN_HEIGHT_FROM_PLAYER = 2f;
        private const float CAM_PARENT_Y_OFFSET = 2f;
        private const float COMBAT_MIN_DIST_FROM_PLAYER = 81.5f; //81.5f if not using sqrtmag then 9f
        private const float COMBAT_MAX_DIST_FROM_PLAYER = 196f; //196f if not using sqrtmag then 14f

        [SerializeField]
        private Transform playerT;
        private NewPlayerMover playerMover;
        private Transform camT;
        private CharController charController;

        [SerializeField]
        private float moveSpeed = 2f;
        //[SerializeField]
        //private float rotationSpeed = 5f;
        //[SerializeField]
        //private float zoomSpeed = 5f;
        [SerializeField, Tooltip("How far the camera is focusing in front of the player.")]
        private float camLeadAmount = 1.5f;

        private float combatZoomSmoothAmount = 22f;

        private float maxPlayerVelMagnitudeMultiplayer = 6f;

        private delegate void ControlTypeDel(float f);
        private ControlTypeDel controlTypeCamMoveDel;
        private ControlTypeDel controlTypeFollowPlayerDel;


        [SerializeField]
        private LayerMask raycastMask;
        [SerializeField]
        private Material transparentBlack;
        private float fadeSpeed = 5f;
        private bool isMatSet;
        private bool changeMatBack;

        private Material[] lastHitMats;
        private Collider lastHitColl;
        private Color transparentMatColor;
        private Renderer lastHitRenderer;
        private float stayBehindTimeBeforeFadeMat = 0f;
        private float stayedBehindTime;

        private Material[] lastHitMats2;
        private Collider lastHitColl2;
        private Color transparentMatColor2;
        private Renderer lastHitRenderer2;
        private bool isMatSet2;
        private bool changeMatBack2;
        private Material transparentBlack2;

        private void Awake()
        {
            camT = Camera.main.transform;
            if (controlTypeCamMoveDel == null)
            {
                controlTypeCamMoveDel = ExploreCamMovement;
            }

            if (controlTypeFollowPlayerDel == null)
            {
                controlTypeFollowPlayerDel = ExploreFollowPlayer;
            }

            playerMover = playerT.gameObject.GetComponent<NewPlayerMover>();
            charController = playerT.gameObject.GetComponent<CharController>();

            transparentMatColor = transparentBlack.color;

            transparentBlack2 = transparentBlack;
            transparentMatColor2 = transparentBlack2.color;
        }

        private void OnEnable()
        {
            // Shit is causing the GameMan to double.
            //GameMan.Instance.CharController.ControlTypeChanged += CamControlTypeChanged;
            //charController.ControlTypeChanged += CamControlTypeChanged;
            CharController.ControlTypeChanged += CamControlTypeChanged;
        }

        private void OnDisable()
        {
            /*
            if (GameMan.Instance != null)
            {
                GameMan.Instance.CharController.ControlTypeChanged -= CamControlTypeChanged;
            }
            */

            //charController.ControlTypeChanged -= CamControlTypeChanged;
            CharController.ControlTypeChanged -= CamControlTypeChanged;
        }

        private void Update()
        {
            if (!charController.IgnoreControl)
            {
                if (!charController.IgnoreCameraControl)
                {
                    controlTypeCamMoveDel(Time.deltaTime);
                }

                controlTypeFollowPlayerDel(Time.deltaTime);
            }

            LinecastToPlayer();
        }

        private bool secondCastHasHit;
        private Collider temp2;

        private void LinecastToPlayer()
        {
            RaycastHit hit;
            if (Physics.Linecast(camT.position, playerT.position, out hit, raycastMask))
            {
                if (stayedBehindTime > stayBehindTimeBeforeFadeMat)
                {
                    
                    if (transparentMatColor.a > 0.3f)
                    {
                        transparentMatColor.a -= Time.deltaTime * fadeSpeed;
                    }

                    transparentBlack.color = transparentMatColor;

                    Collider temp = hit.collider;
                    if (temp != lastHitColl)
                    {
                        transparentMatColor.a = 1f;
                        transparentBlack.color = transparentMatColor;
                        isMatSet = false;
                        if (lastHitColl != null)
                        {
                            if (secondCastHasHit)
                            {
                                if (temp == temp2)
                                {
                                    lastHitRenderer.materials = lastHitMats2;
                                }
                                else
                                {
                                    lastHitRenderer = lastHitColl.GetComponentInChildren<Renderer>();
                                    lastHitRenderer.materials = lastHitMats;
                                }

                                secondCastHasHit = false;
                            }
                            else
                            {
                                lastHitRenderer = lastHitColl.GetComponentInChildren<Renderer>();
                                lastHitRenderer.materials = lastHitMats;
                            }
                        }
                    }

                    if (!isMatSet)
                    {
                        lastHitColl = temp;
                        if (secondCastHasHit)
                        {
                            if (lastHitColl == temp2)
                            {
                                lastHitMats = lastHitMats2;
                            }
                            else
                            {
                                lastHitRenderer = lastHitColl.GetComponentInChildren<Renderer>();
                                lastHitMats = lastHitRenderer.materials;
                            }

                            secondCastHasHit = false;
                        }
                        else
                        {
                            lastHitRenderer = lastHitColl.GetComponentInChildren<Renderer>();
                            lastHitMats = lastHitRenderer.materials;
                        }

                        int length = lastHitMats.Length;
                        Material[] tempMats = new Material[length];
                        for (int i = 0; i < length; i++)
                        {
                            tempMats[i] = transparentBlack;
                        }

                        lastHitRenderer.materials = tempMats;
                        isMatSet = true;
                        changeMatBack = true;
                    }

                    RaycastHit hit2;
                    if (Physics.Linecast(hit.point + camT.forward * 0.1f, playerT.position, out hit2, raycastMask))
                    {
                        temp2 = hit2.collider;
                        if (temp2 != lastHitColl2)
                        {
                            transparentMatColor2.a = 1f;
                            transparentBlack2.color = transparentMatColor2;
                            isMatSet2 = false;
                            if (lastHitColl2 != null)
                            {
                                lastHitRenderer2 = lastHitColl2.GetComponentInChildren<Renderer>();
                                lastHitRenderer2.materials = lastHitMats2;
                            }
                        }

                        if (!isMatSet2)
                        {
                            lastHitColl2 = temp2;
                            lastHitRenderer2 = lastHitColl2.GetComponentInChildren<Renderer>();
                            lastHitMats2 = lastHitRenderer2.materials;
                            int length2 = lastHitMats2.Length;
                            Material[] tempMats2 = new Material[length2];
                            for (int i = 0; i < length2; i++)
                            {
                                tempMats2[i] = transparentBlack2;
                            }

                            lastHitRenderer2.materials = tempMats2;
                            isMatSet2 = true;
                            changeMatBack2 = true;
                        }

                        secondCastHasHit = true;
                    }
                    else
                    {
                        if (changeMatBack2)
                        {
                            if (transparentMatColor2.a < 0.75f)
                            {
                                transparentMatColor2.a += Time.deltaTime * fadeSpeed;
                                transparentBlack2.color = transparentMatColor2;
                            }
                            else
                            {
                                lastHitRenderer2.materials = lastHitMats2;
                                changeMatBack2 = false;
                                isMatSet2 = false;
                                stayedBehindTime = 0f;
                            }
                        }

                        secondCastHasHit = false;
                    }
                }
                else
                {
                    stayedBehindTime += Time.deltaTime;
                }
            }
            else
            {
                if (changeMatBack)
                {
                    if (transparentMatColor.a < 0.75f)
                    {
                        transparentMatColor.a += Time.deltaTime * fadeSpeed;
                        transparentBlack.color = transparentMatColor;
                    }
                    else
                    {
                        lastHitRenderer.materials = lastHitMats;
                        changeMatBack = false;
                        isMatSet = false;
                        stayedBehindTime = 0f;
                    }
                }

                secondCastHasHit = false;
            }
        }


        private void ExploreCamMovement(float deltaTime)
        {
            // TODO: Maybe slerp the rotation
            float dir = Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS);
            Quaternion rotation = Quaternion.Euler(0f, dir * Settings.Instance.CameraRotationSpeed * deltaTime, 0f);
            transform.rotation *= rotation;

            Vector3 camTPos = camT.position;
            Vector3 transPos = transform.position;
            float moveAmount = Input.GetAxisRaw(GlobalVariables.VERTICAL_RS) * Settings.Instance.CameraZoomSpeed * deltaTime;
            Vector3 newCamPos = camTPos + camT.forward * moveAmount;
            float maxCamHeight = transPos.y + MAX_HEIGHT_FROM_PLAYER;
            float minCamHeight = transPos.y + MIN_HEIGHT_FROM_PLAYER;
            if (newCamPos.y > maxCamHeight)
            {
                newCamPos = new Vector3(camTPos.x, maxCamHeight, camTPos.z);
            }
            else if (newCamPos.y < minCamHeight)
            {
                newCamPos = new Vector3(camTPos.x, minCamHeight, camTPos.z);
            }

            camT.position = newCamPos;
        }

        private void CombatCamMovement(float deltaTime)
        {
            //float distanceFromPlayer = Vector3.Distance(camT.position, playerT.position);
            Vector3 toPlayer = playerT.position - camT.position;
            float distanceFromPlayer = Vector3.SqrMagnitude(toPlayer);
            //print($"distance: {distanceFromPlayer}, sqrt: {sqrt}");
            if (distanceFromPlayer < COMBAT_MIN_DIST_FROM_PLAYER)
            {
                Vector3 newCamPos = camT.position + -camT.forward * Settings.Instance.CameraZoomSpeed * deltaTime;
                camT.position = Vector3.Lerp(camT.position, newCamPos, deltaTime * combatZoomSmoothAmount);
            }
            else if (distanceFromPlayer > COMBAT_MAX_DIST_FROM_PLAYER)
            {
                Vector3 newCamPos = camT.position + camT.forward * Settings.Instance.CameraZoomSpeed * deltaTime;
                camT.position = Vector3.Lerp(camT.position, newCamPos, deltaTime * combatZoomSmoothAmount);
            }
        }

        private void ExploreFollowPlayer(float deltaTime)
        {
            Vector3 playerPosWithOffset = new Vector3(playerT.position.x, playerT.position.y + CAM_PARENT_Y_OFFSET, playerT.position.z);
            //float playerVelMag = playerMover.Velocity.magnitude;
            float playerVelMag = playerMover.CharacterController.velocity.magnitude;
            if (playerVelMag > maxPlayerVelMagnitudeMultiplayer)
            {
                playerVelMag = maxPlayerVelMagnitudeMultiplayer;
            }

            Vector3 wantedPos = playerPosWithOffset + playerT.forward * playerVelMag * (camLeadAmount + ((camT.position.y - transform.position.y) / 10f));
            transform.position = Vector3.Lerp(transform.position, wantedPos, moveSpeed * deltaTime);
        }

        private void CombatFollowPlayer(float deltaTime)
        {
            Vector3 playerPosWithOffset = new Vector3(playerT.position.x, playerT.position.y + CAM_PARENT_Y_OFFSET, playerT.position.z);
            //float playerVelMag = playerMover.Velocity.magnitude;
            float playerVelMag = playerMover.CharacterController.velocity.magnitude;
            if (playerVelMag > maxPlayerVelMagnitudeMultiplayer)
            {
                playerVelMag = maxPlayerVelMagnitudeMultiplayer;
            }

            Vector3 wantedPos;
            if (playerVelMag <= 0.1f)
            {
                wantedPos = playerPosWithOffset + playerT.forward * camLeadAmount * (playerVelMag + 2f);
            }
            else
            {
                wantedPos = playerPosWithOffset + playerT.forward * camLeadAmount * playerVelMag;
                wantedPos += playerMover.Velocity * camLeadAmount / 1.7f;
            }

            transform.position = Vector3.Lerp(transform.position, wantedPos, moveSpeed * deltaTime);
        }

        private void CamControlTypeChanged()
        {
            if (controlTypeCamMoveDel == ExploreCamMovement)
            {
                controlTypeCamMoveDel = CombatCamMovement;
                controlTypeFollowPlayerDel = CombatFollowPlayer;
            }
            else
            {
                controlTypeCamMoveDel = ExploreCamMovement;
                controlTypeFollowPlayerDel = ExploreFollowPlayer;
            }
        }
    }
}