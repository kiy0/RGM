using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBox : MonoBehaviour {

    public bool botOnly;

    private Renderer[] _renderers;
    private Transform _transformToFollow;
    private ProceduralBox _childBox;
    public Renderer[] Renderers {
        get
        {
            return _renderers;

        }
        set
        {
           
            _renderers = value;
            if (_childBox == null) _childBox = GetComponentInChildren<ProceduralBox>();
            if (_childBox != null) _childBox._renderers = value;

        }

    }
    public Transform TransformToFollow
    {
        get
        {
            return _transformToFollow;

        }   
        set
        {
            _transformToFollow = value;

            if (_childBox == null) _childBox = GetComponentInChildren<ProceduralBox>();
            if (_childBox != null) _childBox._transformToFollow = value;
        }
    }
    public float height = 1f;
    Mesh mesh;
    public Transform childPlane;
    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        bool boundsSet = false;
        if (Renderers == null) return bounds;
        foreach (var r in Renderers)
        {
            Bounds currentBounds = r.bounds;

            if (!boundsSet)
                bounds = currentBounds;
            else
                bounds.Encapsulate(currentBounds);
            boundsSet = true;

        }
        return bounds;
    }

    public Vector3[] GetBoundingPointsUp()
    {

        if (TransformToFollow == null) return new Vector3[4];
        var storedPos = TransformToFollow.position;
        //var storedRot = transform.rotation;
        TransformToFollow.position = Vector3.zero;
        //Quaternion ignoreRot = Quaternion.Euler(0f, storedRot.eulerAngles.y, 0f);
        //Quaternion ignoreRot = Quaternion.Euler(storedRot.eulerAngles.x, 0f, storedRot.eulerAngles.z);
        //Quaternion ignoreRot = transform.rotation;
        //Quaternion ignoreRot =  Quaternion.FromToRotation(transform.right, Vector3.right);

        //Quaternion ignoreRot = Quaternion.identity;
        //transform.rotation = Quaternion.Inverse(ignoreRot) ;
        Bounds b = GetBounds();
        //transform.rotation = storedRot;
        Vector3[] points = new Vector3[4];

        points[0] = b.center + new Vector3(b.extents.x, 0f, b.extents.z);
        points[1] = b.center + new Vector3(b.extents.x, 0f, -b.extents.z);
        points[2] = b.center + new Vector3(-b.extents.x, 0f, -b.extents.z);
        points[3] = b.center + new Vector3(-b.extents.x, 0f, b.extents.z);

        for (int i = 0; i < points.Length; ++i)
        {
            //points[i] = storedRot * points[i];
            points[i].y = 0;

        }
        TransformToFollow.position = storedPos;
        return points;
    }
    // Use this for initialization
    void Start () {

       
        
        //ap = GetComponentInParent<AssemblyPiece>();

        // You can change that line to provide another MeshFilter
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        mesh = filter.mesh;
        mesh.Clear();

        //float length = 1f;



        Vector3[] apbound = GetBoundingPointsUp();
        #region Vertices
        

        Vector3 p0 = apbound[0];
        Vector3 p1 = apbound[1];
        Vector3 p2 = apbound[2];
        Vector3 p3 = apbound[3];

        Vector3 p4 = apbound[0] + Vector3.up * height;
        Vector3 p5 = apbound[1] + Vector3.up * height;
        Vector3 p6 = apbound[2] + Vector3.up * height;
        Vector3 p7 = apbound[3] + Vector3.up * height;

        Vector3[] vertices = new Vector3[]
        {
	        // Bottom
	        p0, p1, p2, p3,
 
	        // Left
	        p7, p4, p0, p3,
 
	        // Front
	        p4, p5, p1, p0,
 
	        // Back
	        p6, p7, p3, p2,
 
	        // Right
	        p5, p6, p2, p1,
 
	        // Top
	        p7, p6, p5, p4
        };
        #endregion

        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	        // Bottom
	        down, down, down, down,
 
	        // Left
	        left, left, left, left,
 
	        // Front
	        front, front, front, front,
 
	        // Back
	        back, back, back, back,
 
	        // Right
	        right, right, right, right,
 
	        // Top
	        up, up, up, up
        };
        #endregion

        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        // Bottom
	        _11, _01, _00, _10,
 
	        // Left
	        _11, _01, _00, _10,
 
	        // Front
	        _11, _01, _00, _10,
 
	        // Back
	        _11, _01, _00, _10,
 
	        // Right
	        _11, _01, _00, _10,
 
	        // Top
	        _11, _01, _00, _10,
        };
        #endregion
        int[] triangles;

        #region Triangles
        if (botOnly)
        {
            triangles = new int[]
{
                // Bottom
                3, 1, 0,
                3, 2, 1,

            };
        }
        else
        {
            triangles = new int[]
            {
                // Bottom
                /*3, 1, 0,
                3, 2, 1,*/
 
	            // Left
	            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	            // Front
	            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	            // Back
	            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	            // Right
	            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

                // Top
                /*3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,*/

            };
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        
        
    }
	
	// Update is called once per frame
	void Update () {
        if (_transformToFollow == null) return;
        transform.position = TransformToFollow.transform.position;

        Vector3[] apbound = GetBoundingPointsUp();
        #region Vertices


        Vector3 p0 = apbound[0];
        Vector3 p1 = apbound[1];
        Vector3 p2 = apbound[2];
        Vector3 p3 = apbound[3];

        Vector3 p4 = apbound[0] + Vector3.up * height;
        Vector3 p5 = apbound[1] + Vector3.up * height;
        Vector3 p6 = apbound[2] + Vector3.up * height;
        Vector3 p7 = apbound[3] + Vector3.up * height;

        Vector3[] vertices = new Vector3[]
        {
	        // Bottom
	        p0, p1, p2, p3,
 
	        // Left
	        p7, p4, p0, p3,
 
	        // Front
	        p4, p5, p1, p0,
 
	        // Back
	        p6, p7, p3, p2,
 
	        // Right
	        p5, p6, p2, p1,
 
	        // Top
	        p7, p6, p5, p4
        };
        #endregion
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        transform.localScale = new Vector3(0.85f, transform.localScale.y, 0.85f);
        //if(childPlane != null)childPlane.localScale = new Vector3(0.7f, -0.7f, 0.7f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
    }
}
