using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

// This script goes onto every door with an Animator
public class DoorGazeBehaviour : MonoBehaviour
{
    private StudioLoader studioLoader; //refs the studioloaderscript

    private Animator[] animators;

    public bool isClosed = true;
    public float secondsUntilDoorClose = 0.0f;
    public GameObject lightParent; //where the script is on
    public GameObject light; //this one can be turned off and on just fine
    private bool lightsShutter = true;

    private bool hasEverOpened = false; //to check if a window was opened before for the final crack setup

    private static DoorGazeBehaviour currentlyOpenDoor; //variable shared by all instances of the scripts to keep track of the opened doors 


    private void Start()
    {

        DifferentAnimators(); //searches the animator on object or in children

        studioLoader = GetComponent<StudioLoader>();

        if (animators == null)
        {
            Debug.LogError("No Animator found on " + gameObject.name);
        }
        isClosed = true;
        secondsUntilDoorClose = 0.0f; //so that the audio won't be called at the very start (initializes window as closed when starting)
    }

    private void Update()
    {
        if (!isClosed && secondsUntilDoorClose > 0) //prevents countdown from triggering (close window) when timer is already zero
        {
            //check if another door was opened
            secondsUntilDoorClose -= Time.deltaTime;

            if (secondsUntilDoorClose <= 0)
            {
                Close();
            }
        }
    }

    

    void DifferentAnimators()
    {
        if (this.CompareTag("GrandDoor"))
        {
            animators = GetComponentsInChildren<Animator>(); //animators in children
        }
        else
        {
            Animator animator = GetComponent<Animator>(); //animator on object itself
            animators = animator != null ? new[] { animator } : System.Array.Empty<Animator>(); //used google ai for dis one

            //its basically this but shorter:
            //if (animator != null)
            //{
            //    animators = new[] { animator }; //array containing dis animator
            //}
            //else
            //{
            //    animators = System.Array.Empty<Animator>(); //empty animator array
            //}
        }
    }

    [ContextMenu("Open")]

    public void Open()
    {
        //to prevent the timer from resetting every time the windows looked at
        if (!isClosed)
        {
            return;
        }

        //don't open window if another window open
        if (currentlyOpenDoor != null && currentlyOpenDoor != this)
        {
            return; //closes the previously opened door (in case there is one)
        }

        currentlyOpenDoor = this;

        isClosed = false;
        secondsUntilDoorClose = 5.0f; //until when door closes

        if (!hasEverOpened) //happens only after window is allowed to open (so before its unlocked after the return functions above this function won't work)
        {
            hasEverOpened = true;
            studioLoader.OnWindowFirstOpened(); //handing it to studio loader since its prefab no work stuff again (gotta use the proxy so object manager can access)
        }

        if (lightsShutter && lightParent != null)
        {
            GlowScript glow = lightParent.GetComponent<GlowScript>();

            if (glow != null)
            {
                glow.StopAllCoroutines(); //stops glowscripts fadeloop and any fades it started
                StartCoroutine(FadeOutThenDisable(glow));
            }
            else
            {
                Debug.LogError("no GlowScript found on " + lightParent.name);
            }
        }
        else //for when the glow isn't there anymore
        {
            Debug.Log("No more Glow");
            foreach (Animator animator in animators) //makes it so that either 1 or multiple animators in children or on object itself get played
            {
                Assert.IsNotNull(animator);

                animator.SetBool("IsOpen", true);

                //Debug.Log("Door opens.");
            }
        }




    }

    private IEnumerator FadeOutThenDisable(GlowScript glow) //makes glow fade out after window is selected and then turns it off
    {
        yield return glow.FadeToAlpha(0f, 1f);
        light.SetActive(false);

        //activates crack object after glow disappears
        studioLoader.OnGlowFinished();

        foreach (Animator animator in animators) //makes it so that either 1 or multiple animators in children or on object itself get played
        {
            Assert.IsNotNull(animator);

            animator.SetBool("IsOpen", true);

            //Debug.Log("Door opens.");
        }
        lightsShutter = false; //gotta set the lights false before window opens the first time

    }



    [ContextMenu("Close")]
    public void Close()
    {
        if (isClosed)
        {
            return;
        }

        foreach (Animator animator in animators) //same here
        {
            Assert.IsNotNull(animator);

            animator.SetBool("IsOpen", false);

            //Debug.Log("Door closes.");
        }

        isClosed = true; //only needs to happen once

        if (currentlyOpenDoor == this)
        {
            currentlyOpenDoor = null; //forgets this door if its the currently open one
        }

        if (studioLoader != null)
        {
            studioLoader.OnDoorClosed(); //notifies StudioLoader
        }
    }

    public void ResetWindowState()
    {
        StopAllCoroutines(); //no delayed animations and stuffs

        //resets window logic
        isClosed = true;
        secondsUntilDoorClose = 0f;
        lightsShutter = true;
        hasEverOpened = false;

        currentlyOpenDoor = null; //even if door currently open while resetting, after pressing doesn't remember it

        //resets animator
        foreach (Animator animator in animators)
        {
            if (animator != null) //just a check so no errors when missing ref
            {
                animator.SetBool("IsOpen", false);
                animator.Play("new_ShuttersIdle", 0, 0f); //NEEDS NAME OF THE ANIMATION but snaps it directly to idle
                animator.Update(0f); //tells animator to evaluate immediately instead of waiting for next frame (hopefully fixes the window staying open after reset)
            }
        }

        //turns glow back on
        if (lightParent != null)
        {
            GlowScript glow = lightParent.GetComponent<GlowScript>();

            if (glow != null)
            {
                glow.ResetGlow();
                
            }

            light.SetActive(true);
        }

        //also gotta reset studioloader
        if (studioLoader != null)
        {
            studioLoader.ResetLoaderState();
        }
    }


}
