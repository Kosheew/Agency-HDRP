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
    private DialogueGraphView _graphView;
    
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
        
        _graphView.Database = _currentDatabase;
        
        if (_currentDatabase.StartDialogue == null)
        {
            var startDialogue = AddNewDialogue(null);
            _currentDatabase.SetStartDialogue(startDialogue);
            string startGuid = Guid.NewGuid().ToString();
            var startedNode = CreateDialogueNode(startDialogue, new Vector2(100, 100), startGuid);
            _currentDatabase.AddDialogues(startDialogue, new Vector2(100, 100), startGuid);
            _graphView.AddElement(startedNode);

        }
        else
        {
            foreach (var dialogue in _currentDatabase.Dialogues)
            {
                var node = CreateDialogueNode(dialogue.dialogue, dialogue.position, dialogue.nodeGUID);
                _graphView.AddElement(node);
                Debug.Log($"Added node: {dialogue.dialogue.name}");
            }
            
            foreach (var option in _currentDatabase.Options)
            {
                var node = CreateOptionNode(option.dialogueOption, option.position, option.nodeGUID);
                _graphView.AddElement(node);
            }
                    
            LoadConnections();
        }

        Repaint();
    }

    private void RecreateUI()
    {
        rootVisualElement.Clear();
        
        if (_graphView != null)
        {
            _graphView.DeleteElements(_graphView.graphElements.ToList());
            _graphView.Clear();
            _graphView.Database = null;
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
        
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Tree Graph",
            style =
            {
                backgroundColor = new StyleColor(new Color(0.25f, 0.25f, 0.25f)),
            }
        };
        
        _graphView.RegisterCallback<MouseDownEvent>(OnNodeSelected, TrickleDown.TrickleDown);
        
        _graphView.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        _graphView.AddManipulator(new ContentDragger());
        
        splitView.Add(_graphView);
        splitView.Add(_rightPanel);
        rootVisualElement.Add(splitView);
    }

    private T CreateNode<T>(ScriptableObject data, Vector2 position, string guid, Color titleColor, string buttonText, Action<T> buttonAction) where T : Node
    {
        var node = (T)Activator.CreateInstance(typeof(T), data, _graphView, guid);
    
        node.title = data.name;
        node.style.left = position.x;
        node.style.top = position.y;
        node.style.width = 300;

        node.titleContainer.style.backgroundColor = new StyleColor(titleColor);
        node.titleContainer.style.color = new StyleColor(Color.black);

        var button = new Button(() => buttonAction(node))
        {
            text = buttonText,
            style =
            {
                width = 120,
                height = 30,
                marginTop = 5,
                alignSelf = Align.Center
            }
        };

        node.mainContainer.Add(button);
        node.RefreshExpandedState();
        node.RefreshPorts();

        return node;
    }

    private DialogueNode CreateDialogueNode(DialogueSettings dialogue, Vector2 position, string guid)
    {
        return CreateNode<DialogueNode>(
            dialogue, 
            position, 
            guid, 
            new Color(0.3f, 1f, 0.8f, 0.2f), 
            "Create Option", 
            node => CreateNewOption((DialogueNode)node)
        );
    }

    private DialogueOptionNode CreateOptionNode(DialogueOptionSettings dialogueOption, Vector2 position, string guid)
    {
        return CreateNode<DialogueOptionNode>(
            dialogueOption, 
            position, 
            guid, 
            new Color(1f, 1f, 0f, 0.2f), 
            "Create Dialogue", 
            node => CreateNewDialogue((DialogueOptionNode)node)
        );
    }
    
    private void CreateNewOption(DialogueNode dialogueNode)
    {
        Debug.Log("Creating new Option");
        var newOption = AddNewDialogueOptionSettings();
        var option = CreateOptionNode(newOption, Event.current.mousePosition, Guid.NewGuid().ToString());
    
        Conected(dialogueNode.OutputPort, option.InputPort);
    
        dialogueNode.DialogueData.AddOptions(newOption);
        
        _currentDatabase.AddOptions(newOption, Event.current.mousePosition, option.GUID);
        
        _graphView.AddElement(option);
    }

    private void CreateNewDialogue(DialogueOptionNode dialogueNode)
    {
        if (dialogueNode.DialogueOptionSettings.NextDialogue != null)
        {
            Debug.LogWarning("Dialogue Settings already exists");
            return;
        }
        
        Debug.Log("Creating new Dialogue");
        var newDialogue = AddNewDialogue(dialogueNode.DialogueOptionSettings);
        var dialogue = CreateDialogueNode(newDialogue, Event.current.mousePosition, Guid.NewGuid().ToString());
        
        Conected(dialogueNode.OutputPort, dialogue.InputPort);
        
        _currentDatabase.AddDialogues(newDialogue, Event.current.mousePosition, dialogue.GUID);
        
        _graphView.AddElement(dialogue);
    }

    private void LoadConnected(Port portOutput, Port portInPort)
    {
        var edge = new Edge
        {
            output = portOutput,
            input = portInPort 
        };
        edge.AddToClassList("connector");
        _graphView.AddElement(edge);
    }
    
    private void Conected(Port portOutput, Port portInPort)
    {
        LoadConnected(portOutput, portInPort);
        
        if (portOutput.node is IDialogueNode fromNode && portInPort.node is IDialogueNode toNode)
        {
            _currentDatabase.AddConnection(fromNode.GUID, toNode.GUID);
        }
    }
    
    private T CreateAsset<T>(string prefix) where T : ScriptableObject
    {
        if (_currentDatabase == null)
        {
            Debug.LogError("Помилка: База даних не задана.");
            return null;
        }

        string databasePath = AssetDatabase.GetAssetPath(_currentDatabase);
        string directoryPath = System.IO.Path.GetDirectoryName(databasePath);

        if (string.IsNullOrEmpty(directoryPath) || !AssetDatabase.IsValidFolder(directoryPath))
        {
            Debug.LogError("Помилка: Не вдалося знайти директорію бази даних.");
            return null;
        }

        var newAsset = CreateInstance<T>();
        newAsset.name = $"New {typeof(T).Name}";

        string assetPath = $"{directoryPath}/{prefix}_{Guid.NewGuid()}.asset";
        AssetDatabase.CreateAsset(newAsset, assetPath);
        EditorUtility.SetDirty(newAsset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Repaint();
        SceneView.RepaintAll();

        return newAsset;
    }

    private DialogueOptionSettings AddNewDialogueOptionSettings()
    {
        return CreateAsset<DialogueOptionSettings>("Option");
    }

    private DialogueSettings AddNewDialogue(DialogueOptionSettings optionSettings)
    {
        var newDialogue = CreateAsset<DialogueSettings>("Dialogue");
        if (newDialogue != null && optionSettings != null)
        {
            optionSettings.SetNextDialogue(newDialogue);
        }
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

            var fromNode = _graphView.nodes.ToList()
                .OfType<IDialogueNode>()
                .FirstOrDefault(n => n.GUID == connection.fromNodeGUID);

            var toNode = _graphView.nodes.ToList()
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
        
        var titleLabel = new Label(so.name)
        {
            style =
            {
                unityFontStyleAndWeight = FontStyle.Bold,
                fontSize = 14,
                marginBottom = 5
            }
        };
        _rightPanel.Add(titleLabel);
        
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