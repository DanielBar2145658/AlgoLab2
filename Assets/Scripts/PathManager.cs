using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Waypoint> path;

    public GameObject prefab;
    int CurrentPointIndex = 0;
    public List<GameObject> prefabPoints;


    private void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (Waypoint p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.pos;
            prefabPoints.Add(go);
        }
    }


    public List<Waypoint> GetPath() 
    {
        if (path == null) 
        {
            path = new List<Waypoint>();
        }
        return path;
    }
    public void CreateAddPoint() 
    {
        Waypoint go = new Waypoint();
        path.Add(go);
    }
    public Waypoint GetNextTarget() 
    {
        int nextPointIndex = (CurrentPointIndex + 1) % (path.Count);
        CurrentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }

}
