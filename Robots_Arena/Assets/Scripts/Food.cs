using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour
{
    [SerializeField] private Collider knee;
    [SerializeField] private Collider food;
    [SerializeField] private bool _inGround = false;
    public bool InGround => _inGround;// { get; private set; }

    private void Start()
    {
        //Physics.IgnoreCollision(knee, food);
    }

    private void OnTriggerStay(Collider other)
    {       
        _inGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _inGround = false;
    }

}
