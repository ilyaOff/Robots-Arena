using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public Transform[] connectionPoints;

    //[SerializeField]
    //private bool isSelectedTest = false;

    private bool isSelected = false;
    
    public bool IsSelected
    {
        get 
        { 
            return isSelected;
        }
        set 
        { 
            isSelected = value;
            if(connectionPoints.Length > 0)
                foreach (Transform point in connectionPoints)
                {
                    point.gameObject.SetActive(value);
                }
        }
    }

    /* private void OnDrawGizmos()
     {
         Gizmos.color = Color.green;
         if(pointConnection != null)
         Gizmos.DrawSphere(pointConnection.position, 0.1f);
     }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // IsSelected = isSelectedTest;
    }
}
