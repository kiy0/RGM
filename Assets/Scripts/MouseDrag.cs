using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {

    public bool hasKey = false;
    public bool hasScissors = false;
    public bool hasWrench = false;

    GameObject grabbedObject;
    float grabbedObjectSize;
    //public Camera charCam;
    Ray rayMid;
    RaycastHit raycastHit;
    public Vector3 camDistance;

    public GameObject eyeSight;

    public bool VisualizeEyes
    {
        set
        {
            _visualizeEyes = value;
            LeftPointer.SetActive(value);
            RightPointer.SetActive(value);
        }
        get { return _visualizeEyes; }
    }
    private bool _visualizeEyes = false;
    public KeyCode VisualizeEyesKey = KeyCode.E;

    public bool VisualizeMidPoint
    {
        set
        {
            _visualizeMidpoint = value;
            MidpointPointer.SetActive(value);
        }
        get { return _visualizeMidpoint; }
    }
    private bool _visualizeMidpoint = true;
    public KeyCode VisualizeMidpointKey = KeyCode.M;

    public float DistanceOriginWorldPoint = 5.0f;
    public GameObject LeftPointer;
    public GameObject RightPointer;
    public GameObject MidpointPointer;

    public GazeHighLight GazeHighLighter;
    private GameObject highlightedObject = null;

    private void Start()
    {
        VisualizeEyes = _visualizeEyes;
        VisualizeMidPoint = _visualizeMidpoint;
        camDistance = new Vector3 (100, 0, 0);
        //charCam = GameObject.FindGameObjectWithTag("cam").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(VisualizeEyesKey))
            VisualizeEyes = !VisualizeEyes;
        if (Input.GetKeyDown(VisualizeMidpointKey))
        {
            VisualizeMidPoint = !VisualizeMidPoint;
            print("midpoint");
        }

        FoveInterface.EyeRays rays = FoveInterface.GetEyeRays();

        // LEFT
        Ray rayLeft = rays.left;
        Vector3 leftOrigin = rayLeft.origin;
        Vector3 leftDirection = rayLeft.direction;
        Vector3 leftWorldPos = leftOrigin + (DistanceOriginWorldPoint * leftDirection);

        // RIGHT
        Ray rayRight = rays.right;
        Vector3 rightOrigin = rayRight.origin;
        Vector3 rightDirection = rayRight.direction;
        Vector3 rightWorldPos = rightOrigin + (DistanceOriginWorldPoint * rightDirection);

        if (VisualizeEyes)
        {
            LeftPointer.transform.position = leftWorldPos;
            RightPointer.transform.position = rightWorldPos;
        }

        // MIDPOINT
        Vector3 midpoint = (rightWorldPos + leftWorldPos) / 2;
        Vector3 midOrigin = (rightOrigin + leftOrigin) / 2;
       // print midOrigin;

        Vector3 midDirection = (rightDirection + leftDirection) / 2;
        rayMid = new Ray (midOrigin, midDirection);


        if (VisualizeMidPoint)
        {
            MidpointPointer.transform.position = midpoint;
        }

        var tempObj = GetFocussedHighlightPiece(midpoint);

        if (tempObj != null)
        {
            if (highlightedObject != tempObj)
            {
                highlightedObject = tempObj;
                GazeHighLighter.SetProceduralBox(highlightedObject);
            }
        }

        //GRAB
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("DOWN" + grabbedObject);
            if (grabbedObject == null)
                TryGrabObject(GetMouseHoverObject(50));

            switch (grabbedObject.name)
            {
                case "Scissors":
                    hasScissors = true;
                    break;
                case "Key":
                    hasKey = true;
                    break;
                case "Wrench":
                    hasWrench = true;
                    break;
            }
            print("GRAB " + (grabbedObject != null));

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
        //    DropObject();
        }

        if (grabbedObject != null)

        {
        //    //Vector3 newPosition = charCam.ScreenToWorldPoint(Input.mousePosition) + charCam.transform.forward * (grabbedObjectSize + camDistance);
        //    Vector3 newPosition = midpoint;
        //    //Vector3 newPosition = eyeSight.transform.position;
        //    grabbedObject.transform.position = newPosition;
        //    print(newPosition);
        }
    }

    GameObject GetFocussedHighlightPiece(Vector3 dir)
    {
        Debug.DrawRay(gameObject.transform.position, -MidpointPointer.transform.forward * 20, Color.green);


        Ray ray = new Ray(gameObject.transform.position, -MidpointPointer.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30))
        {
            Debug.Log("Hit! " + hit.collider.gameObject.name);
            var itemCheck = hit.collider.gameObject.GetComponent<HighlightItem>();
            if (itemCheck != null)
            {
                Debug.Log("Hit! Correct item type.");
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        // lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.material = new Material(Shader.Find("Materials/ProceduralBox 1"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    GameObject GetMouseHoverObject(float range)
    {
       // Vector3 position = charCam.transform.position; 
       // Ray ray = charCam.ScreenPointToRay(Input.mousePosition);
        //Vector3 target = Input.mousePosition + charCam.transform.forward * range; 

        if (Physics.Raycast(rayMid, out raycastHit)) {

            return raycastHit.collider.gameObject;
        }
        //        if (raycastHit.collider.Raycast(ray, out raycastHit, range))
        //        {
        //            print("xxxxxxxxxxxxxx " + raycastHit.collider.gameObject + target);
        //            return raycastHit.collider.gameObject;
        //       }
        return null;

    }

    void TryGrabObject(GameObject grabObject)
    {
        if (grabObject == null || !CanGrab(grabObject))
            return;

        print("TAG " + grabObject.tag);
        if(grabObject.tag.Equals ("grabable"))
        {
            grabbedObject = grabObject;
            grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size.magnitude;

        }
    }
    bool CanGrab(GameObject candidate)
    {
        return candidate.GetComponent<Rigidbody>() != null;
    }

    void DropObject()
    {

        if (grabbedObject == null)
            return;


        if (grabbedObject.GetComponent<Rigidbody>() != null)
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        grabbedObject = null;
    }


}

