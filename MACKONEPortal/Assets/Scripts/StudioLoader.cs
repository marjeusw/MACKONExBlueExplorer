//since object manager is outside the prefabs of the windows its functions have to be called with this instead
//is for which studio belongs to the respective window


using UnityEngine;

public class StudioLoader : MonoBehaviour
{
    [SerializeField] private int studioNumber;
    [SerializeField] private GameObject studioObject;
    [SerializeField] private AudioSource windowAudio; //for handing over the audio source on the shutter

    private ObjectFunctions objectManager;

    private void Start()
    {
        objectManager = FindAnyObjectByType<ObjectFunctions>();

        //just a to be safe
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

       //the meat of the stuff (just calling the function in object manager)
        objectManager.RegisterStudio(studioNumber, studioObject);

    
    }

    public void ActivateStudio()
    {
        
        if (objectManager == null)
        {
            Debug.LogError("[StudioLoader] ObjectManager not found!");
            return;
        }

       
        //same here
        objectManager.ShowStudio(studioNumber, windowAudio);
    }
}