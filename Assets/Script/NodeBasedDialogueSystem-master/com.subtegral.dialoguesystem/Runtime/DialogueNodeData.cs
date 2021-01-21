using System;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public string NodeGUID;
        public string DialogueText;
        public Sprite SpriteCharacter;
        public Vector2 Position;
        public string nameCharacter;
    }
}