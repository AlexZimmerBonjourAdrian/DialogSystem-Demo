using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEditor;


public class CDialogueGraphView : GraphView
{

    public Blackboard blackboard;
    public readonly Vector2 DafaultNodeSize = new Vector2(x: 150, y: 200);
    public Blackboard Blackboard;
    public List<CExposedProperty> ExposedPropierties = new List<CExposedProperty>();
    private CNodeSearchWindows _searchWindow;
    // Start is called before the first frame update
    public CDialogueGraphView(EditorWindow editorWindow)
    {


        styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "DialogueGraph"));
        //Zoom 
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        //Set configuration ContentDragger, SelectionDragger and Selection Node 
        //manipulation Node and content
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());



        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<CNodeSearchWindows>();
        _searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    private Port GeneratePort(CDialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, type: typeof(float)); //Arbitrary type


    }

    //Generate Start Node to work
    private CDialogueNode GenerateEntryPointNode()
    {
        //Set Value Node
        var node = new CDialogueNode
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatePort = GeneratePort(node, Direction.Output);
        generatePort.portName = "Next";
        node.outputContainer.Add(generatePort);


        //selected Nodes to Rectangule selector
        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Movable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }


    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach(funcCall: (port) =>
        {

            var portView = port;
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public void CreateNode(string nodeName, Vector2 position)
    {
        AddElement(CreateDialogueNode(nodeName, position));
    }

    public CDialogueNode CreateDialogueNode(string nodeName, Vector2 position)
    {
        var dialogueNode = new CDialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        //Genera un puerto de entrada
        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "input";
        dialogueNode.inputContainer.Add(inputPort);

        dialogueNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "Node"));
        //Genera una opcion button

        var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
        button.text = "New choice";
        dialogueNode.titleButtonContainer.Add(button);

        var textField = new TextField(label: string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });

        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: position, DafaultNodeSize));

        return dialogueNode;
    }



    //Crea una opcion 

    public void AddChoicePort(CDialogueNode dialogueNode, string overiderPortName = "")
    {
        var generatePort = GeneratePort(dialogueNode, Direction.Output);

        var oldLabel = generatePort.contentContainer.Q<Label>(name: "type");
        generatePort.contentContainer.Remove(oldLabel);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count();
        // generatePort.portName = $"Choice {outputPortCount}";

        var OutputPortName = string.IsNullOrEmpty(overiderPortName) ? $"Option{outputPortCount + 1}" : overiderPortName;
        var textField = new TextField()
        {
            name = string.Empty,
            value = OutputPortName
        };
        textField.RegisterValueChangedCallback(evt => generatePort.portName = evt.newValue);
        generatePort.contentContainer.Add(child: new Label(text: " "));
        generatePort.contentContainer.Add(textField);
        var deleteButton = new Button(clickEvent: () => RemovePort(dialogueNode, generatePort))
        {
            text = "x"
        };



        generatePort.contentContainer.Add(deleteButton);
        generatePort.portName = OutputPortName;

        dialogueNode.outputContainer.Add(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private void RemovePort(CDialogueNode dialogueNode, Port generatePort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatePort.portName && x.output.node == generatePort.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }
       

        dialogueNode.outputContainer.Remove(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    public void AddPropertyToBlackBoard(CExposedProperty exposedProperty)
    {
        var property = new CExposedProperty();
        property.PropertyName = exposedProperty.PropertyName;
        property.PropertyValue = exposedProperty.PropertyValue;
        ExposedPropierties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
        container.Add(blackboardField);
        //      
        var propertyValueTextField = new TextField(label: "Value:")
        {
            value = property.PropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var changingPropertyIndex = ExposedPropierties.FindIndex(x => x.PropertyName == property.PropertyName);
            ExposedPropierties[changingPropertyIndex].PropertyValue = evt.newValue;
        });

        var blackBoardValueRow = new BlackboardRow(blackboardField,propertyValueTextField );
        container.Add(blackBoardValueRow);
        Blackboard.Add(container);
    }
        
    }


