using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

//This class was itnially used to visually see the values saved in sliders and input field
//We also put here some SaveFields attributes
public class SaveValues : MonoBehaviour
{
    public static SaveValues Instance;

    //Sliders values
    public List<Slider> _sdValue = new();
    public TMPro.TMP_InputField _inptValue;

    //Input Field value
    public List<float> SliderValue => _sdValue.Select(x => x.value).ToList();
    public string InputValue => _inptValue.text;

    //We stock all the values that we want to save via SaveField
    //SaveField can be anywhere in any monobehavior, not necessarly all in one place like this
    [Header("SaveFields")]
    [SaveField] public string testString = "Hello";
    [SaveField] public int testInt = 25;
    [SaveField] public float testfloat = 30.7f;
    [SaveField] public bool testBool = true;

    private void Awake()
    {
        Instance = this;
    }
}
