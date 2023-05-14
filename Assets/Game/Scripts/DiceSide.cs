using UnityEngine;

namespace Assets.Game.Scripts
{
    [System.Serializable]
    public struct DiceSide
    {
        public DiceSide(int value, Vector3 side)
        {
            this.value = value;
            this.side = side;
        }

        [SerializeField]
        private int value;

        [SerializeField]
        private Vector3 side;

        public readonly int GetValue => value;

        public Vector3 GetSide => side;
    }
}