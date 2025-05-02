using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float jumpHeight = 3.5f;
    public float gravity = -15f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    private float spikeDetectionRadius = 2f;
    public LayerMask hiddenSpikeMask;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    private Vector3 startPosition;
    private GameObject rotatingPlatforms;

    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    private Transform groundCheck;

    public delegate void PlayerDeathEvent();
    public event PlayerDeathEvent OnPlayerDeath;

    void Start()
    {
        rotatingPlatforms = GameObject.FindGameObjectWithTag("RotatingPlatform");
        _Start();
    }

    void _Start()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position;

        GameObject groundCheckObj = new GameObject("GroundCheck");
        groundCheckObj.transform.SetParent(transform);
        groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
        groundCheck = groundCheckObj.transform;

        if (rotatingPlatforms != null)
        {
            rotatingPlatforms.GetComponent<RotatingSpikes>().Deactivate();
        }

        GameObject[] hiddenSpikes = GameObject.FindGameObjectsWithTag("HiddenSpike");
        foreach (GameObject spike in hiddenSpikes)
        {
            MeshRenderer renderer = spike.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        CheckHiddenSpikes();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CheckHiddenSpikes()
    {
        if(Physics.CheckSphere(transform.position, spikeDetectionRadius, hiddenSpikeMask))
        {
            GameObject[] hiddenSpikes = GameObject.FindGameObjectsWithTag("HiddenSpike");
            foreach (GameObject spike in hiddenSpikes)
            {
                MeshRenderer renderer = spike.GetComponent<MeshRenderer>();
                if (renderer != null && !renderer.enabled)
                {
                    float distanceToSpike = Vector3.Distance(transform.position, spike.transform.position);
                    if (distanceToSpike <= 2*spikeDetectionRadius)
                    {
                        renderer.enabled = true;
                        if (rotatingPlatforms != null)
                        {
                            rotatingPlatforms.GetComponent<RotatingSpikes>().Activate();
                        }
                    }
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Spike") || 
            hit.gameObject.layer == LayerMask.NameToLayer("HiddenSpike"))
        {
            Die();
        }
    }

    void Die()
    {
        controller.enabled = false;
        transform.position = startPosition;
        velocity = Vector3.zero;
        controller.enabled = true;

        OnPlayerDeath?.Invoke();

        _Start();
    }
}
