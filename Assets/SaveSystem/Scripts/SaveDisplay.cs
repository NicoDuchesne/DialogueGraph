using F2O.SaveSystem;
using TMPro;
using UnityEngine;

public class SaveDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtSaveName;
    [SerializeField] private TMP_Text _txtDateCreated;
    [SerializeField] private TMP_Text _txtDateUpdated;

    private MySaveData data;

    public void Init(MySaveData data)
    {
        this.data = data;
        OnRefreshUI();
    }

    public void OnSaveAs()
    {
        MySaveManager.GameDataToSave(ref data);
        MySaveSystem.GameSaveAs(ref data);

        OnRefreshUI();
    }

    public void OnLoad()
    {
        data = MySaveSystem.LoadGameSave(_txtSaveName.text);
        MySaveManager.GameDataToLoad(data);
    }

    public void OnDelete()
    {
        MySaveSystem.DeleteGameSave(data);
        Destroy(gameObject);
    }

    private void OnRefreshUI()
    {
        _txtSaveName.text = data.Label;
        _txtDateCreated.text = $"Created : {data.DateCreated}";
        _txtDateUpdated.text = $"Updated : {data.DateUpdated}";
    }

}
