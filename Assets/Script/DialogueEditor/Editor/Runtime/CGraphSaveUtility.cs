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
        
    }

    public void LoadGraph(string fileName)
    {

    }
    
   // private Cd
   // public static CGraphSaveUtility GetInstance)
    //public static CGraphSaveUtility GetInstance()
}
