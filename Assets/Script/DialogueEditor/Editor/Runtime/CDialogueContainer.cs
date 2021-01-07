using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CDialogueContainer : ScriptableObject
{
    // Start is called before the first frame update
    public List<CNodeLinkData> NodeLinks = new List<CNodeLinkData>();
    public List<CDialogueNodeData> DialogueNodeData = new List<CDialogueNodeData>();
}
