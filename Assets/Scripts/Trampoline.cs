using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;


    void OnDrawGizmos()
    {
        // 콜라이더 가져오기
        Collider collider = GetComponent<Collider>();
        if (collider == null) return;  // 콜라이더가 없으면 종료

        Vector3 colliderSize = collider.bounds.size;
        Vector3 colliderCenter = collider.bounds.center;

        // Ray 방향을 설정합니다.
        Ray[] rays = new Ray[4]
        {
        new Ray(colliderCenter + (transform.forward * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (-transform.forward * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (transform.right * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (-transform.right * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down)
        };

        // 광선을 씬 뷰에서 볼 수 있도록 Gizmos로 그려줍니다.
        Gizmos.color = Color.red;  // 광선 색상 설정 (필요에 따라 변경)
        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i].origin, rays[i].direction * 0.1f);  // 0.1f는 Ray의 길이
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어 태그가 맞는지 확인
        if (collision.collider.CompareTag("Player"))
        {
            // 충돌 지점의 접촉점을 확인
            foreach (ContactPoint contact in collision.contacts)
            {
                // 접촉점의 법선 벡터가 위쪽인 경우에만 힘을 추가
                if (contact.normal.y > 0.5f)  // 법선 벡터의 Y값이 양수로 위쪽을 향하면
                {
                    Rigidbody playerRigidbody = collision.collider.GetComponent<Rigidbody>();

                    if (playerRigidbody != null)
                    {
                        // 위로 힘을 추가, 점프력은 jumpForce 변수로 제어
                        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
                    }
                    break;  // 첫 번째 접촉만 처리하면 되므로 루프 종료
                }
            }
        }
    }
}

            
