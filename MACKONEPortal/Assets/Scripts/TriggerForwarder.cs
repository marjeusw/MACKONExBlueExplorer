//just so that I can turn off the object where rigidbody and collider are on through the parent and still get the childs trigger on entering 
using UnityEngine;

public class TriggerForwarder : MonoBehaviour
{
    private RollerCoasterTrigger parentTrigger;

    private void Awake()
    {
        parentTrigger = GetComponentInParent<RollerCoasterTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentTrigger != null)
        {
            parentTrigger.HandleChildTrigger(other);
        }
    }
}