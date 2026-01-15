using System;
using UnityEngine;
using System.Collections.Generic;

public enum DialogueUIModuleType
{
    Panel,
    Popup,
    Bulle
}

public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;
    public List<RuntimeDialogueNode> AllNodes = new List<RuntimeDialogueNode>();
}

[Serializable]
public class RuntimeDialogueNode
{
    public string NodeID;
    public string SpeakerName;
    public string DialogueText;
    public List<ChoiceData> Choices = new List<ChoiceData>();
    public string NextNodeID;
    public DialogueUIModuleType UIModuleType;
    public float DisplayDuration; // Duration for which the node is displayed (used for Bulle type)
    
}

[Serializable]
public class ChoiceData
{
    public string ChoiceText;
    public string DestinationNodeID;
}
