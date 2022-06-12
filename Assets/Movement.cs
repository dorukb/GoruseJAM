using UnityEngine;

//The script requires there is a CharacterController component on the object.
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;

    [Header("Camera Movement")]
    //[SerializeField] float mouseSensitivity = 3f;
    [SerializeField] bool lockCursor = true;

    [Tooltip(("Set to true for smoothed camera movement, to false for more snappy & responsive but unrealistic movement."))]
    [SerializeField] bool useCameraSmoothing = true;
    [SerializeField] [Range(0f, 0.5f)] float cameraSmoothTime = 0.05f;

    [Tooltip(("Effectively the angle with which the player can tilt the camera upwards"))]
    [SerializeField] float upwardsMaxTiltAngle = 75f;

    [Tooltip(("Effectively the angle with which the player can tilt the camera downwards"))]
    [SerializeField] float downwardsMaxTiltAngle = 80f;

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float gravity = -13f;

    [Tooltip(("Set to true for smoothed movement, to false for more snappy & responsive but unrealistic movement."))]
    [SerializeField] bool useMovementSmoothing = true;
    [SerializeField] [Range(0f, 0.5f)] float moveSmoothTime = 0.3f;

    // Private instance fields follow
    CharacterController _controller;
    float _cameraPitch = 0.0f;
    float _velocityY = 0.0f;

    // used for player movement smoothing
    Vector2 _currentDir = Vector2.zero;
    Vector2 _currentDirVelocity = Vector2.zero;

    // used for camera movement smoothing
    Vector2 _currentMouseDelta = Vector2.zero;
    Vector2 _currentMouseDeltaVelocity = Vector2.zero;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        // Lock mouse cursor to middle and make it invisible
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        UpdateCamera();
        UpdateMovement();
    }
    void UpdateCamera()
    {
        // get cursor position change(delta).
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (useCameraSmoothing)
        {
            // calculate smoothed pos
            _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, mouseDelta, ref _currentMouseDeltaVelocity, cameraSmoothTime);

            // use the smoothed pos
            mouseDelta = _currentMouseDelta;
        }
        // tilt the camera up & down based on vertical mouse movement.
        // Axis is inverted, hence the subtraction operation.
        _cameraPitch -= mouseDelta.y;

        // Clamp the camera rotation to prevent fully rotating it.
        _cameraPitch = Mathf.Clamp(_cameraPitch, -upwardsMaxTiltAngle, downwardsMaxTiltAngle);

        // Apply the rotation to the camera object.
        playerCamera.localEulerAngles = Vector3.right * _cameraPitch;

        // Turn the player left & right based on horizontal mouse movement.
        transform.Rotate(Vector3.up * mouseDelta.x * GameSettings.mouseSensitivity);
    }

    void UpdateMovement()
    {
        // Get WASD input values
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize for consistent velocity while moving diagonally
        moveInput.Normalize();

        if (useMovementSmoothing)
        {
            // calculate smoothed dir
            _currentDir = Vector2.SmoothDamp(_currentDir, moveInput, ref _currentDirVelocity, moveSmoothTime);

            // use the smoothed dir
            moveInput = _currentDir;
        }

        // custom falling down, gravity effect.
        if (_controller.isGrounded)
        {
            _velocityY = 0f;
        }
        _velocityY += gravity * Time.deltaTime;

        // Calculate velocity taking into account both directions on XZ plane, then add on the Y velocity for falling down.
        Vector3 velocity = (transform.forward * moveInput.y + transform.right * moveInput.x) * moveSpeed + Vector3.up * _velocityY;

        // Apply the velocity
        _controller.Move(velocity * Time.deltaTime);
    }
}