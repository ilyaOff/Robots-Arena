using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricNeuralNetworkAgent : MonoBehaviour
{    
    [SerializeField] private LegController prefab;
    public INeuralNetworkAgent Create(int number, Transform target = null)
    {
        LegController robot = Instantiate(prefab);

        Navigator navigator;
        if (target is null)
        {
            navigator = robot.gameObject.AddComponent<Navigator>();
        }
        else
        {
            navigator = robot.gameObject.AddComponent<NavigatorToTarget>();
            ((NavigatorToTarget)navigator).TryChangeTarget(target);
        }
        robot.Initialize(navigator);

        robot.name += number;

        return (INeuralNetworkAgent)robot;
    }
}
