using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SaveValues : MonoBehaviour
{
    public static SaveValues Instance;

    public List<Slider> _sdValue = new();

    public List<float> SliderValue => _sdValue.Select(x => x.value).ToList();
    private void Awake()
    {
        Instance = this;
    }
}
