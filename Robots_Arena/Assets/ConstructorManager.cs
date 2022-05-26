using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorManager : MonoBehaviour
{
    public Camera camera = null;
    [Range(1, 15)]
    public float maxDistanceRay = 10f;

    public GameObject[] details;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (details.Length < 1) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistanceRay))
            {
                Parts parts = hit.transform.GetComponent<Parts>();
                if (parts is null) return;

                //Debug.Log(details[0].GetComponent<Parts>().connectionPoints[0].position);
                //Рассчёт точки, куда надо поставить деталь
                //Точка пересечения луча + позиция точки привязки в префабе
                Transform connectionPoint = details[0].GetComponent<Parts>().connectionPoints[0];
                Vector3 position = hit.point
                    ;// + connectionPoint.position;
                GameObject newDetails = Instantiate(details[0]);
                //рассчёт поворота детали

                newDetails.transform.rotation *=
                    Quaternion.FromToRotation(connectionPoint.up, hit.normal);
                newDetails.transform.position = hit.point +
                    newDetails.GetComponent<Parts>().connectionPoints[0].position;

                testNormalStart = hit.point;
                testNormal = 3*(newDetails.transform.position - hit.point);
            }
        }
    }
    
    private Vector3 testNormal = Vector3.up;
    private Vector3 testNormalStart = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testNormalStart, testNormalStart + testNormal);
    }
}
