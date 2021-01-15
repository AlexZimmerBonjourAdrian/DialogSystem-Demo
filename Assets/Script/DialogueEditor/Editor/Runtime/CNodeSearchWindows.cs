using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.Editor
{
    public class CNodeSearchWindows : ScriptableObject, ISearchWindowProvider
    {
        private CDialogueGraphView _graphView;
        private EditorWindow _window;
        
        private Texture2D _indentationIcon;
        public void Init(EditorWindow window, CDialogueGraphView graphView)
        {
            _graphView = graphView;
            _window = window;

            //indentatation hack for search window as a transparent icon
            _indentationIcon = new Texture2D(width: 1, height: 1);
            _indentationIcon.SetPixel(x: 0, y: 0, new Color(r: 0, g: 0, b: 0, a: 0));//Dont forget to set the alpha to 0 as well
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent (text: "Create Elements"), level:0),
            new SearchTreeGroupEntry(new GUIContent(text: "Dialogue Node"),level:1),
            new SearchTreeEntry(new GUIContent(text:"Dialogue Node",_indentationIcon))
            {
                userData = new CDialogueNode(),level= 2
            },
           // new SearchTreeEntry (new GUIContent(text: "Hello world"))
        };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            switch (SearchTreeEntry.userData)
            {
                case CDialogueNode dialogueNode:
                    _graphView.CreateNode("Dialogue Node", localMousePosition);
                    return true;
                default:
                    return false;
            }
        }

    }
}
