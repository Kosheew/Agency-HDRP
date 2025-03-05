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
        
        RegisterCallback<GeometryChangedEvent>(evt => graphView.OnNodeDialogueMoved(this));
        
        RefreshExpandedState();
        RefreshPorts();
        
    }


}