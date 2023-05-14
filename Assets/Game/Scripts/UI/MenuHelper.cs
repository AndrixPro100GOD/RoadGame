using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.UI
{
    public class MenuHelper : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onPlay;

        [SerializeField]
        private UnityEvent onPause;

        [SerializeField]
        private UnityEvent onResume;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseToggle();
            }
        }

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        public void Play()
        {
            SetPause(false);
            onPlay?.Invoke();
        }

        public void PauseToggle()
        {
            SetPause(Time.timeScale >= 0.1f);
        }

        public void SetPause(bool value)
        {
            Time.timeScale = value ? 0f : 1f;

            if (value)
                onPause?.Invoke();
            else
                onResume?.Invoke();
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}