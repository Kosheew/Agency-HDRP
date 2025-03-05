using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "ScriptableObjects/Dialogue/Database")]
public class DialogueDatabase : ScriptableObject
{
    [Button("Open Dialogue Editor", ButtonSizes.Large)]
    private void OpenDialogueEditor()
    {
        DialogueTreeEditor window = DialogueTreeEditor.ShowWindow();
        window.LoadNewAsset(this);
    }
    
    [SerializeField] private DialogueSettings startDialogue; 
    private List<DialoguePosition> dialogues = new List<DialoguePosition>();
    private List<DialogueOptionPosition> options = new List<DialogueOptionPosition>();
    private List<Connection> connections = new List<Connection>();
    
    public DialogueSettings StartDialogue => startDialogue;
    public List<DialoguePosition> Dialogues => dialogues;
    public List<DialogueOptionPosition> Options => options;
    public List<Connection> Connections => connections;
    
    public void AddDialogues(DialogueSettings dialogue, Vector2 position, string guid)
    {
        var existingDialogue = dialogues.Find(d => d.dialogue == dialogue);
        if (existingDialogue == null)
        {
            dialogues.Add(new DialoguePosition { dialogue = dialogue, position = position, nodeGUID = guid});
        }
        else
        {
            existingDialogue.position = position; // Оновлюємо позицію
        }
    }

    public void AddOptions(DialogueOptionSettings option, Vector2 position, string guid)
    {
        var existingOption = options.Find(o => o.dialogueOption == option);
        if (existingOption == null)
        {
            options.Add(new DialogueOptionPosition { dialogueOption = option, position = position, nodeGUID = guid });
        }
        else
        {
            existingOption.position = position; // Оновлюємо позицію
        }
    }
    
    public void AddConnection(string fromNodeGUID, string  toNodeGUID)
    {
        connections.Add(new Connection
        {
            fromNodeGUID = fromNodeGUID,
            toNodeGUID = toNodeGUID
        });
    }
}

[System.Serializable]
public class DialoguePosition
{
    public DialogueSettings dialogue;
    public Vector2 position;
    public string nodeGUID;
}

[System.Serializable]
public class DialogueOptionPosition
{
    public DialogueOptionSettings dialogueOption;
    public Vector2 position;
    public string nodeGUID;
}

[System.Serializable]
public class Connection
{
    public string fromNodeGUID; // GUID вузла, звідки йде зв’язок
    public string toNodeGUID;   // GUID вузла, куди веде зв’язок
}