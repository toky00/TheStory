using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Interact/Interaction")]
public class InteractData : ScriptableObject
{
    public string DisplayText => displayText;
    
    [SerializeField]
    string displayText;
}
