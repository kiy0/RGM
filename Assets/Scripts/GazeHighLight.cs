using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeHighLight : MonoBehaviour
{
    public ProceduralBox ProceduralBox = null;
    static ProceduralBox _highLightedObject;

    public void SetProceduralBox(GameObject piece)
    {
        if(_highLightedObject != null)
            DestroyImmediate(_highLightedObject.gameObject);

        var box = Instantiate(ProceduralBox.gameObject).GetComponent<ProceduralBox>();
        _highLightedObject = box;
            
       box.Renderers = piece.GetComponents<Renderer>();
       box.TransformToFollow = piece.transform;
       box.gameObject.SetActive(true);
    }
}
