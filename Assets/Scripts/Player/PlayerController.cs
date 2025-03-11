using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 movementInput;
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
    public float speedBoost = 2f;   // 속도 증가량
    public float speedDuration = 5f;     // 스피드 효과 지속 시간
    public float returnSpeed = 2f;  // 점차 원래 속도로 돌아가는 속도


    public Rigidbody rb;
    public bool isJumping;

    private bool onecheck; // 떨어질때 한번 체크
    private bool isDrop;  // 떨어지고 있는지 확인
    public float dropy;  // 떨어질때 y좌표
    public float groundy; // 바닥 y 좌표
    public float isDropDamage;

    public float dropDam = 1.5f;
    public Vector3 spwanPlayer;

    private Coroutine coroutine;

    private int groundCheckNum = 0;

    public bool isOnLadder;

    public MovePlatform[] movePlatforms;

    public LayerMask OnLadder; // 무시할 레이어

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
        if (IsGrounded() && dropy - groundy > isDropDamage)
        {
            GameManager.Instance.Player.condition.DropDamage((dropy - groundy) * dropDam);
            isDrop = false;
            dropy = groundy;
        }
    }

    
    void FixedUpdate()
    {
        if (!isOnLadder)
        {
            Move();
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;  // 중력 비활성화
            Vector3 moveDirection;
            float lookCheck = transform.forward.x < 0 ? 1 : -1;

            if (!isJumping)
            {
                moveDirection = new Vector3(0, lookCheck * movementInput.y * moveSpeed, lookCheck * movementInput.x * moveSpeed); // y축 방향만 이동
                Debug.Log(transform.forward.x);
                rb.velocity = moveDirection;
            }
        }

        if (isJumping && IsGrounded() && GameManager.Instance.Player.condition.UseStamina(useStamina))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            groundCheckNum++;
        }
    }
    

    private void LateUpdate()
    {
        if (canLook) CameraLook();
    }

    public void SetLadder(bool state)
    {
        isOnLadder = state;
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
            isJumping = true;
            isOnLadder = false;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isJumping = false;
        }
    }

    
    void LookDirection()
    {
        // 카메라의 forward 벡터를 저장 (상하좌우 방향만 저장)
        Vector3 cameraForward = cameraContainer.forward;
        // 카메라의 상/하, 좌/우 방향을 추출
        float directionX = cameraForward.x; // 좌우 방향 (플러스면 오른쪽, 마이너스면 왼쪽)
        float directionZ = cameraForward.z; // 상하 방향 (플러스면 앞쪽, 마이너스면 뒤쪽)
        // 상하좌우만 저장
        float savedXRotation = cameraContainer.eulerAngles.x; // 상하 (pitch)
        float savedYRotation = cameraContainer.eulerAngles.y; // 좌우 (yaw)
        // 저장된 상하좌우 회전 값으로 회전 복원
        cameraContainer.eulerAngles = new Vector3(savedXRotation, savedYRotation, 0);
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
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                groundy = transform.position.y;
                
                if(groundCheckNum == 0) return true;
            }
        }

        if (isOnLadder)
        {
            groundy = transform.position.y;
            return true;
        }

        return false;
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

        if (collision.collider.CompareTag("Ladder"))
        {
            if (transform.parent != null) transform.parent = null;

            foreach (var platform in movePlatforms)
            {
                platform.movePlatformCollider.enabled = false;  // 각 MovePlatform의 콜라이더를 비활성화
            }
        }
        else
        {
            foreach (var platform in movePlatforms)
            {
                platform.movePlatformCollider.enabled = true;
            }
        }
    }
}
