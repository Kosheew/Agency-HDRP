using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogueGraphView : GraphView
{
    public DialogueDatabase Database;
    
    public DialogueGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.RegisterCallback<KeyDownEvent>(OnKeyDown);
    }

    public void OnNodeDialogueMoved(DialogueNode node)
    {
        if (node.DialogueData != null)
        {
            Database.AddDialogues(node.DialogueData, node.GetPosition().position, node.GUID);
        }
    }

    public void OnNodeOptionMoved(DialogueOptionNode node)
    {
        if (node.DialogueOptionSettings != null)
        {
            Database.AddOptions(node.DialogueOptionSettings, node.GetPosition().position, node.GUID);
        }
    }

    public void OnNodeSelected(DialogueNode node)
    {
        
    }
    
    private void OnKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Delete || evt.keyCode == KeyCode.Backspace)
        {
            DeleteSelectionNode();
            DeleteSelection();
        }
    }
    
    public void DeleteSelectionNode()
    {
        Debug.Log("Delete Selection");

        var elementsToRemove = selection
            .OfType<GraphElement>()  // Переконуємося, що це GraphElement
            .Where(e => e is DialogueNode || e is DialogueOptionNode)  // Відбираємо тільки потрібні вузли
            .ToList();

        foreach (var element in elementsToRemove)
        {
            if (element is DialogueNode dialogueNode)
            {
                Debug.Log($"Deleting dialogue node: {dialogueNode.GUID}");
                Database?.RemoveDialogue(dialogueNode.GUID);
                Database?.RemoveConnections(dialogueNode.GUID);
                
            }
            else if (element is DialogueOptionNode optionNode)
            {
                Debug.Log($"Deleting option node: {optionNode.GUID}");
                Database?.RemoveOption(optionNode.GUID);
                Database?.RemoveConnections(optionNode.GUID);
            }
        }

        // Видаляємо елементи з GraphView
        base.DeleteSelection();
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


}