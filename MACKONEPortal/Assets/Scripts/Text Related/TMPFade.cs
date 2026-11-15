
using System.Collections;
using TMPro;
using UnityEngine;

public class TMPFade : MonoBehaviour
{
    [Header("Text Reference")]
    [SerializeField] private TMP_Text targetText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Starting Effects")]
    [SerializeField] private float startingSizeMultiplier = 0.9f;
    [SerializeField] private float startingCharacterSpacing = -27.7f;

    private float originalFontSize;
    private float originalCharacterSpacing;

    private Coroutine fadeCoroutine;
    private bool isVisible = false;

    void Awake()
    {
        // Find TMP component if not manually assigned
        if (targetText == null)
            targetText = GetComponentInChildren<TMP_Text>(true);

        if (targetText == null)
        {
            Debug.LogWarning("No TextMeshPro component found on " + gameObject.name);
            return;
        }

        // Save original text settings
        originalFontSize = targetText.fontSize;
        originalCharacterSpacing = targetText.characterSpacing;

        // Start invisible
        targetText.alpha = 0f;
        targetText.fontSize = originalFontSize * startingSizeMultiplier;
        targetText.characterSpacing =
            originalCharacterSpacing + startingCharacterSpacing;
    }

    public void SetVisible(bool visible)
    {
        if (targetText == null || isVisible == visible)
            return;

        isVisible = visible;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(AnimateText(visible));
    }

    IEnumerator AnimateText(bool show)
    {
        float startAlpha = targetText.alpha;
        float startSize = targetText.fontSize;
        float startSpacing = targetText.characterSpacing;

        float targetAlpha = show ? 1f : 0f;

        float targetSize = show
            ? originalFontSize
            : originalFontSize * startingSizeMultiplier;

        float targetSpacing = show
            ? originalCharacterSpacing
            : originalCharacterSpacing + startingCharacterSpacing;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            targetText.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            targetText.fontSize = Mathf.Lerp(startSize, targetSize, t);
            targetText.characterSpacing =
                Mathf.Lerp(startSpacing, targetSpacing, t);

            yield return null;
        }

        // Ensuring that the final values are exact
        targetText.alpha = targetAlpha;
        targetText.fontSize = targetSize;
        targetText.characterSpacing = targetSpacing;

        fadeCoroutine = null;
    }
}
