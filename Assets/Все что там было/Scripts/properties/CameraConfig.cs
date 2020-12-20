using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public float TurnSmooth;
    public float PivotSpeed;
    public float YRotSpeed;
    public float XRotSpeed;
    public float MinAngle;
    public float MaxAngle;
    public float NormalZ;
    public float NormalX;
    public float AimZ;
    public float AimX;
    public float NormalY;
}
