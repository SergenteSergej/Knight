using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateSpeed = 45.0f;
    [SerializeField] Vector3 forward = Vector3.forward;

    float _xRotate = 0.0f;

    const float _maxVerticalAngle = 60;
    const float _minVerticalAngle = -60;


    CharacterController _mCharacterController;
    private Camera _camera;

    [SerializeField] Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _mCharacterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") !=0)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Mouse Y") !=0)
        {
            _xRotate -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            _xRotate = Mathf.Clamp( _xRotate, _minVerticalAngle, _maxVerticalAngle);

            _camera.transform.localEulerAngles = new Vector3(_xRotate, 0, 0);
        }

        float turbo = Input.GetKey(KeyCode.LeftShift) ? 2 :1 ;
        float curSpeed = speed * Input.GetAxis("Vertical") * turbo;
        Vector3 realForward  = transform.TransformDirection(Vector3.forward);

        if (Input.GetButton("Horizontal"))
        {
            realForward += transform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal");
        }

        realForward = Vector3.ClampMagnitude(realForward, 1);

        _mCharacterController.SimpleMove(realForward * curSpeed);

        playerAnimator.SetFloat(Speed, curSpeed);
    }
}
