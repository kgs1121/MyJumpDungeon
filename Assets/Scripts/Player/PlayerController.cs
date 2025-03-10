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

    [Header("ItemPassive")]
    public float speedBoost = 2f;   // 속도 배율
    public float speedDuration = 5f;     // 스피드 효과 지속 시간
    public float returnSpeed = 2f;  // 점차 원래 속도로 돌아가는 속도


    private Rigidbody rb;
    public bool isJumping;
    private bool canJump = true;
    private float jumpCooldown = 0.1f;

    private bool onecheck; // 떨어질때 한번 체크
    private bool isDrop;  // 떨어지고 있는지 확인
    public float dropy;  // 떨어질대 y좌표
    public float groundy; // 

    public float airTime = 0f; // 공중에 머무른 시간
    public float dropDam = 1.5f;
    public Vector3 spwanPlayer;

    private Coroutine coroutine;

    private int groundCheckNum = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        spwanPlayer = transform.position;
        moveSpeed = GameManager.Instance.Player.condition.OriginSpeed();
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
    }

    
    void FixedUpdate()
    {
        Move();
        

        if (isJumping && IsGrounded() && GameManager.Instance.Player.condition.UseStamina(useStamina))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
            groundCheckNum++;
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
        
        if (canJump && context.phase == InputActionPhase.Started)
        {
            isJumping = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
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
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.1f, Color.red);
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                groundy = transform.position.y;
                
                if(groundCheckNum == 0) return true;
            }
        }

        return false;
    }


    IEnumerator CanJump()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    public IEnumerator SpeedBoost()
    {
        float originspeed = GameManager.Instance.Player.condition.OriginSpeed();
        GameManager.Instance.Player.condition.SpeedUp(speedBoost);
        yield return new WaitForSeconds(speedDuration);

        float currentspeed = moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < returnSpeed)
        {
            elapsedTime += Time.deltaTime;
            moveSpeed = Mathf.Lerp(currentspeed, originspeed, elapsedTime / returnSpeed);
            yield return null;
        }

        moveSpeed = originspeed;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Buff"))
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(SpeedBoost());
        }

        if (((1 << collision.gameObject.layer) & groundLayerMask) != 0) groundCheckNum = 0;
    }
}
