using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCam : MonoBehaviour {

    [SerializeField]
    private float mouseSensitivity;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float dstFromTarget;
    [SerializeField]
    private Animator characterAnimator;
    
    private Vector2 pitchMinMax = new Vector2(-40, 85);    
    private float rotationSmoothTime;
    private Vector3 targetPosAiming = new Vector3(0.511f, 1.387f, 0.598f);
    private Vector3 targetPosDefault;
    //time to take from start to end
    private float lerpTime = 0.3f;
    //this will update the lerp time
    private float currentLerpTime = 0;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start()
    {
        rotationSmoothTime = 0.05f;
        dstFromTarget = 1.2f;
        mouseSensitivity = 8f;
        targetPosDefault = target.localPosition;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstFromTarget;

        if(Input.GetMouseButton(1))
        {
            Aiming(targetPosDefault);
            pitchMinMax = new Vector2(-30, 50);
        }
        else
        {
            pitchMinMax = new Vector2(-40, 85);
            currentLerpTime -= Time.deltaTime;
            if (currentLerpTime <= 0)
            {
                currentLerpTime = 0;
            }
            float lerpPerc = currentLerpTime / lerpTime;
            target.localPosition = Vector3.Lerp(targetPosDefault, targetPosAiming, lerpPerc);
        }

    }

    private void Aiming(Vector3 targetPosDefault)
    {
        currentLerpTime += Time.deltaTime;
        if(currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        float lerpPerc = currentLerpTime / lerpTime;
        target.localPosition = Vector3.Lerp(targetPosDefault, targetPosAiming, lerpPerc);

        //animation
        float animationSpeedPercent = currentRotation.x;
        characterAnimator.SetFloat("aimingPercent", animationSpeedPercent, 0.01f, Time.deltaTime);
    }


}
