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

            playerMover = playerT.gameObject.GetComponent<NewPlayerMover>();
            charController = playerT.gameObject.GetComponent<CharController>();
        }

        private void OnEnable()
        {
            // Shit is causing the GameMan to double.
            //GameMan.Instance.CharController.ControlTypeChanged += CamControlTypeChanged;
            charController.ControlTypeChanged += CamControlTypeChanged;
        }

        private void OnDisable()
        {
            /*
            if (GameMan.Instance != null)
            {
                GameMan.Instance.CharController.ControlTypeChanged -= CamControlTypeChanged;
            }
            */

            charController.ControlTypeChanged -= CamControlTypeChanged;
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
        }

        private void ExploreCamMovement(float deltaTime)
        {
            // TODO: Maybe slerp the rotation
            float dir = Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS);
            Quaternion rotation = Quaternion.Euler(0f, dir * rotationSpeed * deltaTime, 0f);
            transform.rotation *= rotation;

            Vector3 camTPos = camT.position;
            Vector3 transPos = transform.position;
            float moveAmount = Input.GetAxisRaw(GlobalVariables.VERTICAL_RS) * zoomSpeed * deltaTime;
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