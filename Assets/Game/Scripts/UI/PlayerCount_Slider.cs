using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class PlayerCount_Slider : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Slider _slider;

        private void Reset()
        {
            _slider = GetComponent<Slider>();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            _slider = _slider == null ? GetComponent<Slider>() : _slider;

            _slider.wholeNumbers = true;
            _slider.minValue = 1;
            _slider.maxValue = 5;
        }

#endif

        public int GetPlayerCount()
        {
            return (int)_slider.value;
        }
    }
}