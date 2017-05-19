using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {
    
    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    private bool isDragged;
    GameObject character;
    
	void Start () {
        character = this.transform.parent.gameObject;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            isDragged = true;

        if (Input.GetMouseButtonUp(0))
            isDragged = false;

        if (!isDragged)
            return;
     
        var md = new Vector2(Input.GetAxisRaw("MouseX"), Input.GetAxisRaw("MouseY"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
