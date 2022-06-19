using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LegController : MonoBehaviour, INeuralNetworkAgent
{
    [SerializeField] private List<Leg> legs;
    public int CountLegs => legs.Count;
    
    private Rigidbody body;

    public NeuralNetwork Brain { get; private set; }   
    private float[] inputBrain;
    private bool usedBrain = false;

    private Navigator _navigator;
    private void Awake()
    {
        body = this.GetComponent<Rigidbody>();
        legs = this.gameObject.GetComponentsInChildren<Leg>().ToList();
        if (legs is null || legs.Count == 0)
        {
            Debug.LogError("Legs is empty!");
            Destroy(this);
            return;
        }

        foreach (Leg leg in legs)
        {
            leg.AttachToBody(body);
        }
    }

    public void Initialize(Navigator navigator)
    {
        if (navigator is null)
            throw new System.ArgumentNullException("navigator");

        _navigator = navigator;
    }

    public void NewBrain(NeuralNetwork brain)
    {
        Brain = brain;
        inputBrain = new float[brain.Inputs];

        InitialPosition();

        usedBrain = true;
        body.isKinematic = false;
    }

    private void InitialPosition()
    {
        usedBrain = false;
        body.isKinematic = true;
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].Restart();
        }
    }
        
    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        if (!usedBrain) return;

        float[] calculateAngle = CalculateBrain();
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].NormalizeVerticalAngle += CalculateAngle(calculateAngle[3 * i]);
            legs[i].NormalizeHipAngle += CalculateAngle(calculateAngle[3 * i + 1]);
            legs[i].NormalizeKneeAngle += CalculateAngle(calculateAngle[3 * i + 2]);
        }
    }

    private float CalculateAngle(float output)
    {
        float threshold = 0.25f;
        int result = Mathf.RoundToInt(output/ threshold);

        return  result*Time.fixedDeltaTime;        
    }

    private float[] CalculateBrain()
    {
        int shift = 0;
        for (int i = 0; i < legs.Count; i++)
        {
            inputBrain[3 * i] = legs[i].NormalizeVerticalAngle;
            inputBrain[3 * i + 1] = legs[i].NormalizeHipAngle;
            inputBrain[3 * i + 2] = legs[i].NormalizeKneeAngle;
        }

        shift = 3 * legs.Count;

        Vector3 direction = _navigator.Direction();            
        
        inputBrain[0 + shift] = direction.x;
        inputBrain[1 + shift] = direction.y;
        inputBrain[2 + shift] = direction.z;
        inputBrain[3 + shift] = direction.magnitude;

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 directionProject = Vector3.ProjectOnPlane(direction, Vector3.up);
        inputBrain[4 + shift] = Vector3.Angle(forward, directionProject);

        directionProject = Vector3.ProjectOnPlane(direction, transform.right);
        inputBrain[5 + shift] = Vector3.Angle(transform.up, directionProject);
                

        return Brain.CalculeteOutput(inputBrain);
    }    
}
