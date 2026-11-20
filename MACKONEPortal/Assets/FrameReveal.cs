using UnityEngine;

public class FrameReveal : MonoBehaviour
{
    private TMPFade[] textFades;

    private void Awake()
    {
        textFades = GetComponentsInChildren<TMPFade>(true);
    }

    public void SetVisible(bool visible)
    {
        foreach (TMPFade fade in textFades)
        {
            fade.SetVisible(visible);
        }
    }
}