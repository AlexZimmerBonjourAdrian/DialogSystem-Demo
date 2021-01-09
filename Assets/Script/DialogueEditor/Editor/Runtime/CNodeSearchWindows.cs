using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CNodeSearchWindows : ScriptableObject, ISearchWindowProvider
{
    private CDialogueGraphView _graphView;
    private EditorWindow _window;
    public void Init(EditorWindow window,CDialogueGraphView graphView)
    {
        _graphView = graphView;
        _window = window;
        
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent (text: "Create Elements"), level:0),
            new SearchTreeGroupEntry(new GUIContent(text: "Dialogue Node"),level:1),
            new SearchTreeEntry(new GUIContent(text:"Dialogue Node"))
            {
                userData = new CDialogueNode(),level= 2
            },
           // new SearchTreeEntry (new GUIContent(text: "Hello world"))
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,  context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        
        switch(SearchTreeEntry.userData)
        {
            case CDialogueNode dialogueNode:
                _graphView.CreateNode("Dialogue Node",worldMousePosition);
                return true;
            default:
                return false;
        }
    }
   
}
