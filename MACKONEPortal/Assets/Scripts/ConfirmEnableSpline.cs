using UnityEngine;

public class ConfirmEnableSpline : MonoBehaviour
{

    public Pointer myPointerScript;
    public bool hasConfirmedAnchorPos = false;

    public void OnConfirmExecute()
    {
        myPointerScript.enabled = true;
        hasConfirmedAnchorPos = true;
    }
}
