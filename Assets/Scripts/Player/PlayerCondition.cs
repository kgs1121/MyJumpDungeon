using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    
    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition speed {  get { return uiCondition.speed; } }

    public event Action onTakeDamage;
    public event Action onTakeSpeed;

    private void Update()
    {
        stamina.Add(stamina.lossGain * Time.deltaTime);
        
        speed.Minus(speed.lossGain * Time.deltaTime);
        GameManager.Instance.Player.controller.moveSpeed = speed.curValue + speed.startValue;

        if (health.curValue == 0)
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

    public bool SpeedUp(float value)
    {
        if(speed.curValue + value > speed.maxValue) return false;

        speed.Add(value);
        onTakeSpeed?.Invoke();
        return true;
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
