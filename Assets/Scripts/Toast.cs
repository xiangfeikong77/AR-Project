using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 *  Full solution found here: https://stackoverflow.com/a/52592601
 */
public class Toast : MonoBehaviour
{
    public void showToast(TextMeshProUGUI TextObject, string text,
    float duration)
    {
        StartCoroutine(showToastCOR(TextObject, text, duration));
    }

    private IEnumerator showToastCOR(TextMeshProUGUI TextObject, string text,
        float duration)
    {
        Color orginalColor = TextObject.color;

        TextObject.text = text;
        TextObject.enabled = true;

        yield return fadeOut(TextObject, false, duration);

        TextObject.enabled = false;
        TextObject.color = orginalColor;
    }

    private IEnumerator fadeOut(TextMeshProUGUI TextObject, bool fadeIn, float duration)
    {
        //Set lerp range
        float a = 1f;
        float b = 0f;

        Color currentColor = TextObject.color;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            TextObject.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
}
