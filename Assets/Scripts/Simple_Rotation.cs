using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Rotation : MonoBehaviour {

    private Vector3 rot;
    public bool isOn;

    void Start () {
        rot = new Vector3 (0,0,-50);
    }
	
	void Update () {
        if (isOn)
        {
            transform.Rotate(rot * Time.deltaTime, Space.Self);

        }
    }
}
