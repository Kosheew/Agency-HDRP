using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "ScriptableObjects/Dialogue/Database")]
public class DialogueDatabase : ScriptableObject
{
    [Button("Open Dialogue Editor", ButtonSizes.Large)]
    private void OpenDialogueEditor()
    {
        DialogueTreeEditor window = DialogueTreeEditor.ShowWindow();
        window.LoadNewAsset(this);
    }

    [SerializeField, FoldoutGroup("Dialogue Data"), HideIf("@!showData")] 
    private DialogueSettings startDialogue;
    
    [SerializeField, FoldoutGroup("Dialogue Data"), HideIf("@!showData")]
    private List<DialoguePosition> dialogues;
    
    [SerializeField, FoldoutGroup("Dialogue Data"), HideIf("@!showData")]
    private List<DialogueOptionPosition> options;
    
    [SerializeField, FoldoutGroup("Dialogue Data"), HideIf("@!showData")]
    private List<Connection> connections;
    
    private bool showData = true;

    [Button("Toggle Data Visibility", ButtonSizes.Large)]
    private void ToggleDataVisibility()
    {
        showData = !showData;
    }
    
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
    
    public void RemoveDialogue(string guid)
    {
        var dialogueToRemove = dialogues.FirstOrDefault(d => d.nodeGUID == guid);
        if (dialogueToRemove != null)
        {
            dialogues.Remove(dialogueToRemove);
            DeleteDialogue(dialogueToRemove.dialogue);
            Debug.Log($"Removed dialogue with GUID: {guid}");
        }
        else
        {
            Debug.LogWarning($"Dialogue with GUID: {guid} not found.");
        }
    }

    public void RemoveOption(string guid)
    {
        var optionToRemove = options.FirstOrDefault(o => o.nodeGUID == guid);
        if (optionToRemove != null)
        {
            options.Remove(optionToRemove);
            DeleteOption(optionToRemove.dialogueOption);
            Debug.Log($"Removed option with GUID: {guid}");
        }
        else
        {
            Debug.LogWarning($"Option with GUID: {guid} not found.");
        }
    }
    
    public void RemoveConnections(string nodeGUID)
    {
        Connections.RemoveAll(c => c.fromNodeGUID == nodeGUID || c.toNodeGUID == nodeGUID);
        Debug.Log($"Removed connections for node with GUID: {nodeGUID}");
    }
    
    private void DeleteDialogue(DialogueSettings dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogWarning("Dialogue is null, cannot delete.");
            return;
        }
        
        string assetPath = AssetDatabase.GetAssetPath(dialogue);

        if (!string.IsNullOrEmpty(assetPath) && AssetDatabase.LoadAssetAtPath<DialogueSettings>(assetPath) != null)
        {
            AssetDatabase.DeleteAsset(assetPath);
            Debug.Log($"Deleted dialogue: {assetPath}");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning($"Dialogue asset not found at: {assetPath}");
        }
    }

    private void DeleteOption(DialogueOptionSettings option)
    {
        if (option == null)
        {
            Debug.LogWarning("Option is null, cannot delete.");
            return;
        }
        
        string assetPath = AssetDatabase.GetAssetPath(option);

        if (!string.IsNullOrEmpty(assetPath) && AssetDatabase.LoadAssetAtPath<DialogueOptionSettings>(assetPath) != null)
        {
            AssetDatabase.DeleteAsset(assetPath);
            Debug.Log($"Deleted option: {assetPath}");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning($"Option asset not found at: {assetPath}");
        }
    }

    public void SetStartDialogue(DialogueSettings dialogue)
    {
        startDialogue = dialogue;
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
    public string fromNodeGUID; 
    public string toNodeGUID;   
}