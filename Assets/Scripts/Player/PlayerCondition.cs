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
    public event Action onDie;

    private void Update()
    {
        stamina.Add(stamina.lossGain * Time.deltaTime);
        
        //speed.Minus(speed.lossGain * Time.deltaTime);
        //GameManager.Instance.Player.controller.moveSpeed = speed.curValue + speed.startValue;

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
        speed.SpeedAdd(value);
        GameManager.Instance.Player.controller.moveSpeed = speed.curValue;
        onTakeSpeed?.Invoke();
        return true;
    }

    public void Die()
    {
        onDie?.Invoke();
        health.curValue = health.maxValue;
        GameManager.Instance.Player.transform.position = GameManager.Instance.Player.controller.spwanPlayer;
    }

    public bool UseStamina(float value)
    {
        if (stamina.curValue - value < 0f) return false;

        stamina.Minus(value);
        return true;
    }

    public float OriginSpeed()
    {
        return speed.startValue;
    }

    
}
