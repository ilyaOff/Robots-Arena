using UnityEngine;

public class Leg : MonoBehaviour
{
    [SerializeField]
    private HingeJoint verticalJoint, hipJoint, kneeJoint;

    [SerializeField]
    private Rigidbody connection;

    private HingeJointController vertical, hip, knee;

    [SerializeField] private Food food;
    public bool InGround => food.InGround;
    public Vector3 Vertical => verticalJoint.transform.localPosition;
    public Vector3 Hip => hipJoint.transform.localPosition;
    public Vector3 Knee => kneeJoint.transform.localPosition;

    public float VerticalAngle
    {
        get
        {
            return vertical.JointAngle;
        }
        set
        {
            vertical.JointAngle = value;
        }
    }
    public float NormalizeVerticalAngle
    {
        get
        {
            return vertical.NormalizeJointAngle;
        }
        set
        {
            vertical.NormalizeJointAngle = value;
        }
    }

    public float HipAngle
    {
        get
        {
            return hip.JointAngle;
        }
        set
        {
            hip.JointAngle = value;
        }
    }
    public float NormalizeHipAngle
    {
        get
        {
            return hip.NormalizeJointAngle;
        }
        set
        {
            hip.NormalizeJointAngle = value;
        }
    }

    public float KneeAngle
    {
        get
        {
            return knee.JointAngle;
        }
        set
        {
            knee.JointAngle = value;
        }
    }
    public float NormalizeKneeAngle
    {
        get
        {
            return knee.NormalizeJointAngle;
        }
        set
        {
            knee.NormalizeJointAngle = value;
        }
    }

    private void Awake()
    {
        vertical = new HingeJointController(verticalJoint);
        hip = new HingeJointController(hipJoint);
        knee = new HingeJointController(kneeJoint);
    }

    public void AttachToBody(Rigidbody body)
    {
        if (body is null)
            throw new System.ArgumentNullException("Body must be not null");

        verticalJoint.connectedBody = body;
        if (connection != null)
        {
            body.mass += connection.mass;
            Destroy(connection);
        }
    }

    public void Restart()
    {
        vertical.Restart();
        hip.Restart();
        knee.Restart();
    }
}
