using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    public GameObject itemPrefab; // UI ���� ������
    public Transform content;     // �θ� ������Ʈ

    public List<GameObject> haveItem = new List<GameObject>();

    public int maxItems;  // �ִ� ���� ����

    private void Awake()
    {
        GameManager.Instance.ItemUIManager = this;
    }

    public void ShowItem()
    {
        // ������ ������ 5�� �̻��̸� ù ��° ���� ����
        if (content.childCount >= maxItems)
        {
            Destroy(content.GetChild(0).gameObject);  // ù ��° ���� ����
            haveItem.RemoveAt(0);
        }

        // ���ο� ���� ����
        GameObject slot = Instantiate(itemPrefab, content);
        haveItem.Add(slot);
        slot.transform.localPosition = Vector3.zero;
    }

    public void RemoveItem()
    {
        haveItem.Remove(haveItem[haveItem.Count - 1]);
        Destroy(content.GetChild(0).gameObject);
    }
}
