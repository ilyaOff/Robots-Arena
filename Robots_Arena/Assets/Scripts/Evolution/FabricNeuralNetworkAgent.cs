using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricNeuralNetworkAgent : MonoBehaviour
{    
    [SerializeField] private LegController prefab;
    public INeuralNetworkAgent Create(int number, Transform target = null)
    {
        LegController robot = Instantiate(prefab);

        if (target is null)
        {
            robot.Initialize(new Navigator());
        }
        else
        {
            NavigatorToTarget navigator = robot.gameObject.AddComponent<NavigatorToTarget>();
            navigator.TryChangeTarget(target);
            robot.Initialize(navigator);
        }
        robot.name += number;

        return (INeuralNetworkAgent)robot;
    }
}
