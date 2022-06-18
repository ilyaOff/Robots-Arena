﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeJointController
{
    private HingeJoint joint;

    public float MinAngle => joint.limits.min;
    public float MaxAngle => joint.limits.max;
    public float JointAngle
    {
        get
        {
            return joint.spring.targetPosition;
        }
        set
        {
            JointSpring spring = joint.spring;
            spring.targetPosition = Mathf.Clamp(value, MinAngle, MaxAngle);
            joint.spring = spring;
        }
    }

    public float NormalizeJointAngle
    {
        get
        { 
            return (JointAngle - MinAngle) / (MaxAngle - MinAngle);
        }
        set
        {
            JointAngle = Mathf.Clamp(value, 0, 1) * (MaxAngle - MinAngle) + MinAngle;
        }
    }

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startTarget;

    public HingeJointController(HingeJoint joint)
    {
        this.joint = joint;

        startTarget = joint.spring.targetPosition;

        Transform transform = joint.transform;
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    public void Restart()
    {
        Transform transform = joint.transform;

        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        JointSpring spring = joint.spring;
        spring.targetPosition =  startTarget;
        joint.spring = spring;
    }
}