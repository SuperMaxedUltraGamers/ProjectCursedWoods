using System;
using UnityEngine;

public class CharController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [SerializeField]
    private float moveSpeed = 200f;
    private Vector3 moveAmount;
    private Vector3 smoothMoveVel;
    private Vector3 velocity;
    private Vector3 forwardDir, rightDir;

    private Rigidbody rb;

    private Transform camT;

    public event Action<Rigidbody, float> MoveEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camT = Camera.main.transform;
        CorrectDirections();
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    private void Movement()
    {
        CorrectDirections();

        Vector3 correctMovement = (rightDir * Input.GetAxisRaw(HORIZONTAL) + forwardDir * Input.GetAxisRaw(VERTICAL)) * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, correctMovement, ref smoothMoveVel, .15f);

        if (correctMovement.magnitude != 0f)
        {
            transform.forward = correctMovement.normalized;
        }

        velocity = new Vector3(moveAmount.x, rb.velocity.y, moveAmount.z);
    }

    private void Move(float deltaTime)
    {
        rb.velocity = new Vector3(velocity.x * deltaTime, velocity.y, velocity.z * deltaTime);

        //Calls CamController's FollowPlayer method
        MoveEvent?.Invoke(rb, deltaTime);
    }

    private void CorrectDirections()
    {
        //TODO: maybe create check so that these are only calculated if camera has rotated.
        forwardDir = camT.forward;
        forwardDir.y = 0f;
        forwardDir = forwardDir.normalized;
        rightDir = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forwardDir;
    }
}
