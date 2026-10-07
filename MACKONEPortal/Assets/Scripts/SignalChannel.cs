using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSignalChannel", menuName = "Signal Channel", order = 999)]
public class SignalChannel : ScriptableObject
{
    private List<SignalReceiver> _receivers = new();
    public void Invoke()
    {
        foreach (SignalReceiver entry in _receivers)
        {
            entry.OnReceive();
        }
    }

    public void Subscribe(SignalReceiver receiver)
    {
        _receivers.Add(receiver);
    }

    public void Unsubscribe(SignalReceiver receiver)
    {
        _receivers.Remove(receiver);
    }
}
