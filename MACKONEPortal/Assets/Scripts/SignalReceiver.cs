using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//This has to be the on door
public class SignalReceiver : MonoBehaviour
{
   [SerializeField] private SignalChannel _channel;
    
    public UnityEvent Response = new();

    private void OnEnable()
    {
        _channel.Subscribe(this); 
    }

    private void OnDisable()
    {
        _channel.Unsubscribe(this);
    }

    public void OnReceive()
    {
        Response.Invoke();
    }
}
