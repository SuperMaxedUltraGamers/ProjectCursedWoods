﻿using CursedWoods.SaveSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CursedWoods
{
    public class NewCamController : MonoBehaviour, ISaveable
    {
        private const float MAX_HEIGHT_FROM_PLAYER = 8f;
        private const float MIN_HEIGHT_FROM_PLAYER = 1.5f;
        private const float CAM_PARENT_Y_OFFSET = 2f;
        private const float COMBAT_MIN_DIST_FROM_PLAYER = 81.5f; //81.5f if not using sqrtmag then 9f
        private const float COMBAT_MAX_DIST_FROM_PLAYER = 196f; //196f if not using sqrtmag then 14f

        //private const float CHECK_SPHERE_RADIUS = 0.25f;

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
        private float fadeInSpeed = 5f;
        private float fadeBackSpeed = 2.5f;
        private float fadeBackAlphaAmount = 0.4f;
        //private Color transparentMatColor;

        private List<Collider> currentColliders = new List<Collider>();
        private List<Collider> lastColliders = new List<Collider>();
        private List<Renderer> currentRenderers = new List<Renderer>();
        private List<Renderer> lastRenderers = new List<Renderer>();
        private List<Material[]> currentOgMats = new List<Material[]>();
        private List<Material[]> lastOgMats = new List<Material[]>();

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

            //transparentMatColor = transparentBlack.color;
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

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetFloat(SaveUtils.GetKey(keyPrefix, SaveUtils.CAMERA_ROT_Y_KEY), transform.rotation.eulerAngles.y);
        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name.Equals(GlobalVariables.CASTLE))
            {
                if (GameMan.Instance.CastleManager.UseStartPos)
                {
                    return;
                }
            }

            float rotY = saveSystem.GetFloat(SaveUtils.GetKey(keyPrefix, SaveUtils.CAMERA_ROT_Y_KEY), transform.rotation.eulerAngles.y);
            transform.rotation = Quaternion.Euler(0f, rotY, 0f);
        }

        private void LinecastToPlayer()
        {
            Vector3 castStartPos = camT.position - camT.forward * 0.1f;
            Vector3 castEndPos = playerT.position;
            currentColliders.Clear();
            currentRenderers.Clear();
            currentOgMats.Clear();

            while (Physics.Linecast(castStartPos, castEndPos, out RaycastHit hit, raycastMask))
            {
                Collider hitCollider = hit.collider;
                Collider[] hitGOColliders = hit.collider.GetComponentsInChildren<Collider>();
                for (int i = 0; i < hitGOColliders.Length; i++)
                {
                    Renderer rnd = hitGOColliders[i].GetComponent<Renderer>();
                    if (rnd != null)
                    {
                        bool containsCollider = false;
                        if (lastColliders.Contains(hitGOColliders[i]))
                        {
                            hitCollider = hitGOColliders[i];
                            containsCollider = true;
                        }

                        if (containsCollider)
                        {
                            int index = lastColliders.IndexOf(hitCollider);

                            currentColliders.Add(lastColliders[index]);
                            currentRenderers.Add(lastRenderers[index]);
                            currentOgMats.Add(lastOgMats[index]);
                        }
                        else
                        {
                            currentColliders.Add(hitGOColliders[i]);
                            Renderer hitRenderer = hitGOColliders[i].GetComponent<Renderer>();
                            currentRenderers.Add(hitRenderer);
                            currentOgMats.Add(hitRenderer.materials);
                        }
                    }
                }

                castStartPos = hit.point + camT.forward * 0.02f;
            }

            float alpha = transparentBlack.GetFloat("_AlphaValueInverse");
            if (currentColliders.Count > 0)
            {
                if (alpha < 1f)
                {
                    alpha += Time.deltaTime * fadeInSpeed;
                    if (alpha > 1f)
                    {
                        alpha = 1f;
                    }
                }
            }
            else
            {
                alpha = transparentBlack.GetFloat("_AlphaValueInverse");
                if (alpha > fadeBackAlphaAmount)
                {
                    alpha -= Time.deltaTime * fadeBackSpeed;
                    if (alpha < fadeBackAlphaAmount)
                    {
                        alpha = fadeBackAlphaAmount;
                    }
                }
            }

            transparentBlack.SetFloat("_AlphaValueInverse", alpha);

            if (!(currentColliders.Count == 0 && lastColliders.Count == 1 && alpha > fadeBackAlphaAmount))
            {
                for (int i = 0; i < lastColliders.Count; i++)
                {
                    if (lastColliders[i] != null)
                    {
                        lastRenderers[i] = lastColliders[i].GetComponent<Renderer>();
                        lastRenderers[i].materials = lastOgMats[i];
                    }
                }

                for (int i = 0; i < currentOgMats.Count; i++)
                {
                    int length = currentOgMats[i].Length;
                    Material[] tempMats = new Material[length];
                    for (int j = 0; j < length; j++)
                    {
                        tempMats[j] = transparentBlack;
                    }

                    currentRenderers[i].materials = tempMats;
                }

                if (lastColliders.Count > currentColliders.Count)
                {
                    for (int i = 0; i < lastColliders.Count; i++)
                    {
                        if (!currentColliders.Contains(lastColliders[i]))
                        {
                            lastRenderers[i].materials = lastOgMats[i];
                        }
                    }
                }

                lastColliders.Clear();
                lastRenderers.Clear();
                lastOgMats.Clear();

                for (int i = 0; i < currentColliders.Count; i++)
                {
                    lastColliders.Add(currentColliders[i]);
                }

                for (int i = 0; i < currentRenderers.Count; i++)
                {
                    lastRenderers.Add(currentRenderers[i]);
                }

                for (int i = 0; i < currentOgMats.Count; i++)
                {
                    lastOgMats.Add(currentOgMats[i]);
                }
            }
        }

        private void ExploreCamMovement(float deltaTime)
        {
            float dir;
            if (Input.GetMouseButton(GlobalVariables.MOUSE_MIDDLE_BUTTON))
            {
                // Mouse input is already frame independent so we do not multiply it with deltatime.
                dir = Input.GetAxisRaw(GlobalVariables.MOUSE_X) * 0.1f;
                /*
                float width = Screen.width;
                if (CharController.mousePos.x <= 1)
                {
                    dir = -1f;
                }
                else if (CharController.mousePos.x >= width - 1)
                {
                    dir = 1f;
                }
                */
            }
            else
            {
                // Controllers are frame depended so we multiply it with deltatime.
                dir = Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS) * deltaTime;
            }

            dir *= Settings.Instance.CameraRotInvert;
            Quaternion rotation = Quaternion.Euler(0f, dir * Settings.Instance.CameraRotationSpeed, 0f);
            // Maybe slerp the rotation
            //rotation *= transform.rotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100f);
            transform.rotation *= rotation;

            Vector3 camTPos = camT.position;
            Vector3 transPos = transform.position;
            Vector3 newCamPos;
            float moveAmount = Input.GetAxisRaw(GlobalVariables.VERTICAL_RS) * Settings.Instance.CameraZoomSpeed;
            if (moveAmount == 0f)
            {
                moveAmount = Input.GetAxisRaw(GlobalVariables.MOUSE_SCROLL) * 7f;
                newCamPos = camTPos + camT.forward * moveAmount;
            }
            else
            {
                newCamPos = camTPos + camT.forward * moveAmount * deltaTime;
            }

            float maxCamHeight = transPos.y + MAX_HEIGHT_FROM_PLAYER;
            float minCamHeight = transPos.y + MIN_HEIGHT_FROM_PLAYER;

            //newCamPos = CheckCamCollision(newCamPos);

            if (newCamPos.y < maxCamHeight && newCamPos.y > minCamHeight)
            {
                camT.position = newCamPos;
                // Prolly need to change to this when using mousescroll to zoom
                //camT.position = Vector3.Lerp(camT.position, newCamPos, deltaTime * combatZoomSmoothAmount * 10f);
            }
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

            Vector3 wantedPos = playerPosWithOffset + playerT.forward * playerVelMag * (camLeadAmount + ((camT.position.y - transform.position.y) / 10f) * deltaTime);
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

        /*
        private Vector3 CheckCamCollision(Vector3 pos)
        {
            if (Physics.CheckSphere(pos, CHECK_SPHERE_RADIUS))
            {
                return pos + camT.forward * CHECK_SPHERE_RADIUS;
            }

            return pos;
        }
        */
    }
}