using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Linq;
//using PixelCrushers.DialogueSystem.DialogueEditor;

public class CDialogueGraph : EditorWindow
{
    private string _fileName = "New Narrative";
    // Start is called before the first frame update
   // private bool anchored = false;

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
        GenerateMiniMap();
        GenerateBlackBoad();

    }

    private void GenerateBlackBoad()
    {
        var blackboard = new Blackboard(_graphView);
        blackboard.Add(child: new BlackboardSection { title = "Exposed Properties" });
       
       
        blackboard.addItemRequested = _blackboard =>
         {
             _graphView.AddPropertyToBlackBoard(new CExposedProperty());  
         };
        blackboard.editTextRequested = (blackboard1, element, newValue) =>
        {

            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedPropierties.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog(title: "Error", message: "This property name already exists, please chose another one!", ok: "OK");

                return;
            }

            var propertyIndex = _graphView.ExposedPropierties.FindIndex(match: x => x.PropertyName == oldPropertyName);
            _graphView.ExposedPropierties[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;
        };

        blackboard.SetPosition(newPos: new Rect(x: 10, y: 30, width: 200, 300));
        _graphView.Add(blackboard);
        _graphView.Blackboard = blackboard;

        
    }

    private void GenerateMiniMap()
    {
        var minMap = new MiniMap{anchored = true};
        //this will give 10px offset left side
        var cards = _graphView.contentViewContainer.WorldToLocal(p: new Vector2(x: this.maxSize.x - 10, y: 30));
        minMap.SetPosition(newPos: new Rect(x: cards.x, cards.y, width: 200, height: 140));
        _graphView.Add(minMap);
    }

    private void ConstructGraphView()
    {
        //Create new _GraphView, how name "Dialogue Graph"
        _graphView = new CDialogueGraphView(this)
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
            _graphView.CreateNode("Dialogue Node", Vector2.zero);
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
