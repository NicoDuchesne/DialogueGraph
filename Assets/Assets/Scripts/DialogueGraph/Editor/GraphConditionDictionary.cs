using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Condition")]
public class DialogueConditionSO : ScriptableObject
{
    public string id;
    public string description;
}

[CreateAssetMenu(menuName = "Dialogue/Conditions Database")]
public class DialogueConditionsDatabaseSO : ScriptableObject
{
    public List<DialogueConditionSO> conditions;
}
