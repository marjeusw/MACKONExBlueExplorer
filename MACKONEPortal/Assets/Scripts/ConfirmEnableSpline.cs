using UnityEngine;

public class ConfirmEnableSpline : MonoBehaviour
{

    public Pointer myPointerScript;
    

    public void OnConfirmExecute()
    {
        myPointerScript.enabled = true;
    }
}
