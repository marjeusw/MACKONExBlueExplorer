using UnityEngine;

//This script must go onto the player HMD (ideally as a child)
public class RaycastGazeInteractor : MonoBehaviour
{
    LayerMask layerMask;
    [SerializeField] private SignalChannel Door1Channel;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Doors");
    }

     void FixedUpdate()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))

        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red); 
            Debug.Log("Did Hit"); 
            Door1Channel.Invoke();
        }
        else
        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            Debug.Log("Did not Hit"); 
        }

    }
}
