using UnityEngine;

public class HingeJointController : MonoBehaviour
{
    [SerializeField] private Transform _connect;
    [SerializeField] private Vector3 _axis = Vector3.forward;
    [SerializeField] private Vector3 _correctAxis = Vector3.zero;

    [SerializeField] private float _minAngle = 0f;
    [SerializeField] private float _maxAngle = 0f;
    public float MinAngle => _minAngle ;
    public float MaxAngle => _maxAngle;

    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float _angle = 0f;
    public float JointAngle 
    { 
        get => _angle;
        set
        {
            _angle = Mathf.Clamp(value, MinAngle, MaxAngle);
            transform.Rotate(_connect.position, _angle);
        }      
    }

    public float NormalizeJointAngle
    {
        get => (MaxAngle == MinAngle) ? 1 : (JointAngle - MinAngle) / (MaxAngle - MinAngle);        
        set => JointAngle = value * (MaxAngle - MinAngle) + MinAngle;
    }

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 start = _correctAxis + _connect.position;
        Vector3 end = start + _axis*0.4f;
        Gizmos.DrawLine(start, end);
    }
    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        if (MinAngle > MaxAngle)
        {
            float swap = _minAngle;
            _minAngle = _maxAngle;
            _maxAngle = swap;
            Debug.LogWarning($"{name}: Min and Max angle incorrect, used swap");
        }
    }

    public void Restart()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        JointAngle =  startAngle;
    }

}
