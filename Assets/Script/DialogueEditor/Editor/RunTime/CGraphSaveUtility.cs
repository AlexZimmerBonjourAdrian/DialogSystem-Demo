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
    private CDialogueConteiner _containerCache;
    //private CDialogueGraphView _targetGraphView;
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
        _containerCache = Resources.Load<CDialogueConteiner>(fileName);

        if(_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");
        }
        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
       
    }

    private void CreateNodes()
    {
       foreach(var nodeData in _containerCache.DialogueNodeData)
        {
            var tempNode = _graphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.Guid;
            _graphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _graphView.AddChoicePort(tempNode,x.PortName));
        }
    }

    private void ClearGraph()
    {
        //Set Entry points guid back from save. Discard Existing guid.
        Nodes.Find(X => X.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;
        foreach(var node in Nodes)
        {
            //Remove edges thtat connected to this node
            if (node.EntryPoint) return;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _graphView.RemoveElement(edge));


            //then remove the node
            _graphView.RemoveElement(node);
        }

    }

}
