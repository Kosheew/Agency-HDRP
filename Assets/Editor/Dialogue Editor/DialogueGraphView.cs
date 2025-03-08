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
        var elementsToRemove = selection
            .OfType<GraphElement>()  
            .Where(e => e is DialogueNode || e is DialogueOptionNode)  
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
        
        base.DeleteSelection();
    }
}