using F2O.SaveSystem;
using TMPro;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public TMP_Text slotText;

    public void SelectSlot(int index)
    {
        SaveManager.Instance.currentSlot = (SaveSlot)index;
        Refresh();
    }

    public void NewSave()
    {
        SaveManager.Instance.Save();
        Refresh();
    }

    public void LoadSave()
    {
        SaveManager.Instance.Load();
        Refresh();
    }

    public void OpenFolder()
    {
        SaveManager.Instance.OpenSaveFolder();
    }

    void Refresh()
    {
        var slot = SaveManager.Instance.currentSlot;
        slotText.text = SaveManager.Instance.SlotExists(slot)
            ? $"Slot {slot} occupé"
            : $"Slot {slot} vide";
    }
}