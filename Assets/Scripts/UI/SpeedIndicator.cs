using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        GameManager.Instance.Player.condition.onTakeSpeed += Flash;
        flashSpeed = GameManager.Instance.Condition.lossGain * Buff.plusSpeed;
        
    }

    void Flash()
    {
        if (coroutine != null) StopCoroutine(coroutine);

        image.enabled = true;
        image.color = new Color(100f / 255f, 100f / 255f, 1f);
        coroutine = StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(100f / 255f, 100f / 255f, 1f, a);
            yield return null;
        }

        image.enabled = false;
    }
}
