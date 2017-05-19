using UnityEngine;
using System.Collections;

public class FoveWorldVisualizer : MonoBehaviour
{
    #region Source
    // http://math.stackexchange.com/questions/175896/finding-a-point-along-a-line-a-certain-distance-away-from-another-point
    /*
    Another way, using vectors:
    Let v = (x1,y1)−(x0,y0)
    Normalize this to u = v / ||v||.

    The point along your line at a distance d from (x0,y0)(x0,y0) is then (x0,y0)+du, 
    if you want it in the direction of (x1,y1), 
    or (x0,y0)−du, if you want it in the opposite direction. 
     */
    #endregion

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
    }

    void Update()
    {
        if (Input.GetKeyDown(VisualizeEyesKey))
            VisualizeEyes = !VisualizeEyes;
        if (Input.GetKeyDown(VisualizeMidpointKey))
            VisualizeMidPoint = !VisualizeMidPoint;

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
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
