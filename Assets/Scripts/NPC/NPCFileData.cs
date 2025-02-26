using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCFile", menuName = "ScriptableObjects/NPC/NPCFile")]
public class NPCFileData : ScriptableObject
{
    [Title("Основні дані")]
    
    [BoxGroup("Основні дані")] 
    [LabelText("Унікальний ID"), SerializeField] private ushort uniqueID;

    [BoxGroup("Основні дані")] 
    [LabelText("Зображення персонажа"), PreviewField(Height = 50, Alignment = ObjectFieldAlignment.Left)] [SerializeField] private Sprite spritePerson;

    
    [BoxGroup("Основні дані")]
    [TextArea(3, 10)] 
    [LabelText("Опис")]
    [SerializeField]
    private string description;
    
    [Title("Підказки")]
    [BoxGroup("Основні дані"), ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = true)] 
    [SerializeField] 
    private CluesData[] hints;
    
    public Sprite SpritePerson => spritePerson;
    public string Description => description;
    public CluesData[] Hints => hints;
    public ushort UniqueID => uniqueID;
    
    public List<CluesData> GetKnownHints(CluesController cluesController)
    {
        var knownHints = new List<CluesData>();

        foreach (var hint in hints)
        {
            if (cluesController.HasClue(hint) && !cluesController.IsClueArchived(hint))
            {
                knownHints.Add(hint);
            }
        }

        return knownHints;
    }
}
