using UnityEngine;

public class Leg : MonoBehaviour
{
    [SerializeField]
    private HingeJoint horizontalJoint, verticalJoint, kneeJoint;

    public float HorizontalTargetPosition => horizontalJoint.spring.targetPosition;
    public float VerticalTargetPosition => verticalJoint.spring.targetPosition;
    public float KneeTargetPosition => kneeJoint.spring.targetPosition;

    float time = 0f;
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        JointSpring spring = kneeJoint.spring;
        spring.targetPosition = (1 + Mathf.Sin(time)) * (kneeJoint.limits.max - kneeJoint.limits.min)/2
                                + kneeJoint.limits.min;
        kneeJoint.spring = spring;
    }
}
