using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField]
    PathManager pathManager;

    [SerializeField]
    List<Waypoint> path;

    List<int> toDelete;

    Waypoint selectedPoint = null;
    bool doRepaint = true;


    private void OnSceneGUI()
    {
        path = pathManager.GetPath();
        DrawPath(path);
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        path = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path")) 
        {
            pathManager.CreateAddPoint();
        }
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();

    }

    void DrawGUIForPoints()
    {
        if (path != null && path.Count > 0) 
        {
            for (int i = 0; i < path.Count; i++) 
            {
                EditorGUILayout.BeginHorizontal();
                Waypoint p = path[i];

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                Color c = GUI.color;
                if (selectedPoint == p) GUI.color = Color.green;

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                if (GUILayout.Button("-", GUILayout.Width(25))) 
                {
                    toDelete.Add(i);
                }

                GUI.color = c;

                EditorGUILayout.EndHorizontal();

            }

            if (toDelete.Count > 0) 
            {
                foreach (int i in toDelete)
                    path.RemoveAt(i);
                toDelete.Clear();
            }

        }
    }

    void DrawPath(List<Waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (Waypoint wp in path) 
            {
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                Waypoint wpnext = path[next];

                DrawPathLine(wp, wpnext);


                current += 1;


            }
            if (doRepaint) Repaint();

        }


    }

    public bool DrawPoint(Waypoint p)
    {
        bool isChanged = false;


        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = p.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);
            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.25f* handleSize, EventType.Repaint);
            if (EditorGUI.EndChangeCheck()) 
            {
                p.SetPos(newPos);
            }
            Handles.color = c;


        }
        else 
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);

            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }

        }
        
        
        
       
        return isChanged;

    }

    public void DrawPathLine(Waypoint p1, Waypoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.grey;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;



    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
