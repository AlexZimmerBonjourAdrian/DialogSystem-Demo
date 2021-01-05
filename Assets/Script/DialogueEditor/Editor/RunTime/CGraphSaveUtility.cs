using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGraphSaveUtility
{
    // Start is called before the first frame update
    private CDialogueGraphView _targetGraphView;

    public static CGraphSaveUtility GetInstance(CDialogueGraphView targetGraphView)
    {

        return new CGraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    
    }

    public void SaveGraph(string fileName)
    {

    }

    public void LoadGraph(string fileName)
    {

    }
}
