using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using PathCreationEditor;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.PathCreator.Core.Editor
{
    public static class AndrixExtended
    {
        public static Vector3 GetPoint()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                Debug.Log($"Hit to {hit.transform.name} at {hit.point})");
                return hit.point;
            }

            return mouseRay.GetPoint(5);
        }
    }
}