using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public Transform[] spawnPoints;

    public GameObject obstaclePrefab;

    public GameObject scorePrefab;

    public GameObject cube;

    public float forwardForce = 20f;

    public float timeBetweenWaves = 1f;

    private List<GameObject> obstacles = new List<GameObject>();

    public void SpawnObstacles()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (randomIndex != i)
            {
                GameObject ob = Instantiate(obstaclePrefab, spawnPoints[i].position, Quaternion.identity);
                ob.tag = "Obstacle";
                obstacles.Add(ob);
            } else
            {
                GameObject ob = Instantiate(scorePrefab, spawnPoints[i].position, Quaternion.identity);
                ob.tag = "Score";
                obstacles.Add(ob);
            }
        }
    }

    public void RemoveObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i])
            {
                Destroy(obstacles[i]);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacles", 0f, timeBetweenWaves);
    }

    // Update is called once per frame
    void FixedUpdate()

    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            // Add constant force to obstacle
            if (obstacles[i])
            {
                obstacles[i].GetComponent<Rigidbody>().AddForce(0, 0, -forwardForce * Time.fixedDeltaTime, ForceMode.VelocityChange);

                // Remove obstacle if past player
                if (obstacles[i].transform.position.z < cube.transform.position.z-10)
                {
                    Destroy(obstacles[i]);
                }

            }
        }

    }
}
