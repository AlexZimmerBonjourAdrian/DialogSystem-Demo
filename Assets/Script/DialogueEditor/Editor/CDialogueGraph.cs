using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class CDialogueGraph : EditorWindow
{
    // Start is called before the first frame update

    private CDialogueGraphView _graphView;

    //Menu implementation
        [MenuItem("Graph/Dialogue Graph")]
     public static void OpenDialogueGraphwindow()
    {
        //Open windows menu
        var window = GetWindow<CDialogueGraph>();
        //Name menu
        window.titleContent = new GUIContent(text: "Dialogue Graph");

    }

    private void OnEnable()
    {
        //Create new _GraphView, how name "Dialogue Graph"
        _graphView = new CDialogueGraphView
        {
            name = "Dialogue Graph"
        };

        //Set Config to see
        _graphView.StretchToParentSize();
        
        rootVisualElement.Add(_graphView);

    }

    private void OnDisable()
    {
        //Disable element to Close
        rootVisualElement.Remove(_graphView);
    }

    
}
