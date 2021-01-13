using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Subtegral.DialogueSystem.DataContainers
{
    [SerializeField]
public class CDialogueContainer : ScriptableObject
{
    // Start is called before the first frame update
    public List<CNodeLinkData> NodeLinks = new List<CNodeLinkData>();
        public List<CExposedProperty> ExposedProperties = new List<CExposedProperty>();
        public List<CDialogueNodeData> DialogueNodeData = new List<CDialogueNodeData>();
}
}