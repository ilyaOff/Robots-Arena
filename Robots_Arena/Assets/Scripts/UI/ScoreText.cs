using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text chempion;

    [SerializeField] private List<EvolutionRoom> rooms;

    private void Start()
    {
       Invoke(nameof(Initialization), 1f);
    }

    private void Initialization()
    {
        rooms = new List<EvolutionRoom>();
        rooms.AddRange(FindObjectsOfType<EvolutionRoom>());
    }

    // Update is called once per frame
    void Update()
    {
        if (rooms.Count < 1) return;

        float max = rooms[0].Score;
        int k = 0;
        for(int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].Score > max)
            {
                max = rooms[i].Score;
                k = i;
            }
            
        }
        score.text = $"Score: {max}";
        chempion.text = $"Champion: {k}";
    }
}
