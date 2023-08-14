using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public InteractData Properties => interactData;
    
    [SerializeField]
    InteractData interactData;

    [SerializeField]
    UnityEvent activationEvent;

    public void Activate()
        => activationEvent?.Invoke();
}
