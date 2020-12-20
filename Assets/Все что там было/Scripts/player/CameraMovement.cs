using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform CamTransform;
    public Transform Pivot;
    public Transform Character;
    public Transform MTransform;

    public CharacterStatus CharacterStatus;
    public CameraConfig CameraConfig;
    public float Delta;

    public float MouseX;
    public float MouseY;
    public float SmoothX;
    public float SmoothY;
    public float SmoothXVelocity;
    public float SmoothYVeloctity;
    static public float LookAngle;
    public float TitlAngle;

    private void FixedUpdate()
    {
        FixedTick();
    }

    private void FixedTick()
    {
        Delta = Time.deltaTime;

        HandlePosition();
        HandleRotation();

        Vector3 targetPosition = Vector3.Lerp(MTransform.position, Character.position, 1);
        MTransform.position = targetPosition;
    }

    public void HandlePosition()
    {
        float targetX = CameraConfig.NormalX;
        float targetY = CameraConfig.NormalY;
        float targetZ = CameraConfig.NormalZ;

        if(CharacterStatus.IsAiming)
        {
            targetX = CameraConfig.AimX;
            targetZ = CameraConfig.AimZ;
        }

        Vector3 newPivotPosition = Pivot.localPosition;
        newPivotPosition.x = targetX;
        newPivotPosition.y = targetY;

        Vector3 newCameraPosition = CamTransform.localPosition;
        newCameraPosition.z = targetZ;

        float t = Delta * CameraConfig.PivotSpeed;
        Pivot.localPosition = Vector3.Lerp(Pivot.localPosition, newPivotPosition, t);
        CamTransform.localPosition = Vector3.Lerp(CamTransform.localPosition, newCameraPosition, t);
    }

    public void HandleRotation()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        if(CameraConfig.TurnSmooth > 0)
        {
            SmoothX = Mathf.SmoothDamp(SmoothX, MouseX, ref SmoothXVelocity, CameraConfig.TurnSmooth);
            SmoothY = Mathf.SmoothDamp(SmoothY, MouseY, ref SmoothXVelocity, CameraConfig.TurnSmooth);
        }
        else 
        {
            SmoothX = MouseX;
            SmoothY = MouseY;
        }

        LookAngle += SmoothX * CameraConfig.YRotSpeed;
        Quaternion targetRotation = Quaternion.Euler(0, LookAngle, 0);
        MTransform.rotation = targetRotation;

        TitlAngle -= SmoothY * CameraConfig.YRotSpeed;
        TitlAngle = Mathf.Clamp(TitlAngle, CameraConfig.MinAngle, CameraConfig.MaxAngle);
        Pivot.localRotation = Quaternion.Euler(TitlAngle, 0, 0);
     }
}
