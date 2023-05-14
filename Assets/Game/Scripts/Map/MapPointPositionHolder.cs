using TMPro;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Map
{
    [SelectionBase]
    public class MapPointPositionHolder : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro positionNumberText;

        [SerializeField]
        private Transform[] pawnPlaceHolder;

        private int _myNumberPosition;
        private List<Pawn> _pawnsOnPoint = new();

        public void SetNumberPosition(int numPos)
        {
            _myNumberPosition = numPos;
            positionNumberText.text = numPos.ToString();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            transform.rotation = Quaternion.Euler(Vector3.right);

            if (_pawnsOnPoint.Count > 0)
                SetPawnsPosition();
        }

#endif

        public void SetPawns(in ICollection<Pawn> pawns)
        {
            this._pawnsOnPoint = pawns.ToList();
            SetPawnsPosition();
        }

        public void RemovePawn(in Pawn pawn)
        {
            _pawnsOnPoint.Remove(pawn);
            SetPawnsPosition();
        }

        public void AddPawn(in Pawn pawn)
        {
            _pawnsOnPoint.Add(pawn);
            SetPawnsPosition();
        }

        [NaughtyAttributes.Button()]
        private void SetPawnsPosition()
        {
            int count = _pawnsOnPoint.Count;

            for (int i = 0; i < count; i++)
            {
                _pawnsOnPoint[i].transform.position = pawnPlaceHolder[i].position;
                _pawnsOnPoint[i].transform.rotation = transform.rotation;
            }
        }
    }
}