using UnityEngine;

namespace CursedWoods
{
    public class CamController : MonoBehaviour
    {
        private const float MAX_HEIGHT_FROM_PLAYER = 8f;
        private const float MIN_HEIGHT_FROM_PLAYER = 2f;
        private const float CAM_PARENT_Y_OFFSET = 2f;
        private const float COMBAT_MIN_DIST_FROM_PLAYER = 9f;
        private const float COMBAT_MAX_DIST_FROM_PLAYER = 14f;

        [SerializeField]
        private Transform playerT;
        [SerializeField]
        private Rigidbody playerRb;
        private Transform camT;

        [SerializeField]
        private float moveSpeed = 2f;
        [SerializeField]
        private float rotationSpeed = 5f;
        [SerializeField]
        private float zoomSpeed = 5f;
        [SerializeField, Tooltip("How far the camera is focusing in front of the player.")]
        private float camLeadAmount = 1.5f;

        private float combatZoomSmoothAmount = 22f;

        private float maxPlayerVelMagnitudeMultiplayer = 6f;

        private delegate void ControlTypeDel(float f);
        private ControlTypeDel controlTypeCamMoveDel;
        private ControlTypeDel controlTypeFollowPlayerDel;

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
        }

        private void OnEnable()
        {
            CharController.ControlTypeChanged += CamControlTypeChanged;
        }

        private void OnDisable()
        {
            CharController.ControlTypeChanged -= CamControlTypeChanged;
        }

        private void FixedUpdate()
        {
            if (!CharController.IgnoreControl)
            {
                if (!CharController.IgnoreCameraControl)
                {
                    controlTypeCamMoveDel(Time.fixedDeltaTime);
                }

                controlTypeFollowPlayerDel(Time.fixedDeltaTime);
            }
        }

        private void ExploreCamMovement(float deltaTime)
        {
            // TODO: Maybe slerp the rotation
            float dir = Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS);
            Quaternion rotation = Quaternion.Euler(0f, dir * rotationSpeed * deltaTime, 0f);
            transform.rotation *= rotation;

            float moveAmount = Input.GetAxisRaw(GlobalVariables.VERTICAL_RS) * zoomSpeed * deltaTime;
            Vector3 newCamPos = camT.position + camT.forward * moveAmount;
            float maxCamHeight = transform.position.y + MAX_HEIGHT_FROM_PLAYER;
            float minCamHeight = transform.position.y + MIN_HEIGHT_FROM_PLAYER;
            if (newCamPos.y > maxCamHeight)
            {
                newCamPos = new Vector3(camT.position.x, maxCamHeight, camT.position.z);
            }
            else if (newCamPos.y < minCamHeight)
            {
                newCamPos = new Vector3(camT.position.x, minCamHeight, camT.position.z);
            }

            camT.position = newCamPos;
        }

        private void CombatCamMovement(float deltaTime)
        {
            float distanceFromPlayer = Vector3.Distance(camT.position, playerT.position);
            //print(distanceFromPlayer);
            if (distanceFromPlayer < COMBAT_MIN_DIST_FROM_PLAYER)
            {
                Vector3 newCamPos = camT.position + -camT.forward * zoomSpeed * deltaTime;
                camT.position = Vector3.Lerp(camT.position, newCamPos, deltaTime * combatZoomSmoothAmount);
            }
            else if (distanceFromPlayer > COMBAT_MAX_DIST_FROM_PLAYER)
            {
                Vector3 newCamPos = camT.position + camT.forward * zoomSpeed * deltaTime;
                camT.position = Vector3.Lerp(camT.position, newCamPos, deltaTime * combatZoomSmoothAmount);
            }
        }

        private void ExploreFollowPlayer(float deltaTime)
        {
            Vector3 playerPosWithOffset = new Vector3(playerT.position.x, playerT.position.y + CAM_PARENT_Y_OFFSET, playerT.position.z);
            float playerVelMag = playerRb.velocity.magnitude;
            if (playerVelMag > maxPlayerVelMagnitudeMultiplayer)
            {
                playerVelMag = maxPlayerVelMagnitudeMultiplayer;
            }

            Vector3 wantedPos = playerPosWithOffset + playerT.forward * camLeadAmount * playerVelMag;
            transform.position = Vector3.Lerp(transform.position, wantedPos, moveSpeed * deltaTime);
        }

        // Currently the same as ExploreFollowPlayer so just here if we want different behaviour in the future.
        private void CombatFollowPlayer(float deltaTime)
        {
            ExploreFollowPlayer(deltaTime);
            /*
            Vector3 playerPosWithOffset = new Vector3(playerT.position.x, playerT.position.y + CAM_PARENT_Y_OFFSET, playerT.position.z);
            float playerVelMag = playerRb.velocity.magnitude;
            if (playerVelMag > maxPlayerVelMagnitudeMultiplayer)
            {
                playerVelMag = maxPlayerVelMagnitudeMultiplayer;
            }

            Vector3 wantedPos = playerPosWithOffset + playerT.forward * camLeadAmount * playerVelMag;
            transform.position = Vector3.Lerp(transform.position, wantedPos, moveSpeed * deltaTime);
            */
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