using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public GameObject UserTransformStart;
    public GameObject UserTransformTools;
    public GameObject UserTransformEnd;

    enum Position {Start, Tools, End};
    Position currentPosition;

    void Start () {
        
        currentPosition = Position.Start;
        transform.Translate(UserTransformStart.transform.position);

        UserTransformStart = GameObject.Find("Position_Start");
        UserTransformTools = GameObject.Find("Position_Tools");
        UserTransformEnd = GameObject.Find("Position_End");
        print("ugabuga " + UserTransformStart.transform.position);
        print(transform.rotation);
    }

    // Update is called once per frame
    void Update () {


            if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchPosition(currentPosition);
            print(transform.position);
        }

	}

    void SwitchPosition (Position pos)
    {
        switch (pos)
        {
            case Position.Start:
                {
                    transform.position = UserTransformStart.transform.position;
                    currentPosition = Position.Tools;
                    break;
                }
            case Position.Tools:
                {
                    transform.position = UserTransformTools.transform.position;
                    currentPosition = Position.End;
                    break;
                }
            case Position.End:
                {
                    transform.position = UserTransformEnd.transform.position;
                    currentPosition = Position.Start;
                    break;
                }
        }
    }
}

