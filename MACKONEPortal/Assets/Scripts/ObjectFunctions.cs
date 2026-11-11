//handles all the functions that happen when each door is used
//mostly which studio should currently be visible

using UnityEngine;
using System.Collections;
public class ObjectFunctions : MonoBehaviour
{
    //[SerializeField] private float closingSoundDelay = 0.5f;
    private bool windowIsOpen = false;
    private int openStudioNumber = -1;

    private int currentStudio = 1;

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
    public AudioSource audio4;

    public GameObject Studio1;
    public GameObject Studio2;
    public GameObject Studio3;
    public GameObject Studio4;



    //stores refs to all 4 studios and their audio (since after being instantiated, prefabs forgor all references that aren't in the prefab so studios gotta be reffed like dis)
    public void RegisterStudio(int studioNumber, GameObject studio, AudioSource windowAudio)
    {
        switch (studioNumber)
        {
            case 1:
                Studio1 = studio;
                audio1 = windowAudio;
                break;

            case 2:
                Studio2 = studio;
                audio2 = windowAudio;
                break;

            case 3:
                Studio3 = studio;
                audio3 = windowAudio;
                break;

            case 4:
                Studio4 = studio;
                audio4 = windowAudio;
                break;

            default:
                Debug.LogWarning("Invalid studio number: " + studioNumber);
                break;
        }

    }


    //after receiving studio numbr, decides on the following studio and also gets the audio source from studio loader
    //public void ShowStudio(int studioNumber, AudioSource windowAudio)
    //{
    //    //turns off all studios first
    //    if (Studio1 != null) Studio1.SetActive(false);
    //    if (Studio2 != null) Studio2.SetActive(false);
    //    if (Studio3 != null) Studio3.SetActive(false);
    //    if (Studio4 != null) Studio4.SetActive(false);

    //    //plays audio from audio source
    //    if (windowAudio != null)
    //    {
    //        windowAudio.Play();
    //    }

    //    //turns on selected studio
    //    switch (studioNumber)
    //    {
    //        case 1:
    //            if (Studio1 != null) Studio1.SetActive(true);

    //            break;
    //        case 2:
    //            if (Studio2 != null) Studio2.SetActive(true);

    //            break;
    //        case 3:
    //            if (Studio3 != null) Studio3.SetActive(true);

    //            break;
    //        case 4:
    //            if (Studio4 != null) Studio4.SetActive(true);

    //            break;
    //        default:
    //            Debug.LogWarning("Invalid studio numbrr: " + studioNumber);
    //            break;
    //    }
    //}


    public void ShowStudio(int studioNumber, AudioSource windowAudio)
    {
        //no other studio opens while other window is still open
        if (windowIsOpen && openStudioNumber != studioNumber)
        {
            return;
        }

        //locks studio switching until window closes
        windowIsOpen = true;
        openStudioNumber = studioNumber;

        //remembers which studio was last
        int previousStudio = currentStudio;

        //plays audio from audio source
        //if (windowAudio != null)
        //{
        //    windowAudio.Play();
        //}
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

                break;
            case 2:
                if (Studio2 != null) Studio2.SetActive(true);

                break;
            case 3:
                if (Studio3 != null) Studio3.SetActive(true);

                break;
            case 4:
                if (Studio4 != null) Studio4.SetActive(true);

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

    //edit in case we want coroutined delay (didn't look gud tho so I scratched it for now (overlapping environments))
    //Delays the studio disappearing

    //not needed anymore since thats done with the animation ig
    //private IEnumerator PlayClosingSound(int previousStudio)
    //{
    //    yield return new WaitForSeconds(closingSoundDelay);

    //    //turns on selected studio
    //    switch (previousStudio)
    //    {
    //        case 1:
    //            if (audio1 != null) audio1.Play(); ;

    //            break;
    //        case 2:
    //            if (audio2 != null) audio2.Play(); ;

    //            break;
    //        case 3:
    //            if (audio3 != null) audio3.Play(); ;

    //            break;
    //        case 4:
    //            if (audio4 != null) audio4.Play();

    //            break;
    //    }


    //}


}
