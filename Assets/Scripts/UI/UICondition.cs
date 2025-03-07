using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;
    public Condition speed;

    void Start()
    {
        GameManager.Instance.Player.condition.uiCondition = this;
    }
}
