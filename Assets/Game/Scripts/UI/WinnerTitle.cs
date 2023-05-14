using TMPro;
using UnityEngine;
using System.Collections;
using Assets.Game.Scripts.Gameplay;

namespace Assets.Game.Scripts.UI
{
    public class WinnerTitle : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI winnerText;

        private void OnEnable()
        {
            GameManager.VictoryPawnAction += SetTextOfWinner;
        }

        private void OnDisable()
        {
            GameManager.VictoryPawnAction -= SetTextOfWinner;
        }

        private void SetTextOfWinner(Pawn winnerPawn)
        {
            winnerText.text = $"Winner is Player {winnerPawn.PlayerNumber} !!!";
        }
    }
}