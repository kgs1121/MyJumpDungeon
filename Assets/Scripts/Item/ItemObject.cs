using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteracted();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.name}\n{data.description}";
        return str;
    }

    public void OnInteracted()
    {
        GameManager.Instance.Player.itemData = data;
        GameManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}

