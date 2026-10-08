using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

// This script goes onto every door with an Animator
public class DoorGazeBehaviour : MonoBehaviour
{
    private Animator animator;

    public bool isClosed = true;
    public float secondsUntilDoorClose = 0.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No Animator found on " + gameObject.name);
        }

    }

    private void Update()
    {
        if (!isClosed)
        {
            secondsUntilDoorClose -= Time.deltaTime;

            if (secondsUntilDoorClose <= 0)
            {
                Close();
            }
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        Assert.IsNotNull(animator);

        isClosed = false;
        secondsUntilDoorClose = 2.0f;

        animator.SetBool("IsOpen", true);

        Debug.Log("Door opens.");
    }

    [ContextMenu("Close")]
    public void Close()
    {
        Assert.IsNotNull(animator);

        isClosed = true;

        animator.SetBool("IsOpen", false);

        Debug.Log("Door closes.");
    }
}
