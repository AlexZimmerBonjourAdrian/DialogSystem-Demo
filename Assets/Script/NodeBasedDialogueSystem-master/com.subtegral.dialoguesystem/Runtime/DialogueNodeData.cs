using System;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public string NodeGUID;
        public string nameCharacter;
        public string DialogueText;
        public Texture2D SpriteCharacter;
        public Vector2 Position;
       
    }
}