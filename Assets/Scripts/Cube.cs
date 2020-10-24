using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Cube : Agent
{
    [SerializeField] public float sidewaysForce = 100f;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;

    private Rigidbody rBody;
    private Vector3 startingPosition;
    //private int score = 0;

    public ObstacleSpawner obstacleSpawner;

    public GameObject ground;

    public event Action OnReset;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {

        if (transform.position.x < ground.transform.position.y)
        {
            AddReward(increment: 0.1f);
            EndEpisode();
        }

        RequestDecision();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            MoveLeft();
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            MoveRight();
        }
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;

        if (Input.GetKey(leftKey))
        {
            MoveLeft();
            actionsOut[0] = 1;
        }

        if (Input.GetKey(rightKey))
        {
            MoveRight();
            actionsOut[1] = 1;
        }
    }

    private void MoveLeft()
    {
        rBody.AddForce(-sidewaysForce * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);
        //Vector3 position = transform.position;
        //position.x -= .1f;
        //transform.position = position;

    }

    private void MoveRight()
    {
        rBody.AddForce(sidewaysForce * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);
        //Vector3 position = transform.position;
        //position.x += .1f;
        //transform.position = position;
    }

    private void Reset()
    {
        //score = 0;

        //transform.position = startingPosition;

        rBody.velocity = Vector3.zero;

        obstacleSpawner.RemoveObstacles();

        OnReset?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) 
        {
            AddReward(increment: -1.0f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            AddReward(increment: 1.0f);
            //score++;
        }
    }
}
