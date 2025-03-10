using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public bool OnInteracted();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.name}\n{data.description}";
        return str;
    }

    public bool OnInteracted()
    {
        GameManager.Instance.Player.itemData = data;
        //GameManager.Instance.Player.addItem?.Invoke();
        if (data.type == ItemType.Consumable)
        {
            Destroy(gameObject);
            return true;
        }
        else return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

