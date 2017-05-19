using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkMovement : MonoBehaviour {
    float xRandom;
    float yRandom;
    float zRandom;
    float timeRandom;
    public GameObject closestTarget;
    public ParticleSystem Spark;
    public GameObject Trigger;
    public float TargetTime;
    private bool BeginTimer = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timeRandom)
        {
        //    xRandom = Random.Range(-xRandom, xRandom);
        //    yRandom = Random.Range(-yRandom, yRandom);
        //    zRandom = Random.Range(0, zRandom);
         //   timeRandom = Time.time + timeRandom;
        }
        if(BeginTimer)
        {
            if (TargetTime <= 0.0f)
            {
                timerEnded();
            }
            TargetTime -= Time.deltaTime;
        }
    }
    void OnTriggerEnter(Collider otherCol)
    {
        print("Halla");
        BeginTimer = true;
       
    }
    void timerEnded()
    {
        Spark.Play();
        //Upwards movement
        transform.position = Vector3.MoveTowards(transform.position, closestTarget.transform.position, Time.deltaTime);

        //Adds randomized trajectory 
        //transform.Translate(1, 1, 1, closestTarget.transform);

        //keeps the missile looking at its target
        //transform.LookAt(closestTarget.transform.position);

    }
}
