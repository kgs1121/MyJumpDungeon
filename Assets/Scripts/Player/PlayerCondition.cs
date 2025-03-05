using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    
    Condition health { get { return uiCondition.health; } }
    //Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;


    public void Heal(float value)
    {
        health.Add(value);
    }

    public void DropDamage(float damage)
    {
        health.Minus(damage);
        onTakeDamage?.Invoke();
    }
}
