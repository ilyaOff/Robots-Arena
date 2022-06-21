using UnityEngine;

public class Leg : MonoBehaviour
{
    [SerializeField] private Transform connect;

    [SerializeField] private Food food;
    public bool InGround => food.InGround;

    [SerializeField]
    private HingeJointController vertical, hip, knee;
    public float VerticalAngle
    {
        get => vertical.JointAngle;
        set => vertical.JointAngle = value;
    }
    public float NormalizeVerticalAngle
    {
        get => vertical.NormalizeJointAngle;
        set => vertical.NormalizeJointAngle = value;
    }

    public float HiplAngle
    {
        get => hip.JointAngle;
        set => hip.JointAngle = value;
    }
    public float NormalizeHipAngle
    {
        get => hip.NormalizeJointAngle;
        set => hip.NormalizeJointAngle = value;
    }

    public float KneeAngle
    {
        get => knee.JointAngle;
        set => knee.JointAngle = value;
    }
    public float NormalizeKneeAngle
    {
        get => knee.NormalizeJointAngle;
        set => knee.NormalizeJointAngle = value;
    }
    
    public void AttachToBody(Rigidbody body)
    {
        if (body is null)
            throw new System.ArgumentNullException("Body must be not null");

        connect.parent = body.transform;
    }

    public void Restart()
    {
        vertical.Restart();
        hip.Restart();
        knee.Restart();
    }
}
