using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogueNode : Node, IDialogueNode
{
    public string GUID { get; set; }
    public string DialogueText;
    public Port InputPort { get; }
    public Port OutputPort { get; }
    
    public DialogueSettings DialogueData;
    private DialogueGraphView graphView;
    
    public DialogueNode(DialogueSettings data, DialogueGraphView graphView, string guid)
    {
        GUID = guid;
        DialogueData = data;
        title = "Діалоговий вузол";
        
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(object));
        InputPort.portName = "Input";
        inputContainer.Add(InputPort);

        OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(object));
        OutputPort.portName = "Output"; 
        outputContainer.Add(OutputPort);
        
        TextField nameField = new TextField("Name:");
        nameField.value = DialogueData.name;
        nameField.RegisterCallback<FocusOutEvent>(evt =>
        {
            RenameDialogueAsset(nameField.value);
        });
        mainContainer.Add(nameField);
        
        RegisterCallback<GeometryChangedEvent>(evt => graphView.OnNodeDialogueMoved(this));
        
        RefreshExpandedState();
        RefreshPorts();
        
    }

    private void RenameDialogueAsset(string newName)
    {
        if (DialogueData == null) return;
        
        string assetPath = AssetDatabase.GetAssetPath(DialogueData);
        if (!string.IsNullOrEmpty(assetPath))
        {
            string error = AssetDatabase.RenameAsset(assetPath, newName);
            if (string.IsNullOrEmpty(error))
            {
                DialogueData.name = newName;
                title = newName;
                EditorUtility.SetDirty(DialogueData);
                AssetDatabase.SaveAssets();
            }
        }
    }
}