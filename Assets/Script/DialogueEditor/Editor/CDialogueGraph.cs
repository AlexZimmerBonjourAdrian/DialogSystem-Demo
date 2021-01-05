using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
//using PixelCrushers.DialogueSystem.DialogueEditor;

public class CDialogueGraph : EditorWindow
{
    // Start is called before the first frame update

    private CDialogueGraphView _graphView;

    private string _fileName= "New Narrative";

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
        ConstructGraphView();
        GenerateToolBar();

    }

    private void ConstructGraphView()
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

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNameTesstField = new TextField(label: "File Name:");
        fileNameTesstField.SetValueWithoutNotify(_fileName);
        fileNameTesstField.MarkDirtyRepaint();
        fileNameTesstField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTesstField);

        toolbar.Add(child: new Button(clickEvent: () => SaveData()) { text = "Save Data" });

        toolbar.Add(new Button(() => LoadData()) { text = "Load Data" });
        var nodeCreateButton = new Button(clickEvent: () =>
        {
            _graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void LoadData()
    {
        //throw new NotImplementedException();
    }

    private void SaveData()
    {
        //throw new NotImplementedException();
    }

    private void OnDisable()
    {
        //Disable element to Close
        rootVisualElement.Remove(_graphView);
    }

    
}
