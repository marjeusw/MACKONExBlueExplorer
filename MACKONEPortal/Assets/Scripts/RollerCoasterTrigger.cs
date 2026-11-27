using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class RollerCoasterTrigger : MonoBehaviour
{
    private ObjectFunctions objectManager;

    [SerializeField] private LayerMask PortalMaskLayer;

    [Header("rollercoaster parts")] //fancy header heh
    [SerializeField] private GameObject rollerCoaster;
    [SerializeField] private GameObject portalOne;
    //[SerializeField] private GameObject portalOneHalf;
    [SerializeField] private GameObject portalTwo;
    [SerializeField] private GameObject tracks;
    [SerializeField] private float delayrc = 1.0f;

    private void Awake()
    {
        objectManager = FindAnyObjectByType<ObjectFunctions>(); // gets the object manager ref

        if (objectManager == null)
        {
            Debug.LogError("no object functions oh noo"); //jusd in case its not there
        }
        else
        {
            //registers itself with objectfunctions so reset can find it
            objectManager.RegisterRollerCoaster(this);
        }
    }

    public void HandleChildTrigger(Collider other)
    {
        if ((PortalMaskLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        Debug.Log($"Entered trigger: {other.name}");
        DisableThisRollerCoaster();
    }

    public void DisableThisRollerCoaster()
    {
        StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(delayrc);
        SetPartsActive(false);
    }

    public void ResetThisRollerCoaster()
    {
        StopAllCoroutines();
        SetPartsActive(true);
    }


    private void SetPartsActive(bool state)
    {
        if (rollerCoaster != null) rollerCoaster.SetActive(state);
        if (portalOne != null) portalOne.SetActive(state);
        //if (portalOneHalf != null) portalOneHalf.SetActive(state);
        if (portalTwo != null) portalTwo.SetActive(state);
        if (tracks != null) tracks.SetActive(state);
    }
}