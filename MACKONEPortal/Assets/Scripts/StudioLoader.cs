//since object manager is outside the prefabs of the windows its functions have to be called with this instead
//is for which studio belongs to the respective window


using System.Collections;
using UnityEngine;

public class StudioLoader : MonoBehaviour
{
    [SerializeField] private int studioNumber;
    [SerializeField] private GameObject studioObject;
    [SerializeField] private AudioSource windowAudio; //for handing over the audio source on the shutter
    [SerializeField] private float closingSoundDelay = 0.5f; //for delay of window sound close

    private DoorGazeBehaviour doorBehaviour;

    private ObjectFunctions objectManager;

    private void Start()
    {
        doorBehaviour = GetComponent<DoorGazeBehaviour>();

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
        objectManager.RegisterStudio(studioNumber, studioObject, windowAudio);

    
    }

    public void ActivateStudio()
    {
        
        if (objectManager == null)
        {
            Debug.LogError("[StudioLoader] ObjectManager not found!");
            return;
        }

        if (doorBehaviour != null && doorBehaviour.isClosed)
        {
            return;
        }

       
        //same here
        objectManager.ShowStudio(studioNumber, windowAudio);
    }

    public void OnDoorClosed()
    {
        //unlocks studio switching
        if (objectManager != null)
        {
            objectManager.UnlockWindow(studioNumber);
        }

        //plays the closing sound afdter delay
        StartCoroutine(PlayClosingSound());

    }

    private IEnumerator PlayClosingSound()
    {
        yield return new WaitForSeconds(closingSoundDelay);

        if (windowAudio != null)
        {

            windowAudio.Play();
        }
    }
}