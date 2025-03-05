using UnityEditor.Experimental.GraphView;

public interface IDialogueNode
{
    public string GUID { get; }
    public Port InputPort { get; }
    public Port OutputPort { get; }
}
