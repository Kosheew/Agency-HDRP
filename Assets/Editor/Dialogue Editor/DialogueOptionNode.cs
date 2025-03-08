using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

public class DialogueOptionNode : Node, IDialogueNode
{
    public string GUID { get; }
    public Port InputPort { get; }
    public Port OutputPort { get; }
    
    public DialogueOptionSettings DialogueOptionSettings;
    private DialogueGraphView graphView;
    
    public DialogueOptionNode(DialogueOptionSettings data, DialogueGraphView graphView, string guid)
    {
        GUID = guid;
        
        if (GUID == null)
        {
            GUID = Guid.NewGuid().ToString();
        }
        
        DialogueOptionSettings = data;
        
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(object));
        InputPort.portName = "Input";
        inputContainer.Add(InputPort);

        OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(object));
        OutputPort.portName = "Output"; 
        outputContainer.Add(OutputPort);
        
        TextField nameField = new TextField("Name:");
        nameField.value = DialogueOptionSettings.name;
        nameField.RegisterCallback<FocusOutEvent>(evt =>
        {
            RenameDialogueAsset(nameField.value);
        });
        mainContainer.Add(nameField);
        
        RegisterCallback<GeometryChangedEvent>(evt => graphView.OnNodeOptionMoved(this));
        
        RefreshExpandedState();
        RefreshPorts();
    }
    
    private void RenameDialogueAsset(string newName)
    {
        if (DialogueOptionSettings == null) return;
        
        string assetPath = AssetDatabase.GetAssetPath(DialogueOptionSettings);
        if (!string.IsNullOrEmpty(assetPath))
        {
            string error = AssetDatabase.RenameAsset(assetPath, newName);
            if (string.IsNullOrEmpty(error))
            {
                DialogueOptionSettings.name = newName;
                title = newName;
                EditorUtility.SetDirty(DialogueOptionSettings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
