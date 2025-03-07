using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    public float jumpForce;
    private Vector2 movementInput;
    public LayerMask groundLayerMask;
    public float useStamina;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;  // x�� ȸ�� �ּҰ�
    public float maxXLook;  // x�� ȸ�� �ִ밪
    private float camCurXRot;  // x�� ȸ���� : �� �Ʒ� ���� ȸ��
    public float lookSensitivity;  // ���콺 �ΰ���
    private Vector2 mouseDelta;
    public bool canLook = true;

    private Rigidbody rb;
    private bool isJumping;
    private bool canJump = true;
    private float jumpCooldown = 0.01f;

    private bool onecheck; // �������� �ѹ� üũ
    private bool isDrop;  // �������� �ִ��� Ȯ��
    public float dropy;  // �������� y��ǥ
    public float groundy; // 

    public float airTime = 0f; // ���߿� �ӹ��� �ð�
    public float dropDam = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckDropPlayer();
        if (IsGrounded() && dropy - groundy > 10f)
        {
            GameManager.Instance.Player.condition.DropDamage((dropy - groundy) * dropDam);
            isDrop = false;
            dropy = groundy;
        }
            
        //airTime = 0f;
    }

    
    void FixedUpdate()
    {
        Move();

        if (isJumping && IsGrounded() && canJump && GameManager.Instance.Player.condition.UseStamina(useStamina))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
        }
    }
    

    

    private void LateUpdate()
    {
        if (canLook) CameraLook();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    public void CheckDropPlayer()
    {
        if (!isDrop && !IsGrounded() && rb.velocity.y < 0)
        {
            isDrop = true;
            onecheck = true;
        }
        if (isDrop && onecheck)
        {
            dropy = transform.position.y;
            onecheck = false;
        }
        if (rb.velocity.y > 0) onecheck = true;
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    
    public void OnJump(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started)
        {
            StartCoroutine(JumpCooldownRoutine());
            isJumping = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            canJump = false;
            isJumping = false;
        }
    }


    void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                groundy = transform.position.y;
                return true;
            }
        }
        
        return false;
    }

    IEnumerator JumpCooldownRoutine()
    {
        yield return new WaitForSeconds(jumpCooldown); // ��Ÿ�� ��ٸ�
        canJump = true; // ��Ÿ�� ������ ���� ����
    }

}
