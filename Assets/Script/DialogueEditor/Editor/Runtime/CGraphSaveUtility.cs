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
    private CDialogueContainer _ContainerCache;
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
        _ContainerCache = Resources.Load<CDialogueContainer>(fileName);

        if(_ContainerCache == null)
        {
            EditorUtility.DisplayDialog(title: "File not found", message: "target dialogue graph file does not exists!", ok: "0");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
       for(var i = 0;i<Nodes.Count; i++)
        {
            var connections = _ContainerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            for(var j = 0;j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(),  (Port)targetNode.inputContainer[0]);
                targetNode.SetPosition(newPos: new Rect( _ContainerCache.DialogueNodeData.First(x => x.Guid == targetNodeGuid).Position, _targetGraphView.DafaultNodeSize
                    ));

               
               
            }
        }
    }

    public void LinkNodes(Port outPut, Port input)
    {
        var tempEdge = new Edge
        {
            output = outPut,
            input = input
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);

    }

    private void CreateNodes()
    {
       foreach(var nodeData in _ContainerCache.DialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText,Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _ContainerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ClearGraph()
    {
        //set entry points guid back from the save. Duscard existing guid.
        Nodes.Find(match: x => x.EntryPoint).GUID = _ContainerCache.NodeLinks[0].BaseNodeGuid;


        //Remove edges that connected to this node
        foreach (var node in Nodes)
        {

            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge=>_targetGraphView.RemoveElement(edge));

           
            //then remove the node
            _targetGraphView.RemoveElement(node);

            
        }

    }

    // private Cd
    // public static CGraphSaveUtility GetInstance)
    //public static CGraphSaveUtility GetInstance()
}
