//since object manager is outside the prefabs of the windows its functions have to be called with this instead
//is for which studio belongs to the respective window


using System.Collections;
using UnityEngine;

public class StudioLoader : MonoBehaviour
{
    [SerializeField] private int studioNumber; //ref to which studio number it is
    [SerializeField] private GameObject studioObject;
    [SerializeField] private AudioSource windowAudio; //for handing over the audio source on the shutter
    [SerializeField] private float closingSoundDelay = 0.5f; //for delay of window sound close

    private DoorGazeBehaviour doorBehaviour;

    private ObjectFunctions objectManager;

    private bool openingSoundPlayed = false; //to keep the sound from playing when window is already opened

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

        //same for the windows (for reset button)
        objectManager.RegisterWindow(doorBehaviour);
    
    }

    //for the reset button
    public void ResetLoaderState()
    {
        openingSoundPlayed = false;
        StopAllCoroutines();
        //no delayed sound playing after pressing reset
        objectManager.ResetRollerCoaster(); //resets the rollercoasters
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
        objectManager.ShowStudio(studioNumber, openingSoundPlayed ? null : windowAudio); //? : is short for if else -> if openingsound true then no audio, if false play audio
        openingSoundPlayed = true;
    }

    public void OnDoorClosed()
    {
        openingSoundPlayed = false; //resets the bool for no window playing sound twice after opening
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

    public void OnWindowFirstOpened() //reference to object manager (as always) since that ones from the prefab -> this ones for tracking if window was previously opened
    {
        if (objectManager != null)
        {
            objectManager.ActivateFirstOpenObject(studioNumber);
        }
    }

    public void OnGlowFinished()
    {
        if (objectManager != null)
        {
            objectManager.TryActivateFirstOpenObject();
        }
    }
}