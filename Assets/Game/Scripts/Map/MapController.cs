using UnityEngine;
using System.Collections.Generic;
using Assets.Game.Scripts.Gameplay;

namespace Assets.Scripts.Map
{
    public class MapController : MonoBehaviour
    {
        [SerializeField]
        private Pawn pawnPrefab;

        [SerializeField]
        private int startNumber = 1;

        [SerializeField]
        private Vector3 pointsRotation = Vector3.zero;

        private Pawn[] _pawns;
        private List<MapPointPositionHolder> _mapPoints = new();

        public ICollection<Pawn> GetPawns => _pawns;

#if UNITY_EDITOR

        private void OnValidate()
        {
            InitializeMap();
        }

#endif

        #region Init

        [NaughtyAttributes.Button("Initialize map")]
        [ContextMenu("Initialize map")]
        private void InitializeMap()
        {
            SetUpMapPoint();
        }

        public void Initialize(int pawnsCount)
        {
            InitializeMap();

            if (_pawns != null && _pawns.Length > 0)
            {
                //clear
                foreach (var pawn in _pawns)
                {
                    Destroy(pawn);
                }
                _pawns = null;
            }

            //create new pawns
            _pawns = new Pawn[pawnsCount];

            for (int i = 0; i < _pawns.Length; i++)
            {
                _pawns[i] = Instantiate(pawnPrefab);
                _pawns[i].SetPlayerNumber(i + 1);
            }

            //set pawns to start position
            _mapPoints[0].SetPawns(_pawns);
        }

        [NaughtyAttributes.Button("Set up map points")]
        [ContextMenu("Set up map points")]
        private void SetUpMapPoint()
        {
            _mapPoints.Clear();

            int number = startNumber;
            foreach (Transform element in transform)
            {
                if (element.TryGetComponent(out MapPointPositionHolder component))
                {
                    SetNumberToPoint(component, number);
                    SetRotationPointToVector(component, pointsRotation);
                    _mapPoints.Add(component);
                }
                number++;
            }
        }

        private void SetNumberToPoint(in MapPointPositionHolder mapPoint, in int number) => mapPoint.SetNumberPosition(number);

        private void SetRotationPointToVector(in MapPointPositionHolder mapPoint, in Vector3 vectorRotation) => mapPoint.transform.rotation = Quaternion.Euler(vectorRotation);

        #endregion Init

        public void PawnGoForward(Pawn pawn, int stepsToGo)
        {
            int maxPos = _mapPoints.Count;
            int currentPos = pawn.CurrentPosition;
            int nextPos = currentPos + stepsToGo;

            if (nextPos > maxPos - 1)
            {
                nextPos = _mapPoints.Count - 1;
                GameManager.VictoryPawnAction?.Invoke(pawn);
            }

            _mapPoints[currentPos].RemovePawn(pawn);
            _mapPoints[nextPos].AddPawn(pawn);

            pawn.SetPosition(nextPos);
        }
    }
}