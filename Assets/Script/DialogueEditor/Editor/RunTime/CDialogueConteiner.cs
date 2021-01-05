using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CDialogueConteiner : ScriptableObject
{

    public List<CNodeLinkData> NodeLinks = new List<CNodeLinkData>();
    public List<CDialogueNodeData> DialogueNodeData = new List<CDialogueNodeData>();
}
