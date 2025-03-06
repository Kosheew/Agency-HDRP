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
        // Видаляємо всі зв'язки, пов'язані з цим вузлом
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

        // Отримуємо шлях до файлу
        string assetPath = AssetDatabase.GetAssetPath(dialogue);

        if (!string.IsNullOrEmpty(assetPath) && AssetDatabase.LoadAssetAtPath<DialogueSettings>(assetPath) != null)
        {
            // Видаляємо діалог
            AssetDatabase.DeleteAsset(assetPath);
            Debug.Log($"Deleted dialogue: {assetPath}");
        
            // Оновлюємо базу даних Unity
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

        // Отримуємо шлях до файлу
        string assetPath = AssetDatabase.GetAssetPath(option);

        if (!string.IsNullOrEmpty(assetPath) && AssetDatabase.LoadAssetAtPath<DialogueOptionSettings>(assetPath) != null)
        {
            // Видаляємо опцію
            AssetDatabase.DeleteAsset(assetPath);
            Debug.Log($"Deleted option: {assetPath}");

            // Оновлюємо базу даних Unity
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning($"Option asset not found at: {assetPath}");
        }
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