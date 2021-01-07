using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
//using PixelCrushers.DialogueSystem.DialogueEditor;

public class CDialogueGraph : EditorWindow
{
    private string _fileName = "New Narrative";
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
        var filenameTextField = new TextField(label: "File Name:");
        filenameTextField.SetValueWithoutNotify(_fileName);
        filenameTextField.MarkDirtyRepaint();
        filenameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(filenameTextField);

        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save: true)) { text = "Save Data" });
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save: false)) { text = "Load Data" });

        var nodeCreateButton = new Button(clickEvent: () =>
        {
            _graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
       if(string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog(title: "Invalid file Name!", message: "Please enter a valid file name", ok: "OK");
            return;
        }

        var saveUtility = CGraphSaveUtility.GetInstance(_graphView);
           
            if(save)
            {
                saveUtility.SaveGraph(_fileName);
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
            }
    }

    private void OnDisable()
    {
        //Disable element to Close
        rootVisualElement.Remove(_graphView);
    }

    
}
