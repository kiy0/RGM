using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeSimple : MonoBehaviour
{

    public Material firstMaterial;
    public MeshRenderer childColor;

    void Start()
    {
        childColor = GetComponent<MeshRenderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if ("ball".Equals(collision.other.tag) || "domino".Equals(collision.other.tag))
        { 
        gameObject.GetComponent<MeshRenderer>().material = firstMaterial;
        childColor.material = firstMaterial;
        }
    }

}