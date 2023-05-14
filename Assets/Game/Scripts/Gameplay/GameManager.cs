using System;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Map;
using UnityEngine.Events;
using Assets.Game.Scripts.UI;
using System.Collections.Generic;

namespace Assets.Game.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerCount_Slider playerCount_Slider;

        [SerializeField]
        private RollingDiceController rollingDiceController;

        [SerializeField]
        private MapController mapController;

        private bool _isPlaying = false;
        private int _currentPlayerIndex = 0;
        private int _turnCount = 0;
        private Pawn[] _playerList;

        public static Action<Pawn> VictoryPawnAction { get; set; }

        [SerializeField]
        private UnityEvent OnPlayerWins;

        private void Awake()
        {
            VictoryPawnAction += (_) => OnPlayerWins?.Invoke();
        }

        private void OnEnable()
        {
            rollingDiceController.OnRollingDone += HandleRollingResult;
        }

        private void OnDisable()
        {
            rollingDiceController.OnRollingDone -= HandleRollingResult;
        }

        public void Init()
        {
            mapController.Initialize(playerCount_Slider.GetPlayerCount());

            _playerList = mapController.GetPawns.ToArray();

            _isPlaying = true;
        }

        #region Rolling

        public void OnRollButtonPress()
        {
            if (!_isPlaying)
                return;

            rollingDiceController.StartRolling();
            _isPlaying = false;
        }

        private void HandleRollingResult()
        {
            IEnumerable<DiceSide> result = rollingDiceController.GetLastRollingResult;
            int valueToWalk = 0;
            foreach (DiceSide diceSide in result)
            {
                valueToWalk += diceSide.GetValue;
            }

            MakeTurn(valueToWalk);
        }

        #endregion Rolling

        private void MakeTurn(int valueToWalk)
        {
            int currentPlayerToMakeTurn = _currentPlayerIndex;

            mapController.PawnGoForward(_playerList[currentPlayerToMakeTurn], valueToWalk);

            //set next turn player index
            if (currentPlayerToMakeTurn + 1 < _playerList.Length)
            {
                _currentPlayerIndex += 1;
            }
            else
            {
                _turnCount += 1;
                _currentPlayerIndex = 0;
            }

            _isPlaying = true;
        }
    }
}