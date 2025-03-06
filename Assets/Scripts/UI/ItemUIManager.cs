using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    public GameObject itemPrefab; // UI 슬롯 프리팹
    public Transform content;     // 부모 오브젝트

    public List<GameObject> haveItem = new List<GameObject>();

    public int maxItems;  // 최대 슬롯 개수

    private void Awake()
    {
        GameManager.Instance.ItemUIManager = this;
    }

    public void ShowItem()
    {
        // 생성된 슬롯이 5개 이상이면 첫 번째 슬롯 삭제
        if (content.childCount >= maxItems)
        {
            Destroy(content.GetChild(0).gameObject);  // 첫 번째 슬롯 삭제
            haveItem.RemoveAt(0);
        }

        // 새로운 슬롯 생성
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
