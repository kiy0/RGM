using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour {

    public GameObject Handle;
    private Vector3 pos;
    public bool isHanging = true;
    private GameObject[] ropeArr;
    MouseDrag inst;
 

    void Start()
    {
        ropeArr = GameObject.FindGameObjectsWithTag("rope");
        if (isHanging)
        {
            IgnoreRopeCollision();
        }
        inst = GetComponent<MouseDrag>();
    }
    void IgnoreRopeCollision()
    {
        for (int i = 0; i < ropeArr.Length; i++)
        {
            Physics.IgnoreCollision(ropeArr[i].GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
    public void Fall()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update () {

   //     if (Input.GetKeyDown(KeyCode.E) && inst.hasScissors)
        {
            Fall();
        }

        if(isHanging)
        { 

        pos = Handle.transform.position;
        transform.position = pos;
            return;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.name == "Scissors")
        //{
        ////    isHanging = false;
        //    Fall();
        //}

    }
}
