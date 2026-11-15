
using System.Collections;
using TMPro;
using UnityEngine;

public class TMPScatteredReveal : MonoBehaviour
{
    [Header("Text Reference")]
    [SerializeField] private TMP_Text targetText;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 5f;
    [SerializeField] private float characterDelay = 0.035f;

    [Header("Scattering")]
    [SerializeField] private float horizontalScatter = 40f;
    [SerializeField] private float verticalScatter = 25f;
    [SerializeField] private float depthScatter = 30f;

    private Vector3[][] originalVertices;
    private Color32[][] originalColors;

    private Coroutine animationCoroutine;

    void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<TMP_Text>();

        if (targetText == null)
            targetText = GetComponentInChildren<TMP_Text>(true);
    }

    // Automatically starts whenever the door becomes active
    void OnEnable()
    {
        if (targetText == null)
            return;

        targetText.ForceMeshUpdate();

        SaveOriginalMesh();

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(RevealText());
    }

    void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    void SaveOriginalMesh()
    {
        TMP_TextInfo textInfo = targetText.textInfo;

        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        originalColors = new Color32[textInfo.meshInfo.Length][];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] =
                (Vector3[])textInfo.meshInfo[i].vertices.Clone();

            originalColors[i] =
                (Color32[])textInfo.meshInfo[i].colors32.Clone();
        }
    }

    IEnumerator RevealText()
    {
        TMP_TextInfo textInfo = targetText.textInfo;

        int characterCount = textInfo.characterCount;

        // Random starting positions for each character
        Vector3[] scatterOffsets = new Vector3[characterCount];

        for (int i = 0; i < characterCount; i++)
        {
            scatterOffsets[i] = new Vector3(
                Random.Range(-horizontalScatter, horizontalScatter), // X
                Random.Range(-verticalScatter, verticalScatter),     // Y
                Random.Range(-depthScatter, depthScatter)            // Z
            );
        }

        // Make all characters invisible and scattered
        UpdateTextMesh(0f, scatterOffsets);

        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            for (int i = 0; i < characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                float characterProgress = Mathf.Clamp01(
                    (elapsed - i * characterDelay) / animationDuration
                );

                // Smooth, natural movement
                characterProgress = Mathf.SmoothStep(
                    0f, 1f, characterProgress
                );

                UpdateCharacter(i, characterProgress, scatterOffsets);
            }

            targetText.UpdateVertexData(
                TMP_VertexDataUpdateFlags.Vertices |
                TMP_VertexDataUpdateFlags.Colors32
            );

            // Stop when every character has finished
            if (elapsed >= animationDuration +
                (characterCount - 1) * characterDelay)
                break;

            yield return null;
        }

        // Restore exact original positions and colors
        UpdateTextMesh(1f, scatterOffsets);

        animationCoroutine = null;
    }

    void UpdateCharacter(
        int index,
        float progress,
        Vector3[] scatterOffsets)
    {
        TMP_CharacterInfo charInfo =
            targetText.textInfo.characterInfo[index];

        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;

        Vector3 offset = scatterOffsets[index] * (1f - progress);

        for (int j = 0; j < 4; j++)
        {
            originalVertexToCurrent(materialIndex, vertexIndex + j, offset);

            Color32 color = originalColors[materialIndex][vertexIndex + j];

            color.a = (byte)(color.a * progress);

            targetText.textInfo.meshInfo[materialIndex].colors32[vertexIndex + j] = color;
        }
    }

    void originalVertexToCurrent(
        int materialIndex,
        int vertexIndex,
        Vector3 offset)
    {
        targetText.textInfo.meshInfo[materialIndex].vertices[vertexIndex] =
            originalVertices[materialIndex][vertexIndex] + offset;
    }

    void UpdateTextMesh(float progress, Vector3[] scatterOffsets)
    {
        TMP_TextInfo textInfo = targetText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            UpdateCharacter(i, progress, scatterOffsets);
        }

        targetText.UpdateVertexData(
            TMP_VertexDataUpdateFlags.Vertices |
            TMP_VertexDataUpdateFlags.Colors32
        );
    }
}

