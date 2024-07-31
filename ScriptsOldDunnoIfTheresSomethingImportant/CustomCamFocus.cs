using ModWobblyLife;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CustomCamFocus : ModCameraFocus
{
    public const float MaxDistance = 6f;

    public const float MinDistance = 2f;

    public const float Lerp = 10f;

    public const float DistanceLerp = 3f;

    public const float LerpHeightFactor = 1f;

    public const float SmoothTime = 0.125f;

    public const float MaxDistanceBeforeSnap = 20f;

    public const float XCameraRotationOffset = 10f;

    public const float XRotationClamp = 85f;

    public const float XRotationCamClamp = 50f;

    [SerializeField]
    public Transform focusTransform;

    public PlayerCharacter playerCharacter;

    public HawkTransformSync transformSync;

    public Vector3 rotationAxis;

    public Vector3 currentVelocity;

    public Quaternion currentRotVelocity;

    public bool bInitialRotation;

    public float distance = 0f;

    public float lerpMul = 1f;

    public float currentLerpMul = 1f;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        transformSync = GetComponent<HawkTransformSync>();
        HawkTransformSync hawkTransformSync = transformSync;
        hawkTransformSync.onRecievedInitial = (Action)Delegate.Combine(hawkTransformSync.onRecievedInitial, new Action(OnRecievedInital));
        if (!playerCharacter)
        {
            Debug.LogError(typeof(CameraFocusPlayerCharacter)?.ToString() + " requires " + typeof(PlayerCharacter)?.ToString() + " component");
        }
    }

    private void OnRecievedInital()
    {
        if ((bool)focusTransform)
        {
            rotationAxis.y = focusTransform.eulerAngles.y;
        }

        bInitialRotation = true;
    }

    protected override void OnUnfocusCamera(ModPlayerController playerController)
    {
        
    }

    protected override void OnFocusCamera(ModPlayerController playerController)
    {
        if (modNetworkObject != null && modNetworkObject.IsServer())
        {
            OnRecievedInital();
        }
    }

    protected bool HandleCollision(GameplayCamera camera, Vector3 targetPosition, out float distance, ref Vector3 targetCamPosition, bool bIgnoreGameObjectLayer = false)
    {
        int num = camera.GetCollisionLayerMask();
        if (bIgnoreGameObjectLayer)
        {
            num &= ~(1 << base.gameObject.layer);
        }

        bool result = false;
        distance = 0f;
        if (Physics.Linecast(targetPosition, targetCamPosition, out var hitInfo, num, QueryTriggerInteraction.Ignore))
        {
            Vector3 vector = hitInfo.normal * 0.51f;
            Vector3 vector2 = hitInfo.point + vector;
            distance = hitInfo.distance;
            targetCamPosition = vector2;
            result = true;
        }

        Camera camera2 = camera.GetCamera();
        Vector3 position = camera2.transform.position;
        camera2.transform.position = targetCamPosition;
        Vector3 vector3 = camera2.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera2.nearClipPlane));
        Vector3 end = camera2.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, camera2.nearClipPlane));
        if (Physics.Linecast(vector3, end, out hitInfo, num, QueryTriggerInteraction.Ignore))
        {
            Vector3 vector4 = (vector3 - hitInfo.point).normalized * 0.3f;
            targetCamPosition += vector4;
            camera2.transform.position = targetCamPosition;
            vector3 = camera2.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera2.nearClipPlane));
            result = true;
        }

        Vector3 end2 = camera2.ViewportToWorldPoint(new Vector3(1.1f, -0.1f, camera2.nearClipPlane));
        if (Physics.Linecast(vector3, end2, out hitInfo, num, QueryTriggerInteraction.Ignore))
        {
            Vector3 vector5 = (vector3 - hitInfo.point).normalized * 0.3f;
            targetCamPosition += vector5;
            camera2.transform.position = targetCamPosition;
            vector3 = camera2.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera2.nearClipPlane));
            result = true;
        }

        Vector3 end3 = camera2.ViewportToWorldPoint(new Vector3(-0.1f, 1.1f, camera2.nearClipPlane));
        if (Physics.Linecast(vector3, end3, out hitInfo, num, QueryTriggerInteraction.Ignore))
        {
            Vector3 vector6 = (vector3 - hitInfo.point).normalized * 0.3f;
            targetCamPosition += vector6;
            camera2.transform.position = targetCamPosition;
            vector3 = camera2.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera2.nearClipPlane));
            result = true;
        }

        Vector3 end4 = camera2.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, camera2.nearClipPlane));
        if (Physics.Linecast(vector3, end4, out hitInfo, num, QueryTriggerInteraction.Ignore))
        {
            Vector3 vector7 = (vector3 - hitInfo.point).normalized * 0.3f;
            targetCamPosition += vector7;
            result = true;
        }

        camera2.transform.position = position;
        return result;
    }

    public override void UpdateCamera(ModGameplayCamera camera, Transform cameraTransform)
    {
        if (!focusTransform || !transformSync.HasRecievedInitialWithDelay())
        {
            return;
        }
        GameplayCamera _camera = camera.GetComponent<GameplayCamera>();

        float num = _camera.GetAxisDeltaX() * 5f;
        float num2 = _camera.GetAxisDeltaY() * 5f;
        rotationAxis.x = Mathf.Clamp(rotationAxis.x - num2, -85f, 85f);
        rotationAxis.y += num;
        rotationAxis.y = HawkMathUtils.Clamp0360(rotationAxis.y);
        float num3 = Mathf.Clamp(rotationAxis.x + 10f, -50f, 50f);
        float num4 = Mathf.Abs(num3) / 50f;
        float num5 = Mathf.Lerp(6f, 2f, num4);
        Quaternion quaternion = Quaternion.AngleAxis(num3, Vector3.right);
        Quaternion quaternion2 = Quaternion.AngleAxis(rotationAxis.y, Vector3.up);
        Vector3 vector = Vector3.forward * distance;
        Vector3 targetCamPosition = focusTransform.position - quaternion2 * (quaternion * vector);
        targetCamPosition += Vector3.up * (1f - num4) * 1f;
        Quaternion quaternion3 = Quaternion.LookRotation(focusTransform.position - targetCamPosition, Vector3.up);
        if (HandleCollision(_camera, focusTransform.position, out var num6, ref targetCamPosition))
        {
            float num7 = Mathf.Clamp(num6 + 1.5f, 2f, 6f);
            if (num7 < num5)
            {
                distance = Mathf.Lerp(distance, num7, Time.deltaTime * 10f);
            }
            else
            {
                distance = Mathf.MoveTowards(distance, num5, Time.deltaTime * 3f);
            }
        }
        else
        {
            distance = Mathf.MoveTowards(distance, num5, Time.deltaTime * 3f);
        }

        float num8 = Vector3.Distance(focusTransform.position, camera.transform.position);
        currentLerpMul = Mathf.Lerp(currentLerpMul, lerpMul, Time.deltaTime * 3f);
        if (num8 < 20f)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetCamPosition, Time.deltaTime * 10f * currentLerpMul);
            camera.transform.rotation = UnityExtensions.SmoothDamp(camera.transform.rotation, quaternion3, ref currentRotVelocity, 0.125f, Time.deltaTime);
        }
        else
        {
            camera.transform.SetPositionAndRotation(targetCamPosition, quaternion3);
        }
    }

    public void SetLerpMul(float lerpMul)
    {
        this.lerpMul = lerpMul;
    }

    public void ResetLerpMul()
    {
        lerpMul = 1f;
    }

    public Vector3 GetRotationAxis()
    {
        return rotationAxis;
    }

    public void SetRotationAxis(Vector3 axis)
    {
        rotationAxis = axis;
    }

    public bool HasDoneInitialRotation()
    {
        return bInitialRotation;
    }

    public Transform GetFocusTransform()
    {
        return focusTransform;
    }
}
