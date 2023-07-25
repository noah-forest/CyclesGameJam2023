using System.Numerics;
using UnityEngine;
using UnityEditor;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[CustomEditor(typeof(DragAndRotateLockedAxis))]
public class DragAndRotateLockedAxisInspector : Editor
{
    private DragAndRotateLockedAxis customPlane;
    private SerializedProperty positionProperty;

    private void OnEnable()
    {
        customPlane = (DragAndRotateLockedAxis)target;
        positionProperty = serializedObject.FindProperty("position");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        // Allow handles to be shown in the Scene View during editing
        if (customPlane == null)
            return;

        Handles.color = Color.yellow;

        Vector3 upLineDir = customPlane.rotation * Vector3.up * 0.5f;
        Vector3 rightLineDir = customPlane.rotation * Vector3.right * 0.5f;
        Handles.DrawLine(customPlane.planePosition - upLineDir, customPlane.planePosition + upLineDir);
        Handles.DrawLine(customPlane.planePosition - rightLineDir, customPlane.planePosition + rightLineDir);
        Handles.ArrowHandleCap(0, customPlane.planePosition, Quaternion.LookRotation(customPlane.PlaneNormal), HandleUtility.GetHandleSize(customPlane.planePosition)*1.2f, EventType.Repaint);

        // Draw move handle
        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(customPlane.planePosition, customPlane.transform.rotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customPlane, "Move Plane");
            customPlane.planePosition = newPosition;
        }

        // Draw rotation handle
        EditorGUI.BeginChangeCheck();
        Quaternion newRotation = Handles.RotationHandle(customPlane.rotation, customPlane.planePosition);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customPlane, "Rotate Plane");
            customPlane.rotation = newRotation;
        }
    }
}