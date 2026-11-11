using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

// This script goes onto every door with an Animator
public class DoorGazeBehaviour : MonoBehaviour
{
    private StudioLoader studioLoader; //refs the studioloaderscript

    private Animator[] animators;

    public bool isClosed = true;
    public float secondsUntilDoorClose = 0.0f;

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
        //don't open window if another window open
        if (currentlyOpenDoor != null && currentlyOpenDoor != this)
        {
            return; //closes the previously opened door (in case there is one)
        }

        currentlyOpenDoor = this;

        isClosed = false;
        secondsUntilDoorClose = 5.0f; //until when door closes

        foreach (Animator animator in animators) //makes it so that either 1 or multiple animators in children or on object itself get played
        {
            Assert.IsNotNull(animator);

            animator.SetBool("IsOpen", true);

            //Debug.Log("Door opens.");
        }

      
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
}
