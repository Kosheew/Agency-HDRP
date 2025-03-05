using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DialogueTreeEditor : EditorWindow
{
    private DialogueDatabase _currentDatabase;
    private VisualElement _rightPanel;
    
    public DialogueGraphView graphView;
    
    private const string DialogueFolder = "Assets/Dialogues";

    [MenuItem("Window/Dialogue Graph Window")]
    public static DialogueTreeEditor ShowWindow()
    {
        return GetWindow<DialogueTreeEditor>("Dialogue Editor");
    }
    
    public void LoadNewAsset(DialogueDatabase asset)
    {
        if (Application.isPlaying)
        {
            Debug.Log("Load new asset aborted. Will not open assets during play.");
            return;
        }
        _currentDatabase = null;
        
        RecreateUI();
        
        _currentDatabase = asset;
        Debug.Log($"Loading new asset: {_currentDatabase.name}");
        
        graphView.Database = _currentDatabase;
        
        if (_currentDatabase.Dialogues.Count <= 0)
        {
            string startGuid = Guid.NewGuid().ToString();
            var startedNode = CreateDialogueNode(_currentDatabase.StartDialogue, new Vector2(100, 100), startGuid);
            _currentDatabase.AddDialogues(_currentDatabase.StartDialogue, new Vector2(100, 100), startGuid);
            graphView.AddElement(startedNode);
        }
        else
        {
            foreach (var dialogue in _currentDatabase.Dialogues)
            {
                var node = CreateDialogueNode(dialogue.dialogue, dialogue.position, dialogue.nodeGUID);
                graphView.AddElement(node);
                Debug.Log($"Added node: {dialogue.dialogue.name}");
            }
            
            foreach (var option in _currentDatabase.Options)
            {
                var node = CreateOptionNode(option.dialogueOption, option.position, option.nodeGUID);
                graphView.AddElement(node);
            }
                    
            LoadConnections();
        }

        Repaint();
    }

    private void RecreateUI()
    {
        rootVisualElement.Clear();
        
        if (graphView != null)
        {
            graphView.DeleteElements(graphView.graphElements.ToList());
            graphView.Clear();
            graphView.Database = null;
        }
        
        var splitView = new TwoPaneSplitView(0, position.width * 0.8f, TwoPaneSplitViewOrientation.Horizontal);
        
       // splitView.style.flexGrow = 1; 
        
        _rightPanel = new VisualElement();
        
        _rightPanel.style.paddingTop = new StyleLength(10);
        _rightPanel.style.paddingRight = new StyleLength(15);
        _rightPanel.style.paddingBottom = new StyleLength(10);
        _rightPanel.style.paddingLeft = new StyleLength(15);
        _rightPanel.style.flexGrow = 1;
        
        _rightPanel.style.backgroundColor = new StyleColor(new Color(0.22f, 0.22f, 0.22f));
        _rightPanel.name = "right-panel";
        
        _rightPanel.style.minWidth = position.width * 0.2f;
        
        _rightPanel.style.flexShrink = 0;
        _rightPanel.style.flexGrow = 0;
        
        graphView = new DialogueGraphView
        {
            name = "Dialogue Tree Graph",
            style =
            {
                backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f)),
            }
        };
        
        graphView.RegisterCallback<MouseDownEvent>(OnNodeSelected, TrickleDown.TrickleDown);
        
        graphView.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        graphView.AddManipulator(new ContentDragger());
        
        splitView.Add(graphView);
        splitView.Add(_rightPanel);
        rootVisualElement.Add(splitView);
    }

    private DialogueNode CreateDialogueNode(DialogueSettings dialogue, Vector2 position, string guid)
    {
        var node = new DialogueNode(dialogue, graphView, guid)
        {
            title = dialogue.name,
            style = 
            {
                left = position.x,
                top = position.y,
                width = 300,
            },
        };
        
        node.titleContainer.style.backgroundColor = new StyleColor(new Color(0.3f, 1f, 0.8f, 0.2f));
        node.titleContainer.style.color = new StyleColor(Color.black);
        
        var optionNameField = new TextField("Option Name")
        {
            value = "New Option",
            style =
            {
                height = 20,
                marginTop = 5
            }
        };
        node.mainContainer.Add(optionNameField );
        
        var createOptionButton = new Button(() => CreateNewOption(node, optionNameField.value)) 
        {
            text = "Create Option",
            style =
            {
                width = 120,
                height = 30, 
                marginTop = 5,
                alignSelf = Align.Center 
            }
        };
        node.mainContainer.Add(createOptionButton);
        
        node.RefreshExpandedState();
        node.RefreshPorts();
        
        return node;
    }

    private DialogueOptionNode CreateOptionNode(DialogueOptionSettings dialogueOption, Vector2 position, string guid)
    {
        var node = new DialogueOptionNode(dialogueOption, graphView, guid)
        {
            title = dialogueOption.name,
            style =
            {
                left = position.x,
                top = position.y,
                width = 300,

            }
        };
        
        node.titleContainer.style.backgroundColor = new StyleColor(new Color(1f, 1f, 0f,0.2f));
        node.titleContainer.style.color = new StyleColor(Color.black);
        
        var dialogueNameField = new TextField("Dialogue Name")
        {
            value = "New Dialogue",
            style =
            {
                height = 20,
                marginTop = 5
            }
        };
        node.mainContainer.Add(dialogueNameField);

        var createOptionButton = new Button(() => CreateNewDialogue(node, dialogueNameField.value))
        {
            text = "Create Dialogue",
            style =
            {
                width = 120, 
                height = 30, 
                marginTop = 5, 
                alignSelf = Align.Center 
            }
        };
        
        node.mainContainer.Add(createOptionButton);
        
        node.RefreshExpandedState();
        node.RefreshPorts();
        
        return node;
    }
    
    private void CreateNewOption(DialogueNode dialogueNode, string optionName)
    {
        Debug.Log("Creating new Option");
        var newOption = AddNewDialogueOptionSettings(optionName);
        var option = CreateOptionNode(newOption, Event.current.mousePosition, Guid.NewGuid().ToString());
    
        Conected(dialogueNode.OutputPort, option.InputPort);
    
        dialogueNode.DialogueData.AddOptions(newOption);
        
        _currentDatabase.AddOptions(newOption, Event.current.mousePosition, option.GUID);
        
        graphView.AddElement(option);
    }

    private void CreateNewDialogue(DialogueOptionNode dialogueNode, string dialogueName)
    {
        Debug.Log("Creating new Dialogue");
        var newDialogue = AddNewDialogue(dialogueNode.DialogueOptionSettings, dialogueName);
        var dialogue = CreateDialogueNode(newDialogue, Event.current.mousePosition, Guid.NewGuid().ToString());
        
        Conected(dialogueNode.OutputPort, dialogue.InputPort);
        
        _currentDatabase.AddDialogues(newDialogue, Event.current.mousePosition, dialogue.GUID);
        
        graphView.AddElement(dialogue);
    }

    private void LoadConnected(Port portOutput, Port portInPort)
    {
        var edge = new Edge
        {
            output = portOutput,
            input = portInPort 
        };
        edge.AddToClassList("connector");
        graphView.AddElement(edge);
    }
    
    private void Conected(Port portOutput, Port portInPort)
    {
        LoadConnected(portOutput, portInPort);
        
        if (portOutput.node is IDialogueNode fromNode && portInPort.node is IDialogueNode toNode)
        {
            _currentDatabase.AddConnection(fromNode.GUID, toNode.GUID);
        }
    }
    
    private DialogueOptionSettings AddNewDialogueOptionSettings(string optionName)
    {
        if (!AssetDatabase.IsValidFolder(DialogueFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Dialogues");
        }
        
        var newOption = CreateInstance<DialogueOptionSettings>();
        newOption.name = "New Option";
        
        string optionPath = $"{DialogueFolder}/Option_{optionName}.asset";
        AssetDatabase.CreateAsset(newOption, optionPath);
        EditorUtility.SetDirty(newOption);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Repaint();
        SceneView.RepaintAll();
        
        return newOption;
    }
    
    private DialogueSettings AddNewDialogue(DialogueOptionSettings optionSettings, string dialogueName)
    {
        if (!AssetDatabase.IsValidFolder(DialogueFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Dialogues");
        }
        
        var newDialogue = CreateInstance<DialogueSettings>();
        newDialogue.name = "New Dialogue"; 
        optionSettings.SetNextDialogue(newDialogue);
        // newDialogue.Sentence = "Enter dialogue text";
        
        string dialoguePath = $"{DialogueFolder}/Dialogue_{dialogueName}.asset";
        AssetDatabase.CreateAsset(newDialogue, dialoguePath);
        EditorUtility.SetDirty(newDialogue);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Repaint();
        SceneView.RepaintAll();
        
        return newDialogue;
    }
    
    private void LoadConnections()
    {
        if (_currentDatabase == null || _currentDatabase.Connections == null)
        {
            Debug.LogWarning("No connections found in the database.");
            return;
        }

        // Створюємо копію колекції для ітерації
        var connectionsCopy = new List<Connection>(_currentDatabase.Connections);

        foreach (var connection in connectionsCopy)
        {
            Debug.Log($"Loading connection: {connection.fromNodeGUID} -> {connection.toNodeGUID}");

            var fromNode = graphView.nodes.ToList()
                .OfType<IDialogueNode>()
                .FirstOrDefault(n => n.GUID == connection.fromNodeGUID);

            var toNode = graphView.nodes.ToList()
                .OfType<IDialogueNode>()
                .FirstOrDefault(n => n.GUID == connection.toNodeGUID);

            if (fromNode != null && toNode != null)
            {
                Debug.Log($"Found nodes: {fromNode.GUID} -> {toNode.GUID}");

                if (fromNode.OutputPort != null && toNode.InputPort != null)
                {
                    LoadConnected(fromNode.OutputPort, toNode.InputPort);
                }
                else
                {
                    Debug.LogWarning($"Ports are not initialized for connection: {connection.fromNodeGUID} -> {connection.toNodeGUID}");
                }
            }
            else
            {
                Debug.LogWarning($"Failed to restore connection: {connection.fromNodeGUID} -> {connection.toNodeGUID}");
            }
        }
    }
    
    private void OnNodeSelected(MouseDownEvent evt)
    {
        Debug.Log("MouseDownEvent triggered");
        
        var target = evt.target;

        if (target != null)
        {
            Debug.Log($"Clicked on node: {target}");
            
            if (target is DialogueNode dialogueNode)
            {
                DisplayScriptableObject(dialogueNode.DialogueData);
            }
            else if (target is DialogueOptionNode optionNode)
            {
                DisplayScriptableObject(optionNode.DialogueOptionSettings);
            }
        }
    }
    
    private void DisplayScriptableObject(ScriptableObject so)
    {
        _rightPanel.Clear();
        if (so == null)
        {
            Debug.LogWarning("ScriptableObject is null.");
            return;
        }
        
        var editor = UnityEditor.Editor.CreateEditor(so);
        var imguiContainer = new IMGUIContainer(() =>
        {
            if (editor != null)
            {
                editor.OnInspectorGUI();
            }
        });
        
        _rightPanel.Add(imguiContainer);
    }
}
