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
    public float minXLook;  // x축 회전 최소값
    public float maxXLook;  // x축 회전 최대값
    private float camCurXRot;  // x축 회전값 : 위 아래 방향 회전
    public float lookSensitivity;  // 마우스 민감도
    private Vector2 mouseDelta;
    public bool canLook = true;

    private Rigidbody rb;
    private bool isJumping;
    private bool canJump = true;
    private float jumpCooldown = 0.01f;

    public float airTime = 0f; // 공중에 머무른 시간
    public float dropDam = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsGrounded())
        {
            airTime += Time.deltaTime;
        }
        else
        {
            if (airTime > 4f)
            {
                GameManager.Instance.Player.condition.DropDamage(airTime * dropDam);
            }
            
            airTime = 0f;
        }
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
                return true;
            }
        }

        return false;
    }

    IEnumerator JumpCooldownRoutine()
    {
        yield return new WaitForSeconds(jumpCooldown); // 쿨타임 기다림
        canJump = true; // 쿨타임 끝나면 점프 가능
    }

    void ClearConsole()
    {
#if UNITY_EDITOR
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
#endif
    }
}
