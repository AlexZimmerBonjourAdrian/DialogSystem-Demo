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

        [MenuItem("Graph/Dialogue Graph")]
     public static void OpenDialogueGraphwindow()
    {

        var window = GetWindow<CDialogueGraph>();
        window.titleContent = new GUIContent(text: "Dialogue Graph");

    }

    private void OnEnable()
    {

        _graphView = new CDialogueGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    
}
