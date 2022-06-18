using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCircularSymmetricalInstaller", menuName = "Installer/CircularSymmetricalInstaller")]

public abstract class Chalange : ScriptableObject
{
    [SerializeField] protected float reward = 0.001f;
    public float Score { get; private set; }
    
    public void Start()
    {
        Score = 0;
        Initialize();
    }

    protected virtual void Initialize() { }

    public abstract void Update();

}
