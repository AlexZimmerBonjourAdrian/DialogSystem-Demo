using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using UnityEngine.UIElements;

public class CGraphSaveUtility
{
    // Start is called before the first frame update
   // private CDialogueGraphView _targetGraphView;

    private List<Edge> Edges => _graphView.edges.ToList();
    private List<CDialogueNode> Nodes => _graphView.nodes.ToList().Cast<CDialogueNode>().ToList();
    private CDialogueGraphView _graphView;
    public static CGraphSaveUtility GetInstance(CDialogueGraphView targetGraphView)
    {

        return new CGraphSaveUtility
        {
           _graphView= targetGraphView
        };
    
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return; 
        
            var DialogueConteiner = ScriptableObject.CreateInstance<CDialogueConteiner>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for(var i = 0;i <connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as CDialogueNode;
            var inputNode = connectedPorts[i].input.node as CDialogueNode;

            DialogueConteiner.NodeLinks.Add(new CNodeLinkData()
            {
                BaseNodeGuid = outputNode.GUID,
                PortName= connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
         }
        foreach(var dialogueNode in Nodes.Where(node=>!node.EntryPoint))
        {
            DialogueConteiner.DialogueNodeData.Add(new CDialogueNodeData 
            {
                Guid = dialogueNode.GUID, 
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        if(!AssetDatabase.IsValidFolder("Assets/Resources"))
        AssetDatabase.CreateFolder("Asset", "Resources");

        AssetDatabase.CreateAsset(DialogueConteiner, $"Asset/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {

    }
}
