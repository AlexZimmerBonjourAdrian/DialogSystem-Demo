﻿namespace Subtegral.DialogueSystem.DataContainers
{
    [System.Serializable]
    public class ExposedProperty
    {
        public static ExposedProperty CreateInstance()
        {
            return new ExposedProperty();
        }

        public string PropertyName = "New String";
        public string PropertySprite = "New Sprite";
        public string PropertyValue = "New Value";
    }
}