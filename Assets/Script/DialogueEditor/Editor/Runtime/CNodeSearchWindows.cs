using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CNodeSearchWindows : ScriptableObject, ISearchWindowProvider
{
    

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
        switch(SearchTreeEntry.userData)
        {
            case CDialogueNode dialogueNode:
                Debug.Log(message: "Dialogue Node Created!!!");
                return true;
            default:
                return false;
        }
    }
   
}
