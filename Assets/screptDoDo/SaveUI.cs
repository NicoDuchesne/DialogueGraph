using UnityEngine;
using TMPro;

public class SaveUI : MonoBehaviour
{
    [Header("Références")]
    public PlayerData playerData;
    public TMP_Text infoText;

    public void UpdateUI()
    {
        infoText.text =
            $"Level : {playerData.level}\n" +
            $"Health : {playerData.health}\n";
     
    }

    public void Save()
    {
        playerData.Save();
        UpdateUI();
        Debug.Log("Sauvegarde effectuée");
    }

    public void Load()
    {
        playerData.Load();
        UpdateUI();
        Debug.Log("Chargement effectué");
    }

    void Start()
    {
        UpdateUI();
    }
}