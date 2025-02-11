using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueSettings")]
public class DialogueSettings : ScriptableObject
{
    [SerializeField] private Sprite personPortrait;
    [SerializeField] private string personName;
    [SerializeField] private ushort uniqueID;
    [SerializeField, TextArea(3,5)] private string sentence;
    [SerializeField] private DialogueOptionSettings[] options;
    
    
}
