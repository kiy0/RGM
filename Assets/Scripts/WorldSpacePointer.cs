using UnityEngine.UI;
using UnityEngine;

public class WorldSpacePointer : MonoBehaviour
{
    // Rotation towards
    public float RotationSpeed;
    public GameObject ObjectToRotate;
    public GameObject ObjectToLookAt;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Tag
    public bool EnableTag = true;
    public Text Tag;
    public string TagContent = "";

    void Start()
    {
        #region Set up Tag
        if (Tag != null)
            Tag.text = TagContent;
        #endregion
    }

    void Update()
    {
        #region Rotation
        // Find the vector pointing from our position to the target.
        _direction = (ObjectToLookAt.transform.position - transform.position).normalized;
        // Create the rotation we need to be in to look at the target.
        _lookRotation = Quaternion.LookRotation(_direction);
        // Rotate us over time according to speed until we are in the required rotation.
        ObjectToRotate.transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        #endregion
    }
}
