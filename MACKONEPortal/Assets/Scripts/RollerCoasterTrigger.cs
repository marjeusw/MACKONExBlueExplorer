using UnityEngine;

public class RollerCoasterTrigger : MonoBehaviour
{
    private ObjectFunctions objectManager;

    [SerializeField] private LayerMask PortalMaskLayer;



    private void Start()
    {
        objectManager = FindAnyObjectByType<ObjectFunctions>();

        if (objectManager == null)
        {
            Debug.LogError("no object functions oh noo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((PortalMaskLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        Debug.Log($"Entered trigger: {other.name}");
        if (objectManager != null)
        {
            objectManager.DisableRollerCoaster();
        }
    }

    
}