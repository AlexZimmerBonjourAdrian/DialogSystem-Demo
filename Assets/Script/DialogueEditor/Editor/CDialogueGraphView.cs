using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class CDialogueGraphView : GraphView
{
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
        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }  
}
