using Unity.VisualScripting;
using UnityEngine;

//This script must go onto the player HMD (ideally as a child)
public class RaycastGazeInteractor : MonoBehaviour
{
    LayerMask layerMask;

    //for the 2 sec timer
    float gazeTime = 2f;
    float Timer = 0f; //for how long user has been looking
    SignalReceiver currentDoor; //what door they're looking at

    bool doorActivated = false; //to keep the door from opening after being called


    [SerializeField] private SignalChannel Door1Channel;
    [SerializeField] private SignalChannel Door4Channel;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Doors");
    }

     void FixedUpdate()
    {
        CheckRaycast();



    }

    public void CheckRaycast() //made it public so we can reference it easier (no idea if we need to tho)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))

        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Did Hit");

            //checks if hit object has signal receiver (for now if its a door since the table doesn't have one yet)
            SignalReceiver door = hit.collider.GetComponentInParent<SignalReceiver>();

            if (door != null)
            {

                if (door != currentDoor) //if they're still looking at current door
                {
                    currentDoor = door;

                    Timer = 0f;
                    doorActivated = false;
                }

                Timer += Time.deltaTime; //how long they've been gazing

                if (Timer >= gazeTime && !doorActivated)
                {
                    door.OnReceive(); //THIS door gets called
                    Debug.Log("lookedDoor");
                    doorActivated = true;
                }

            }
            else
            {
                //not looking at door
                ResetGaze();
            }
        
        }
        else
        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            Debug.Log("Did not Hit");

            ResetGaze(); //not looking at anything
        }
    }

    void ResetGaze()
    {
        //resets everything when lookin away
        currentDoor = null;
        Timer = 0f;
        doorActivated = false;
        Debug.Log("Gaze Cancelled"); //Gaze cancelled due to being less than 2 seconds
    }

    

}
