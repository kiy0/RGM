using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope_Script : MonoBehaviour {
    BallBehaviour ball;
	// Use this for initialization
	void Start () {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        for (int i = 0; i < balls.Length; i++)
        {
            ball = balls[i].GetComponent<BallBehaviour>();
            if (ball != null)
            {
                break;
            }
        }
        GetComponent<CharacterJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>(); 
	}

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("grabable"))
        {
            print("kfoekfojwojegs");
            ball.isHanging = false;
            ball.Fall();
        }
    }
    void Update () {
		
	}
}
