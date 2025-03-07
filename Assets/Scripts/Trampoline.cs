using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;


    void OnDrawGizmos()
    {
        // �ݶ��̴� ��������
        Collider collider = GetComponent<Collider>();
        if (collider == null) return;  // �ݶ��̴��� ������ ����

        Vector3 colliderSize = collider.bounds.size;
        Vector3 colliderCenter = collider.bounds.center;

        // Ray ������ �����մϴ�.
        Ray[] rays = new Ray[4]
        {
        new Ray(colliderCenter + (transform.forward * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (-transform.forward * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (transform.right * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down),
        new Ray(colliderCenter + (-transform.right * 0.5f) + (transform.up * (colliderSize.y * 0.5f)), Vector3.down)
        };

        // ������ �� �信�� �� �� �ֵ��� Gizmos�� �׷��ݴϴ�.
        Gizmos.color = Color.red;  // ���� ���� ���� (�ʿ信 ���� ����)
        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i].origin, rays[i].direction * 0.1f);  // 0.1f�� Ray�� ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾� �±װ� �´��� Ȯ��
        if (collision.collider.CompareTag("Player"))
        {
            // �浹 ������ �������� Ȯ��
            foreach (ContactPoint contact in collision.contacts)
            {
                // �������� ���� ���Ͱ� ������ ��쿡�� ���� �߰�
                if (contact.normal.y > 0.5f)  // ���� ������ Y���� ����� ������ ���ϸ�
                {
                    Rigidbody playerRigidbody = collision.collider.GetComponent<Rigidbody>();

                    if (playerRigidbody != null)
                    {
                        // ���� ���� �߰�, �������� jumpForce ������ ����
                        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
                    }
                    break;  // ù ��° ���˸� ó���ϸ� �ǹǷ� ���� ����
                }
            }
        }
    }
}

            
