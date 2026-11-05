//handles all the functions that happen when each door is used
//mostly which studio should currently be visible

using UnityEngine;
public class ObjectFunctions : MonoBehaviour
{
   

    public GameObject Studio1;
    public GameObject Studio2;
    public GameObject Studio3;
    public GameObject Studio4;



    //stores refs to all 4 studios (since after being instantiated, prefabs forgor all references that aren't in the prefab so studios gotta be reffed like dis)
    public void RegisterStudio(int studioNumber, GameObject studio)
    {
        switch (studioNumber)
        {
            case 1:
                Studio1 = studio;
                break;

            case 2:
                Studio2 = studio;
                break;

            case 3:
                Studio3 = studio;
                break;

            case 4:
                Studio4 = studio;
                break;

            default:
                Debug.LogWarning("Invalid studio number: " + studioNumber);
                break;
        }

    }


    //after receiving studio numbr, decides on the following studio and also gets the audio source from studio loader
    public void ShowStudio(int studioNumber, AudioSource windowAudio)
    {
        //turns off all studios first
        if (Studio1 != null) Studio1.SetActive(false);
        if (Studio2 != null) Studio2.SetActive(false);
        if (Studio3 != null) Studio3.SetActive(false);
        if (Studio4 != null) Studio4.SetActive(false);

        //plays audio from audio source
        if (windowAudio != null)
        {
            windowAudio.Play();
        }

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
    }


    //edit in case we want coroutined delay (didn't look gud tho so I scratched it for now (overlapping environments))
    //Delays the studio disappearing
    //private IEnumerator SwitchStudioAfterDelay(int studioNumber)
    //{
    //    //remembers which studio was active before switching
    //    int previousStudio = currentStudio;

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
    //            yield break;
    //    }

    //    yield return new WaitForSeconds(environmentDelay);

    //    //only deactivates the previous studio 
    //    switch (previousStudio)
    //    {
    //        case 1:
    //            if (Studio1 != null) Studio1.SetActive(false);
    //            break;

    //        case 2:
    //            if (Studio2 != null) Studio2.SetActive(false);
    //            break;

    //        case 3:
    //            if (Studio3 != null) Studio3.SetActive(false);
    //            break;

    //        case 4:
    //            if (Studio4 != null) Studio4.SetActive(false);
    //            break;
    //    }

    //    currentStudio = studioNumber;


    //}


}
