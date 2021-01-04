using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
public class CDialogueGraph : EditorWindow
{
    // Start is called before the first frame update
 
        [MenuItem("Graph/Dialogue Graph")]
     public static void OpenDialogueGraphwindow()
    {

        var window = GetWindow<CDialogueGraph>();
        window.titleContent = new GUIContent(text: "Dialogue Graph");

    }
}
