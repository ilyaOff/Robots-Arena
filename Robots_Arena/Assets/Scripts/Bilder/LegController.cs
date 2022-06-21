using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class LegController : MonoBehaviour//, INeuralNetworkAgent
{
    [SerializeField] protected List<Leg> legs;
    public int CountLegs => legs.Count;
    
    private Rigidbody body;

    protected Navigator _navigator;
    protected void Awake()
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

    protected void InitialPosition()
    {
        body.isKinematic = true;
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].Restart();
        }
        body.isKinematic = false;
    }
        
    protected void FixedUpdate()
    {
        Moving();
    }

    protected abstract void Moving();
    
}
