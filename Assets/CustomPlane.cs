using UnityEngine;
using UnityEngine.Serialization;

public class CustomPlane : MonoBehaviour
{
    public Vector3 planePosition;
    private Quaternion rotation;
    public Vector3 PlaneNormal
    {
        get { return rotation * Vector3.forward; }
    }

    // Other CustomPlane script logic and functionality here.
}