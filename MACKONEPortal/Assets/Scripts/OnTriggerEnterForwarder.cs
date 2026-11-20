using UnityEngine;

public class OnTriggerEnterForwarder : MonoBehaviour
{
    public GameObject Researcher;

     void OnTriggerEnter(Collider other)
    {
        if(other.name == "pathcube1")
        {
            Debug.Log("Hit pathcube 1");
            Researcher.GetComponent<ResearcherBehaviour>().IdleBehaviour();
        }
        else if(other.name == "pathcube2")
        {
            Debug.Log("Hit pathcube 2");
            Researcher.GetComponent<ResearcherBehaviour>().TeleportBehaviour();
        }
        else if(other.name == "pathcube3")
        {
            Debug.Log("Hit pathcube 3");
            Researcher.GetComponent<ResearcherBehaviour>().WalkBehaviour();
        }
    }
}
