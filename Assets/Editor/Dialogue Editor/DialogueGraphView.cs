using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor;

public class DialogueGraphView : GraphView
{
    public DialogueDatabase Database;
    
    public DialogueGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
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
}