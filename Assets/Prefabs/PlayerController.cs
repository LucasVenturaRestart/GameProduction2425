using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Vector2 _movementDirection = new Vector2();
    private Rigidbody _rb;

    private float mouseX;
    private float mouseY;

    private float playerXRotation;
    private float playerYRotation;

    [SerializeField] private float _cameraSensitivity;
    [SerializeField] private Transform _cameraTransformHolder;
    [SerializeField] private Vector3 _initialCameraRotation;

    [SerializeField] private float _jumpForce = 500;

    private bool _grounded = true;

    [SerializeField] private float _movementSpeed = 1;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.transform.position = _cameraTransformHolder.position;

    }

    private void Update() {
        if(Input.GetKey(KeyCode.W))
        {
            _movementDirection.y = 1;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            _movementDirection.y = -1;
        }
        else
        {
            _movementDirection.y = 0;
        }
        

        if(Input.GetKey(KeyCode.A))
        {
            _movementDirection.x = -1;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            _movementDirection.x = 1;
        }
        else
        {
            _movementDirection.x = 0;
        }

        RaycastHit hit; 
        Physics.Raycast(transform.position, - Vector3.up, out hit, 1.1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, -Vector3.up * 1.1f, Color.red);

        if(hit.collider != null)
        {
            _grounded = true;
        }
        else
        {
            _rb.AddForce(Vector3.up * -300, ForceMode.Force);
            _grounded = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _grounded = false;
            _rb.AddForce(Vector3.up * _jumpForce ,ForceMode.Impulse);
        }
       
        Vector3 movement = _cameraTransformHolder.forward * _movementDirection.y + _cameraTransformHolder.right * _movementDirection.x;
        _rb.velocity = movement.normalized * _movementSpeed;



    }

    private void LateUpdate() {

        mouseX = Input.GetAxis("Mouse X") * _cameraSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * _cameraSensitivity * Time.deltaTime;

        playerXRotation -= mouseY;
        playerXRotation = Mathf.Clamp(playerXRotation, 0, 45);
        playerYRotation += mouseX;

        Camera.main.transform.parent = _cameraTransformHolder;
        Camera.main.transform.rotation = Quaternion.Euler(playerXRotation,0,0);

        _cameraTransformHolder.rotation = Quaternion.Euler(0, playerYRotation,0);

        transform.rotation = Quaternion.Euler(0,playerYRotation,0);
        transform.Rotate(0,playerXRotation,0); 
    }
}
