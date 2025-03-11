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
    public float minXLook;  // x�� ȸ�� �ּҰ�
    public float maxXLook;  // x�� ȸ�� �ִ밪
    private float camCurXRot;  // x�� ȸ���� : �� �Ʒ� ���� ȸ��
    public float lookSensitivity;  // ���콺 �ΰ���
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("ItemPassive")]
    public float speedBoost = 2f;   // �ӵ� ������
    public float speedDuration = 5f;     // ���ǵ� ȿ�� ���� �ð�
    public float returnSpeed = 2f;  // ���� ���� �ӵ��� ���ư��� �ӵ�


    public Rigidbody rb;
    public bool isJumping;

    private bool onecheck; // �������� �ѹ� üũ
    private bool isDrop;  // �������� �ִ��� Ȯ��
    public float dropy;  // �������� y��ǥ
    public float groundy; // �ٴ� y ��ǥ
    public float isDropDamage;

    public float dropDam = 1.5f;
    public Vector3 spwanPlayer;

    private Coroutine coroutine;

    private int groundCheckNum = 0;

    public bool isOnLadder;

    public MovePlatform[] movePlatforms;

    public LayerMask OnLadder; // ������ ���̾�

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
            rb.useGravity = false;  // �߷� ��Ȱ��ȭ
            Vector3 moveDirection;
            float lookCheck = transform.forward.x < 0 ? 1 : -1;

            if (!isJumping)
            {
                moveDirection = new Vector3(0, lookCheck * movementInput.y * moveSpeed, lookCheck * movementInput.x * moveSpeed); // y�� ���⸸ �̵�
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
        // ī�޶��� forward ���͸� ���� (�����¿� ���⸸ ����)
        Vector3 cameraForward = cameraContainer.forward;
        // ī�޶��� ��/��, ��/�� ������ ����
        float directionX = cameraForward.x; // �¿� ���� (�÷����� ������, ���̳ʽ��� ����)
        float directionZ = cameraForward.z; // ���� ���� (�÷����� ����, ���̳ʽ��� ����)
        // �����¿츸 ����
        float savedXRotation = cameraContainer.eulerAngles.x; // ���� (pitch)
        float savedYRotation = cameraContainer.eulerAngles.y; // �¿� (yaw)
        // ����� �����¿� ȸ�� ������ ȸ�� ����
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
                platform.movePlatformCollider.enabled = false;  // �� MovePlatform�� �ݶ��̴��� ��Ȱ��ȭ
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
