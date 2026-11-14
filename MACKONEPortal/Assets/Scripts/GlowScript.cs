using System.Collections;
using UnityEngine;

public class GlowScript : MonoBehaviour
{
    private Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeOut(3f));
            yield return StartCoroutine(FadeIn(3f));
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        Color color = rend.material.color;
        float startAlpha = color.a; //from opaque
        float targetAlpha = 0.2f; //to more transparent

        for (float t = 0; t < duration; t+= Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / duration); //fade from opaque to transparent
            rend.material.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        rend.material.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        Color color = rend.material.color;
        float startAlpha = color.a; //almost transparent
        float targetAlpha = 1f; //fully opaque again

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / duration); //fades from transparent to opaque
            rend.material.color = color;
            yield return null;
        }
        color.a = targetAlpha; //ends fully opaque
        rend.material.color = color;
    }
}
