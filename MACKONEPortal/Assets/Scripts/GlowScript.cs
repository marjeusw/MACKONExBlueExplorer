using System.Collections;
using UnityEngine;

public class GlowScript : MonoBehaviour
{
    private Renderer rend;
    public GameObject light;

   

    float min = 0.1f;
    float max = 0.4f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = light.GetComponent<Renderer>();
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            float duration = Random.Range(1f, 3f);
            yield return StartCoroutine(FadeOut(duration));
            yield return StartCoroutine(FadeIn(duration));
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        Color color = rend.material.color;
        float startAlpha = color.a; //from opaque
        float targetAlpha = Random.Range(min, max); //to more transparent

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

    public IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        Color color = rend.material.color;
        float startAlpha = color.a;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            rend.material.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        rend.material.color = color;
    }

    //for the reset button so that glow goes back to non transparent
    public void ResetGlow()
    {
        StopAllCoroutines();

        Color color = rend.material.color;
        color.a = 1f; //fully opaque
        rend.material.color = color;

        StartCoroutine(FadeLoop()); //gotta start it again since it doesn't start through void start anymore after the reset
    }

}
