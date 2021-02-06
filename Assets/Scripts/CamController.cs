using UnityEngine;

public class CamController : MonoBehaviour
{
    private const string HORIZONTAL_RS = "HorizontalRS";
    private const string VERTICAL_RS = "VerticalRS";
    private const float MAX_HEIGHT_FROM_PLAYER = 14f;
    private const float MIN_HEIGHT_FROM_PLAYER = 8f;
    private const float CAM_PARENT_Y_OFFSET = 2f;

    [SerializeField]
    private Transform playerT;
    private Transform camT;

    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float zoomSpeed = 5f;
    [SerializeField, Tooltip("How far the camera is focusing in front of the player.")]
    private float camLeadAmount = 1.5f;

    private void Awake()
    {
        camT = Camera.main.transform;
        //TODO: unsubscribe FollowPlayer from MoveEvent in some reasonable spot.
        playerT.gameObject.GetComponent<CharController>().MoveEvent += FollowPlayer;
    }

    private void Update()
    {
        CamMovement();
    }

    private void CamMovement()
    {
        //TODO: Maybe slerp the rotation
        float dir = Input.GetAxisRaw(HORIZONTAL_RS);
        Vector3 rotation = new Vector3(0f, dir  * rotationSpeed * Time.deltaTime, 0f);
        transform.Rotate(rotation);

        float moveAmount = Input.GetAxisRaw(VERTICAL_RS) * zoomSpeed * Time.deltaTime;
        Vector3 newCamPos = camT.position + camT.forward * moveAmount;
        float maxCamHeight = playerT.position.y + MAX_HEIGHT_FROM_PLAYER;
        float minCamHeight = playerT.position.y + MIN_HEIGHT_FROM_PLAYER;
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

    private void FollowPlayer(Rigidbody playerRb, float deltaTime)
    {
        Vector3 playerPosWithOffset = new Vector3(playerT.position.x, playerT.position.y + CAM_PARENT_Y_OFFSET, playerT.position.z);
        Vector3 wantedPos = playerPosWithOffset + playerT.forward * camLeadAmount * playerRb.velocity.magnitude;
        transform.position = Vector3.Lerp(transform.position, wantedPos, moveSpeed * deltaTime);
    }
}
