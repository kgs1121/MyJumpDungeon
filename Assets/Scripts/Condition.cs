using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float lossGain;
    public Image uiBar;

    void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float value)
    {
        curValue += value;
    }

    public void Minus(float value)
    {
        curValue -= Mathf.Max(curValue - value, 0);
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

}
