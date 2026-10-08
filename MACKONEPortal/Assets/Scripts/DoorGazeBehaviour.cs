using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

// This script goes onto every door with a HingeJoint.
public class DoorGazeBehaviour : MonoBehaviour
{
    private HingeJoint hinge;

    public bool isClosed = true;
    public float secondsUntilDoorClose = 0.0f;

    private void Start()
    {
        hinge = GetComponent<HingeJoint>();

        if (hinge == null)
        {
            Debug.LogError("No HingeJoint found on " + gameObject.name);
        }
        else
        {
            // Initialize Spring values
            JointSpring spring = hinge.spring;
            spring.spring = 10f;
            spring.damper = 3f;
            hinge.spring = spring;
            hinge.useSpring = true;
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
        Assert.IsNotNull(hinge);
        
        isClosed = false;
        secondsUntilDoorClose = 2.0f;

        JointSpring spring = hinge.spring;
        spring.targetPosition = 120f;

        Debug.Log("Door opens. Current angle: " + hinge.angle);
    }

    [ContextMenu("Close")]
    public void Close()
    {
        Assert.IsNotNull(hinge);

        isClosed = true;

        JointSpring spring = hinge.spring;
        spring.targetPosition = 0f;

        Debug.Log("Door closes.");
    }
}
