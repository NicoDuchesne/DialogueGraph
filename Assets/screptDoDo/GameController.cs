using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        
            SaveManager.Instance.Save();
            Debug.Log("Save appelé par GameController");
        
    }
}