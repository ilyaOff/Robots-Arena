using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LegController : MonoBehaviour
{
    [SerializeField] private List<Leg> legs;

    private Rigidbody body;
    [SerializeField] private float[] startAngle;
    private void Start()
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


        startAngle = new float[legs.Count];
        for (int i = 0; i < startAngle.Length; i++)
        {
            startAngle[i] = Random.Range(0, 2*Mathf.PI);
        }
    }
    private void FixedUpdate()
    {     
        for(int i = 0; i < legs.Count; i++)
        {
            legs[i].HipAngle = 0f;
            legs[i].NormalizeKneeAngle = Mathf.Sin(Time.fixedTime + startAngle[i]);
        }
    }
}
