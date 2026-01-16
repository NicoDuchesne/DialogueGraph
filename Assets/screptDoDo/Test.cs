using UnityEngine;

public class Test : MonoBehaviour
{
    [SaveFieldA] public int level;
    [SaveFieldB] private float health;
    [SaveFieldC("player_name")] public string playerName;
    [SaveFieldD] public bool hasKey;
}
