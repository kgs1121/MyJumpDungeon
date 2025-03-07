using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾� �±װ� �´��� Ȯ��
        if (collision.collider.CompareTag("Player"))
        {
            // �浹 ������ �������� Ȯ��
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log("Contact normal: " + contact.normal);
                // �������� ���� ���Ͱ� �Ʒ����� ��쿡�� ���� �߰�
                if (contact.normal.y < 0f)  // ���� ������ Y���� �����̸�
                {
                    if (collision.collider != null)
                    {
                        // �� �������� �� �߰�
                        collision.rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.VelocityChange);
                    }
                    break;  // ù ��° ���˸� ó���ϸ� �ǹǷ� ���� ����
                }
            }
        }
    }
}

            
