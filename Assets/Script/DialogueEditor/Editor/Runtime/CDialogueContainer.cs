
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Subtegral.DialogueSystem.DataContainers
 {
    [Serializable]
    public class CDialogueContainer : ScriptableObject
    {
        public List<CNodeLinkData> NodeLinks = new List<CNodeLinkData>();
        public List<CExposedProperty> ExposedProperties = new List<CExposedProperty>();
        public List<CDialogueNodeData> DialogueNodeData = new List<CDialogueNodeData>();
    }
  }