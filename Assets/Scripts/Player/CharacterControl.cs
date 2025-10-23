using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Header("Speed")]
    public float baseSpeed = 5f;
    public float runSpeed = 10f;
    [Header("Jump")]
    public float jumpForce = 5;
    public float groundCheckDistance = 0.1f;
    public bool isJump = false;
    public LayerMask groundLayer;

    [Header("Rotation")]
    public float rotationSpeed = 10f;


    Animator anim;
    Rigidbody rb;
    Vector3 inputDir;           // raw input
    Vector3 moveDir;            // world-space movement dir (camera relative)

    PlayerStats playerStats;
    Transform cam;              // camera transform (can be assigned in inspector)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        if (cam == null && Camera.main != null) cam = Camera.main.transform;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDir = new Vector3(horizontal, 0f, vertical);

        Jump();

        // Convert input to camera-relative world direction
        if (cam != null)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cam.right;
            camRight.y = 0f;
            camRight.Normalize();

            moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;

        }
        else
        {
            moveDir = inputDir.normalized;
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);            
        }

        anim.SetBool("IsJump", !IsGrounded());
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float maxDistance = groundCheckDistance + 0.1f;
        return Physics.Raycast(origin, Vector3.down, maxDistance, groundLayer);
    }

    private void FixedUpdate()
    {
        Walk();
    }

    public void Walk()
    {
        // keep vertical velocity (gravity)
        Vector3 currentVel = rb.linearVelocity;

        if (moveDir.sqrMagnitude > 0.0001f) // moving
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            float speed = isRunning ? runSpeed : baseSpeed;

            // set horizontal velocity while preserving y
            rb.linearVelocity = new Vector3(moveDir.x * speed, currentVel.y, moveDir.z * speed);

            // smooth rotation toward movement direction
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(moveDir.x, 0f, moveDir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            anim.SetBool("IsIdle", false);
            anim.SetBool("IsWalk", !isRunning);
            anim.SetBool("IsRun", isRunning);

            // stamina/hunger change (note: this starts many coroutines if called every FixedUpdate)
            float staminaDelta = -playerStats.staminaDrainRate * (isRunning ? 2f : 1f);
            float hungerDelta = -playerStats.hungerDrainRate * (isRunning ? 2f : 1f);
            StartCoroutine(playerStats.ChangeStatOverTime(staminaChange: staminaDelta, HungerChange: hungerDelta, duration: 0.2f));
        }
        else // idle
        {
            // stop horizontal movement, preserve vertical velocity
            rb.linearVelocity = new Vector3(0f, currentVel.y, 0f);

            anim.SetBool("IsIdle", true);
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsRun", false);

            StartCoroutine(playerStats.ChangeStatOverTime(staminaChange: playerStats.staminaIntervalRate));
        }
    }
}
