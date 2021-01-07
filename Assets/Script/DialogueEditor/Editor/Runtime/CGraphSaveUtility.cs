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
//using Subtegral.DialogueSystem.DataContainers;
using UnityEngine.UIElements;

public class CGraphSaveUtility
{
    private CDialogueGraphView _targetGraphView;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<CDialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<CDialogueNode>().ToList();


    public static CGraphSaveUtility GetInstance(CDialogueGraphView targetGraphView)
    {
       return new CGraphSaveUtility
        {
        _targetGraphView= targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return;
        var dialogueConteniner = ScriptableObject.CreateInstance<CDialogueContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        for(var i= 0; i<connectedPorts.Length;i++)
        {
            var outPutNode = connectedPorts[i].output.node as CDialogueNode;
            var inputNode = connectedPorts[i].input.node as CDialogueNode;

            dialogueConteniner.NodeLinks.Add(item: new CNodeLinkData
            {
                BaseNodeGuid=outPutNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }
        foreach(var dialogNode in Nodes.Where(node =>!node.EntryPoint))
        {
            dialogueConteniner.DialogueNodeData.Add(item: new CDialogueNodeData
            {
                Guid = dialogNode.GUID,
                DialogueText = dialogNode.DialogueText,
                Position = dialogNode.GetPosition().position
            });
        }


        if (!AssetDatabase.IsValidFolder(path: "Assets/Resources")){
            AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");
        }
        AssetDatabase.CreateAsset(dialogueConteniner, path: $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {

    }
    
   // private Cd
   // public static CGraphSaveUtility GetInstance)
    //public static CGraphSaveUtility GetInstance()
}
