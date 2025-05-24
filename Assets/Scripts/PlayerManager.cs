using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Strafe = Animator.StringToHash("Strafe");

    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateSpeed = 45.0f;

    float _xRotate = 0.0f;

    const float _maxVerticalAngle = 60;
    const float _minVerticalAngle = -60;

    CharacterController _mCharacterController;
    private Camera _camera;

    [SerializeField] Animator playerAnimator;

    void Start()
    {
        _camera = Camera.main;
        _mCharacterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseRotation();
        HandleMovement();
    }

    private void HandleMouseRotation()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            _xRotate -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            _xRotate = Mathf.Clamp(_xRotate, _minVerticalAngle, _maxVerticalAngle);

            _camera.transform.localEulerAngles = new Vector3(_xRotate, 0, 0);
        }
    }

    private void HandleMovement()
    {
        float turbo = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float curForwardSpeed = speed * verticalInput * turbo;
        float curStrafeSpeed = speed * horizontalInput * turbo;

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        _mCharacterController.SimpleMove(moveDirection * speed * turbo);

        // Normalizzazione per l'animazione
        float normalizedForward = verticalInput;
        float normalizedStrafe = horizontalInput;

        playerAnimator.SetFloat(Speed, normalizedForward);
        playerAnimator.SetFloat(Strafe, normalizedStrafe);
    }
}
