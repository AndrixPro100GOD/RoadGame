using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class SliderValueViewer : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField, HideInInspector]
        private TextMeshProUGUI text;

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            OnSliderValueChanged();
        }

        public void OnSliderValueChanged()
        {
            text.text = slider.value.ToString();
        }
    }
}