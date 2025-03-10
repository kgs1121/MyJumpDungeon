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

    private float[] axes; // 0이면 x축으로 이동, 1이면 y축으로 이동, 2이면 z축으로 이동

    private void Start()
    {
        startPosition = transform.position;
        // Vector3 축을 계산하는 배열
        axes = new float[3] { startPosition.x, startPosition.y, startPosition.z };
    }

    private void Update()
    {
        // 선택된 축에 대해 PingPong을 적용
        axes[(int)platformRotate] = startPosition[(int)platformRotate] + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // 계산된 값을 바탕으로 새 위치 설정
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
