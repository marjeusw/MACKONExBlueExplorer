using UnityEngine;
using System.Collections;

public class DoorGazeBehaviour : MonoBehaviour
{
    public GameObject door;
    HingeJoint hinge;
    public bool isClosed;
    public float secondsUntilDoorClose = 2.0f;
    
    private void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    private void Update()
    {
        if (secondsUntilDoorClose > 0)
        {
            secondsUntilDoorClose -= Time.deltaTime;
        }

        if (isClosed == false && secondsUntilDoorClose <= 0)
        {
            Close();
        }
    }

    public void Open()
    {
        isClosed = false;
        secondsUntilDoorClose = 2.0f;
        JointSpring hingeSpring = hinge.spring;
        hingeSpring.spring = 10;
        hingeSpring.damper = 3;
        hingeSpring.targetPosition = 45;
        hinge.spring = hingeSpring;
        hinge.useSpring = true;
        Debug.Log("Door is open.");
    }
    
    public void Close()
    {
        isClosed = true;
        JointSpring hingeSpring = hinge.spring;
        hingeSpring.spring = 10;
        hingeSpring.damper = 3;
        hingeSpring.targetPosition = 0;
        hinge.spring = hingeSpring;
        hinge.useSpring = true;
        Debug.Log("Door is closed.");
    }
}
