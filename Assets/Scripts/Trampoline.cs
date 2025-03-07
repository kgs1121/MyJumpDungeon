using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어 태그가 맞는지 확인
        if (collision.collider.CompareTag("Player"))
        {
            // 충돌 지점의 접촉점을 확인
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log("Contact normal: " + contact.normal);
                // 접촉점의 법선 벡터가 아래쪽인 경우에만 힘을 추가
                if (contact.normal.y < 0f)  // 법선 벡터의 Y값이 음수이면
                {
                    if (collision.collider != null)
                    {
                        // 위 방향으로 힘 추가
                        collision.rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
                    }
                    break;  // 첫 번째 접촉만 처리하면 되므로 루프 종료
                }
            }
        }
    }
}

            
