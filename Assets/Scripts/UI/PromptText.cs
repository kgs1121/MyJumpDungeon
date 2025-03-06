using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptText : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    void Start()
    {
        GameManager.Instance.Player.interaction.promptTexT = this;
    }
}
