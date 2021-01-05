using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class CDialogueGraphView : GraphView
{

    private readonly Vector2 dafaultNodeSize = new Vector2(x: 150, y: 200);
    // Start is called before the first frame update
   public CDialogueGraphView()
    {
        //Zoom 
        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
        //Set configuration ContentDragger, SelectionDragger and Selection Node 
        //manipulation Node and content
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement( GenerateEntryPointNode()); 
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
            if(startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

            return compatiblePorts;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    public CDialogueNode CreateDialogueNode(string nodeName)
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


        //Genera una opcion button

        var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
        button.text = "New choice";
        dialogueNode.titleButtonContainer.Add(button);

       ;

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: Vector2.zero, dafaultNodeSize));

        return dialogueNode;
    }

    //Crea una opcion 

    private void AddChoicePort(CDialogueNode dialogueNode)
    {
        var generatePort = GeneratePort(dialogueNode, Direction.Output);
        var outputPortCount = dialogueNode.outputContainer.Query( name: "connector").ToList().Count;
        generatePort.portName = $"Choice {outputPortCount}";



        dialogueNode.outputContainer.Add(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
