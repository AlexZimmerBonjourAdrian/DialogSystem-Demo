using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;



namespace Subtegral.DialogueSystem.RunTime
{
    public class CDialogueParser : MonoBehaviour
    {

        [SerializeField] private CDialogueContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button buttonChoice;
        [SerializeField] private Text ChoicePrefab;
        
        // Start is called before the first frame update
        void Start()
        {
            var narrativeData = dialogue.NodeLinks.First();
            
        }

        // Update is called once per frame
        
       

    }
}
