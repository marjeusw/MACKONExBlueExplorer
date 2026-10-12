using Unity.VisualScripting;
using UnityEngine;

//This script must go onto the player HMD (ideally as a child)
public class RaycastGazeInteractor : MonoBehaviour
{
    LayerMask layerMask;
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
            
            if (hit.collider.CompareTag("Door"))   //tagdoor
            {
                Door1Channel.Invoke();
                Door4Channel.Invoke();
                Debug.Log("lookedDoor");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                
                //checked seconds function
                //do open door invoke thingy
                //also check which door function -> so we can call different stuff (like eg. an animation of the rollercoaster going into the specific door)
            }
            else if (hit.collider.CompareTag("Table")) //tagtable
            {
                Debug.Log("lookedTable");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                //checked seconds function
                //do table function
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("new Gazable object?"); //shouldn't exist but just in case
            }
            
        }
        else
        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            Debug.Log("Did not Hit"); 
        }
    }

    //function for different doors and what to do with which in new script
    //for later: Debug.Log("Gaze Cancelled"); //Gaze cancelled due to being less than 2 seconds

}
