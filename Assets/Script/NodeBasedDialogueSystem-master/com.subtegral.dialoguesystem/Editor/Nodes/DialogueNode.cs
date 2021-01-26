using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Subtegral.DialogueSystem.Editor
{
    public class DialogueNode : Node
    {
        public string DialogueText;
        public string GUID;
        public Texture2D SpriteCharacter;
        public string Name;
        public string NameCharacter;
        public bool EntyPoint = false;
    }
}