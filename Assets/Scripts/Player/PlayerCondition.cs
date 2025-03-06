using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    
    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    private void Update()
    {
        stamina.Add(stamina.lossGain * Time.deltaTime);

        if(health.curValue == 0)
        {
            Die();
        }
    }

    public void Heal(float value)
    {
        health.Add(value);
    }

    public void DropDamage(float damage)
    {
        health.Minus(damage);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {

    }

    public bool UseStamina(float value)
    {
        if (stamina.curValue - value < 0f) return false;

        stamina.Minus(value);
        return true;
    }
}
