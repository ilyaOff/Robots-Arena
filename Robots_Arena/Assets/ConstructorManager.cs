using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructorManager : MonoBehaviour
{
    public Camera camera = null;
    [Range(1, 15)]
    public float maxDistanceRay = 10f;
    
    Parts tmpDetail = null;

    public Button button;//Кнопка добавления детали (исправить)
    Vector3 saveNormalPlane;
    public void StartPlacingBilding(Parts prefab)
    {
        if (tmpDetail != null)
            Destroy(tmpDetail);

        tmpDetail = Instantiate(prefab);
        tmpDetail.SetTransparent();
        tmpDetail.gameObject.GetComponent<Collider>().enabled = false;
        saveNormalPlane = tmpDetail.transform.up;
    }
    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            button.onClick.Invoke();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            DetailRotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            DetailRotate(0, 1, 0);
        }

    }
    void FixedUpdate()
    {
        if (tmpDetail is null) return;//не выбрана деталь

        Transform connectionPoint = tmpDetail.connectionPoints[0];

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);         
        Vector3 position = ray.origin + ray.direction* maxDistanceRay;
        tmpDetail.transform.position = position;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceRay))
        {
            position = hit.point;
            Parts parts = hit.transform.GetComponent<Parts>();
            if (parts is null)
            {                
                tmpDetail.transform.position = position;
                return;
            }

            //Рассчёт точки, куда надо поставить деталь
            //Точка пересечения луча + позиция точки привязки в префабе            
            position = hit.point + connectionPoint.position - tmpDetail.transform.position;
            tmpDetail.transform.position = position;

            //рассчёт поворота детали
            Vector3 right = Vector3.Project(connectionPoint.right, saveNormalPlane);

            tmpDetail.transform.rotation =
                Quaternion.FromToRotation(connectionPoint.right, right)
                 * tmpDetail.transform.rotation;

            tmpDetail.transform.rotation =
                Quaternion.FromToRotation(connectionPoint.up, hit.normal)
                 * tmpDetail.transform.rotation;

            
            //DetailRotate(0,, 0);
            if (Input.GetMouseButtonDown(0))
            {
                tmpDetail.SetNormal();
                tmpDetail.gameObject.GetComponent<Collider>().enabled = true;  
                
                tmpDetail = null;
            }
        }
    }

    void DetailRotate(float xAngle, float yAngle, float zAngle)
    {
        if (tmpDetail is null) return;

        Transform connectionPoint = tmpDetail.connectionPoints[0];
        Vector3 yAxis = connectionPoint.up;
        //Vector3 yAxis = tmpDetail.transform.up;
        //test, delete this
        testNormalStart = tmpDetail.transform.position;
        testNormal = yAxis;

        //tmpDetail.transform.rotation = Quaternion.identity;
        tmpDetail.transform.rotation = Quaternion.AngleAxis(yAngle, yAxis)* tmpDetail.transform.rotation;
        //tmpDetail.transform.Rotate(yAxis, yAngle);
        saveNormalPlane = tmpDetail.transform.up;
    }
    private Vector3 testNormal = Vector3.up;
    private Vector3 testNormalStart = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testNormalStart, testNormalStart + 3*testNormal);
    }
}
