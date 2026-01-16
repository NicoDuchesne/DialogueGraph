using UnityEngine;

public class Test : MonoBehaviour
{
    [SaveField] public int level;
     private float health;
    [SaveField("player_name")] public string playerName;
    [SaveField] public bool hasKey;
}
