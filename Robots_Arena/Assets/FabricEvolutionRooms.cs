using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricEvolutionRooms : MonoBehaviour
{
    [SerializeField] private LegController prefab;
    [SerializeField] private TargetRobots targetPrefab;
    [SerializeField] private EvolutionRoom roomPrefab;

    [SerializeField] private float distanceBetweenRooms = 10;

    public List<EvolutionRoom> Create(int Quantity)
    {
        if (Quantity <= 0)
            throw new System.ArgumentException("Zero EvolutionRoom create");

        List<EvolutionRoom> rooms = new List<EvolutionRoom>(Quantity);
        int wallLengthForward = Mathf.FloorToInt(Mathf.Sqrt(Quantity));

        for (int i = 0; i < Quantity; i++)
        {
            EvolutionRoom room = Instantiate(roomPrefab);
            room.transform.position = Position(i, wallLengthForward, distanceBetweenRooms);
            rooms.Add(room);
        }
        for (int i = 0; i < Quantity; i++)
        { 
            LegController robot = Instantiate(prefab);
            EvolutionScorer agent = robot.gameObject.AddComponent<EvolutionScorer>();

            TargetRobots target = Instantiate(targetPrefab);

            rooms[i].Initialize(agent, target);
        }

        return rooms;
    }

    private Vector3 Position(int i, int wall, float size)
    {
        return new Vector3(0,0, 2)
                            + size * (Vector3.forward * (i % wall)
                                    + Vector3.right * (i / wall));
    }
}
