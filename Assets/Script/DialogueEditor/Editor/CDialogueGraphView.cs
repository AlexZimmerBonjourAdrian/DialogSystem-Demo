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
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement( GenerateEntryPointNode()); 
    }

    private CDialogueNode GenerateEntryPointNode()
    {
        var node = new CDialogueNode
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };
        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }  
}
