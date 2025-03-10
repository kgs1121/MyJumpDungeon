using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Moveplatform : MonoBehaviour
{
    public Vector3 startPosition;

    public float speed = 1f;

    public float distance = 4f;

    [Range(0, 2)] public int platformRotate;

    public float movePlatform;

    private float[] axes; // 0�̸� x������ �̵�, 1�̸� y������ �̵�, 2�̸� z������ �̵�

    private void Start()
    {
        startPosition = transform.position;
        // Vector3 ���� ����ϴ� �迭
        axes = new float[3] { startPosition.x, startPosition.y, startPosition.z };
    }

    private void Update()
    {
        // ���õ� �࿡ ���� PingPong�� ����
        axes[(int)platformRotate] = startPosition[(int)platformRotate] + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // ���� ���� �������� �� ��ġ ����
        transform.position = new Vector3(axes[0], axes[1], axes[2]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(null);
    }

}
