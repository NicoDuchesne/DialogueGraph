using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace F2O.SaveSystem
{
    public class ExempleSaveValues : MonoBehaviour
    {
        public static ExempleSaveValues Instance;

        public List<Slider> _sdValue = new();
        public Image imgPokemon;
        public TMP_InputField ifdPokemon;

        public List<float> SliderValue => _sdValue.Select(x => x.value).ToList();
        private void Awake()
        {
            Instance = this;
        }
    }
}
