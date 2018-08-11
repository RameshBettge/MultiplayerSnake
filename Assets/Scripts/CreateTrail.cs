using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrail : MonoBehaviour
{
    List<GameObject> trails;
    Team team;

    TrailPooler pooler;

    void Start()
    {
        team = GetComponent<Obstacle>().team;
        pooler = GameObject.FindGameObjectWithTag("Pooler").GetComponent<TrailPooler>();


        trails = new List<GameObject>();
    }

    void SpawnTrail(Vector3 pos)
    {
        pooler.GetTrail(team, pos);
    }

    void Update()
    {

    }
}
