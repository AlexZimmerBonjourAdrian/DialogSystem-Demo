using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class CDialogueGraphView : GraphView
{

    private readonly Vector2 dafaultNodeSize = new Vector2(x: 150, y: 200);
    // Start is called before the first frame update
   public CDialogueGraphView()
    {
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
        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "input";
        dialogueNode.inputContainer.Add(inputPort);
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: Vector2.zero, dafaultNodeSize));

        return dialogueNode;
    }
}
