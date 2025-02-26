using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Hint", menuName = "ScriptableObjects/NPC/Hint")]
public class CluesData : ScriptableObject
{
    [Title("Основні дані")]
        
    [BoxGroup("Основні дані")]
    [LabelText("Унікальний ID"), SerializeField] 
    private ushort uniqueID;
    
    [BoxGroup("Основні дані")]
    [LabelText("Назва зачіпки"), SerializeField]
    private string hintName;
    
    [BoxGroup("Основні дані")]
    [LabelText("Опис зачіпки"), SerializeField, TextArea(3, 10)]
    private string description;
    
    public ushort UniqueID => uniqueID;
    public string HintName => hintName;
    public string Description => description;
}
