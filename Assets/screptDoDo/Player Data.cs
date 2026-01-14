using UnityEngine;

public class PlayerData : SaveBehaviour
{
    [SaveField]
    public int level = 1;

    [SaveField("HP")]
    public float health = 100f;

    [SaveField]
    [SerializeField] private bool hasSword;

    void Start()
    {
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }
}
