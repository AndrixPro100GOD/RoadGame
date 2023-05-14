﻿using UnityEditor;

using UnityEngine;

namespace PathCreation.Examples
{
    [CustomEditor(typeof(PathSceneTool), true)]
    public class PathSceneToolEditor : Editor
    {
        protected PathSceneTool pathTool;
        private bool isSubscribed;

        public override void OnInspectorGUI()
        {
            using (EditorGUI.ChangeCheckScope check = new())
            {
                _ = DrawDefaultInspector();

                if (check.changed)
                {
                    if (!isSubscribed)
                    {
                        _ = TryFindPathCreator();
                        Subscribe();
                    }

                    if (pathTool.autoUpdate)
                    {
                        TriggerUpdate();

                    }
                }
            }

            if (GUILayout.Button("Manual Update"))
            {
                if (TryFindPathCreator())
                {
                    TriggerUpdate();
                    SceneView.RepaintAll();
                }
            }

        }

        private void TriggerUpdate()
        {
            if (pathTool.pathCreator != null)
            {
                pathTool.TriggerUpdate();
            }
        }


        protected virtual void OnPathModified()
        {
            if (pathTool.autoUpdate)
            {
                TriggerUpdate();
            }
        }

        protected virtual void OnEnable()
        {
            pathTool = (PathSceneTool)target;
            pathTool.onDestroyed += OnToolDestroyed;

            if (TryFindPathCreator())
            {
                Subscribe();
                TriggerUpdate();
            }
        }

        private void OnToolDestroyed()
        {
            if (pathTool != null)
            {
                pathTool.pathCreator.pathUpdated -= OnPathModified;
            }
        }


        protected virtual void Subscribe()
        {
            if (pathTool.pathCreator != null)
            {
                isSubscribed = true;
                pathTool.pathCreator.pathUpdated -= OnPathModified;
                pathTool.pathCreator.pathUpdated += OnPathModified;
            }
        }

        private bool TryFindPathCreator()
        {
            // Try find a path creator in the scene, if one is not already assigned
            if (pathTool.pathCreator == null)
            {
                if (pathTool.GetComponent<PathCreator>() != null)
                {
                    pathTool.pathCreator = pathTool.GetComponent<PathCreator>();
                }
                else if (FindObjectOfType<PathCreator>())
                {
                    pathTool.pathCreator = FindObjectOfType<PathCreator>();
                }
            }
            return pathTool.pathCreator != null;
        }
    }
}