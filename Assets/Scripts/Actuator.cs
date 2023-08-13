using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuator : MonoBehaviour
{
    [SerializeField]
    TransformationType transformationType;

    [SerializeField]
    AxisType axisType;

    [SerializeField]
    AnimationCurve displacementCurve;

    [SerializeField]
    float displacementPower;

    [SerializeField]
    float duration;

    bool _isActivated;
    float _activationTime;
    float _initialDisplacement;

    // Update is called once per frame
    void Update()
    {
        if (_isActivated)
        {
            float elapsedTime = Time.time - _activationTime;
            float delta = Mathf.Clamp01(elapsedTime / duration);
            float displacement = displacementCurve.Evaluate(delta) * displacementPower;
            TRANSFORMATION_ACTION[GetTransformationIndex(transformationType, axisType)]
                (transform, _initialDisplacement + displacement);
            _isActivated = elapsedTime < duration;
        }
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        _isActivated = true;
        _activationTime = Time.time;
        CacheInitialDisplacement();
    }

    void CacheInitialDisplacement()
        => _initialDisplacement = TRANSFORMATION_CACHE[GetTransformationIndex(transformationType, axisType)](transform);
    
    int GetTransformationIndex(TransformationType transformation, AxisType axis)
        => ((int)transformationType * 3) + (int)axisType;

    static Action<Transform, float>[] TRANSFORMATION_ACTION =
    {
        // Position Transformations
        (transform, displacement) =>
            { transform.localPosition = new(displacement, transform.localPosition.y, transform.localPosition.z); },
        (transform, displacement) =>
            { transform.localPosition = new(transform.localPosition.x, displacement, transform.localPosition.z); },
        (transform, displacement) =>
            { transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, displacement); },
        // Rotation Transformations
        (transform, displacement) =>
            { transform.localRotation = Quaternion.Euler(new(displacement, transform.localEulerAngles.y, transform.localEulerAngles.z)); },
        (transform, displacement) =>
            { transform.localRotation = Quaternion.Euler(new(transform.localEulerAngles.x, displacement, transform.localEulerAngles.z)); },
        (transform, displacement) =>
            { transform.localRotation = Quaternion.Euler(new(transform.localEulerAngles.x, transform.localEulerAngles.y, displacement)); },
        // Scale Transformations
        (transform, displacement) =>
            { transform.localScale = new(displacement, transform.localScale.y, transform.localScale.z); },
        (transform, displacement) =>
            { transform.localScale = new(transform.localScale.x, displacement, transform.localScale.z); },
        (transform, displacement) =>
            { transform.localScale = new(transform.localScale.x, transform.localScale.y, displacement); },
    };

    static Func<Transform, float>[] TRANSFORMATION_CACHE =
    {
        // Position Caching
        transform => transform.position.x,
        transform => transform.position.y,
        transform => transform.position.z,
        // Rotation Caching
        transform => transform.rotation.x,
        transform => transform.rotation.y,
        transform => transform.rotation.z,
        // Scale Caching
        transform => transform.localScale.x,
        transform => transform.localScale.y,
        transform => transform.localScale.z,
    };
}

public enum TransformationType
{
    Location,
    Rotation,
    Scale
}

public enum AxisType
{
    X,
    Y,
    Z
}

