//handles all the functions that happen when each door is used
//mostly which studio should currently be visible

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectFunctions : MonoBehaviour
{
    //[SerializeField] private float closingSoundDelay = 0.5f;
    private bool windowIsOpen = false;
    private int openStudioNumber = -1;

    private int currentStudio = 1;

    [SerializeField] private GameObject firstOpenObject;
    [SerializeField] private float finalCracksDelay = 7f;
    private Coroutine finalCracksCoroutine; //so the final cracks tunring back on is not dependent on the glow anymore

    private bool[] windowsOpened = new bool[4]; //for all 4 windows to track for crack
    private int openedWindowCount = 0;
    private bool firstOpenObjectActivated = false; //for the coroutine

    private List<DoorGazeBehaviour> allWindows = new List<DoorGazeBehaviour>(); //for reset button

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
    public AudioSource audio4;

    public GameObject Studio1;
    public GameObject Studio2;
    public GameObject Studio3;
    public GameObject Studio4;

    public GameObject rcObject1; //roller coaster objects to turn on for animation after coroutine
    public GameObject rcObject2;
    public GameObject rcObject3;
    public GameObject rcObject4;

    [SerializeField] private float rcDelay = 7.0f;

    private Coroutine rcDelayCoroutine; //for tracking the coroutine for stopping it when resetting

    //for the rollercoasters to trigger
    private List<RollerCoasterTrigger> allRollerCoasters = new List<RollerCoasterTrigger>();


    //registers the windows for the reset button (same idea as the registerstudio to get the reference of each window)
    public void RegisterWindow(DoorGazeBehaviour door)
    {
        if (door != null && !allWindows.Contains(door))
            {
                allWindows.Add(door);
            }
    }

    //registers all rollercoasters of the windows (besides final cracks) cause they do be prefab stuff
    public void RegisterRollerCoaster(RollerCoasterTrigger coaster)
    {
        if (coaster != null && !allRollerCoasters.Contains(coaster))
        {
            allRollerCoasters.Add(coaster);
        }
    }

    //stores refs to all 4 studios and their audio (since after being instantiated, prefabs forgor all references that aren't in the prefab so studios gotta be reffed like dis)
    public void RegisterStudio(int studioNumber, GameObject studio, GameObject rollerCoasterObject, AudioSource windowAudio)
    {
        switch (studioNumber)
        {
            case 1:
                Studio1 = studio;
                audio1 = windowAudio;
                rcObject1 = rollerCoasterObject;
                break;

            case 2:
                Studio2 = studio;
                audio2 = windowAudio;
                rcObject2 = rollerCoasterObject;
                break;

            case 3:
                Studio3 = studio;
                audio3 = windowAudio;
                rcObject3 = rollerCoasterObject;
                break;

            case 4:
                Studio4 = studio;
                audio4 = windowAudio;
                rcObject4 = rollerCoasterObject;
                break;

            default:
                Debug.LogWarning("Invalid studio number: " + studioNumber);
                break;
        }

    }


   

    


    public void ShowStudio(int studioNumber, AudioSource windowAudio)
    {
        //no other studio opens while other window is still open
        if (windowIsOpen && openStudioNumber != studioNumber)
        {
            return;
        }
        //stops roollercoaster coroutine
        if (rcDelayCoroutine != null)
        {
            StopCoroutine(rcDelayCoroutine);
        }

        //locks studio switching until window closes
        windowIsOpen = true;
        openStudioNumber = studioNumber;

        //remembers which studio was last
        int previousStudio = currentStudio;

        
        if (windowAudio != null)
        {
            //Debug.Log("[ObjectFunctions] OPENING AUDIO: " + windowAudio.name + " | Parent: " + windowAudio.transform.parent.name + " | Position: " + windowAudio.transform.position);

            windowAudio.Play();
        }

        //turns off all studios first
        if (Studio1 != null) Studio1.SetActive(false);
        if (Studio2 != null) Studio2.SetActive(false);
        if (Studio3 != null) Studio3.SetActive(false);
        if (Studio4 != null) Studio4.SetActive(false);

        //turns on selected studio
        switch (studioNumber)
        {
            case 1:
                if (Studio1 != null) Studio1.SetActive(true);
                //startcoroutine for rollercoasterobj
                rcDelayCoroutine = StartCoroutine(RollerCoasterDelay(rcObject1));

                break;
            case 2:
                if (Studio2 != null) Studio2.SetActive(true);
                rcDelayCoroutine = StartCoroutine(RollerCoasterDelay(rcObject2));
                break;
            case 3:
                if (Studio3 != null) Studio3.SetActive(true);
                rcDelayCoroutine = StartCoroutine(RollerCoasterDelay(rcObject3)); //calling the coroutine and also making rcDelayCoroutine not null (for ref in reset function)
                break;
            case 4:
                if (Studio4 != null) Studio4.SetActive(true);
                rcDelayCoroutine = StartCoroutine(RollerCoasterDelay(rcObject4));
                break;
            default:
                Debug.LogWarning("Invalid studio numbrr: " + studioNumber);
                break;
        }

        //remembers current studio numbrr
        currentStudio = studioNumber;

        //plays closing sound
        //StartCoroutine(PlayClosingSound(previousStudio));
    }

    //unlocks studio switching when window closes
    public void UnlockWindow(int studioNumber)
    {
        if (openStudioNumber == studioNumber)
        {
            windowIsOpen = false;
            openStudioNumber = -1;
        }
    }

    public void ActivateFirstOpenObject(int studioNumber)
    {
        //studios 1-4 into index array ß-3
        int index = studioNumber - 1;

        //checks if the studio number is valid
        if (index < 0 || index >= windowsOpened.Length)
        {
            Debug.LogWarning("Invalid studio number: " + studioNumber);
            return;
        }

        //only counts each window once
        if (!windowsOpened[index])
        {
            windowsOpened[index] = true;
            openedWindowCount++;

            //Debug.Log("Window " + studioNumber + " opened for the first time! Total: " + openedWindowCount);
            if (openedWindowCount >= 4 && !firstOpenObjectActivated && finalCracksCoroutine == null)
            {
                finalCracksCoroutine = StartCoroutine(ActivateFirstOpenObjectDelayed());
            }
        }

        //activates object only after all four windows have been opened
        //if (openedWindowCount == 4 && firstOpenObject != null)
        //{
        //    firstOpenObject.SetActive(true);
        //    Debug.Log("All windows have been opened wee");
        //}

        
    }

    private IEnumerator ActivateFirstOpenObjectDelayed()
    {
        yield return new WaitForSeconds(finalCracksDelay);

        if (firstOpenObject != null)
        {
            firstOpenObject.SetActive(true);
        }

        firstOpenObjectActivated = true;
        finalCracksCoroutine = null;

        Debug.Log("cracks show up wee");
    }

    private IEnumerator RollerCoasterDelay(GameObject targetRcObject) //to turn on the rollercoaster after delay so it starts movin
    {

        if (targetRcObject == null) yield break;
        
        //gotta turn it off at the start just in case
        targetRcObject.SetActive(false);

        yield return new WaitForSeconds(rcDelay);

        targetRcObject.SetActive(true);
        Debug.Log("rc goin after" + rcDelay + "secs");
        

    }

    //public void TryActivateFirstOpenObject()
    //{
    //    if (openedWindowCount >= 4 && !firstOpenObjectActivated)
    //    {
    //        firstOpenObject.SetActive(true);
    //        firstOpenObjectActivated = true;

    //        Debug.Log("All windows have been opened after glow finished wee");
    //    }
    //}





    public void ResetRollerCoaster()
    {
        StartCoroutine(ResetRollerCoasterCoroutine()); 
        
    }

    private IEnumerator ResetRollerCoasterCoroutine() //cause otherwise it immediately despawns after colliding with the cube again
    {
        yield return null; //waits for current frame to be done

        foreach (RollerCoasterTrigger coaster in allRollerCoasters)
        {
            if (coaster != null)
            {
                coaster.ResetThisRollerCoaster();
            }
        }
    }

    


    //reset TOIMEEE
    public void ResetEverything() //cept the window pos of course
    {
        Debug.Log("resetting everything");

        //studio switching reset
        windowIsOpen = false;
        openStudioNumber = -1;
        currentStudio = 1;

        //all studios turn off
        if (Studio1 != null) Studio1.SetActive(false);
        if (Studio2 != null) Studio2.SetActive(false);
        if (Studio3 != null) Studio3.SetActive(false);
        if (Studio4 != null) Studio4.SetActive(false);

        //stops active rollercoaster delays
        if (rcDelayCoroutine != null)
        {
            StopCoroutine(rcDelayCoroutine);
            rcDelayCoroutine = null;
        }

        //turns off all the rollercoasters in the windows so they wait for delay after reset
        if (rcObject1 != null) rcObject1.SetActive(false);
        if (rcObject2 != null) rcObject2.SetActive(false);
        if (rcObject3 != null) rcObject3.SetActive(false);
        if (rcObject4 != null) rcObject4.SetActive(false);


        //progress of opening windows reset
        windowsOpened = new bool[4];
        openedWindowCount = 0;

        if (finalCracksCoroutine != null)
        {
            StopCoroutine(finalCracksCoroutine);
            finalCracksCoroutine = null;
        }

        firstOpenObjectActivated = false;

        //first open object (CRACKS) tunred off
        if (firstOpenObject != null)
        {
            firstOpenObject.SetActive(false);
        }

        //RESET all windows
        foreach (DoorGazeBehaviour door in allWindows)
        {
            if (door != null)
            {
                door.ResetWindowState();
            }
        }

        //resets rollercoastzer
        ResetRollerCoaster();

        Debug.Log("Everythings Resettt");
    }

}
