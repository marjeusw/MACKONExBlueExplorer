using UnityEngine;

public class StudioLoader : MonoBehaviour
{
    [SerializeField] private int studioNumber;
    [SerializeField] private GameObject studioObject;

    private void Start()
    {
        ObjectFunctions objectManager = FindAnyObjectByType<ObjectFunctions>();

        if (objectManager == null)
        {
            Debug.LogError("[StudioLoader] ObjectManager not found!");
            return;
        }

        if (studioObject == null)
        {
            Debug.LogError("[StudioLoader] Studio object not assigned!");
            return;
        }

        objectManager.RegisterStudio(studioNumber, studioObject);

        Debug.Log("[StudioLoader] Studio " + studioNumber + " registered!");
    }
}