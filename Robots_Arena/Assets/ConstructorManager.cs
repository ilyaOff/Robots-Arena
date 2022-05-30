using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructorManager : MonoBehaviour
{
    public Camera camera = null;
    [Range(1, 15)]
    public float maxDistanceRay = 10f;

    public Button button;//Кнопка добавления детали (исправить)

    [Range(0, 90)]
    public float speedRotate = 30;    

    Parts tmpDetail = null;
    Vector3 oldPointPlaceDetail = Vector3.zero;//Problems??
    //Добавить модифицированный режим, в котором будет использоваться
    //[Range(0.005f, 0.4f)]
    //public float changePositionDistance = 0.1f;
    public void StartPlacingBilding(Parts prefab)
    {
        if (tmpDetail != null)
            Destroy(tmpDetail);

        tmpDetail = Instantiate(prefab);
        tmpDetail.SetTransparent();
        tmpDetail.gameObject.GetComponent<Collider>().enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            button.onClick.Invoke();
        }
    }

    private void KeyBoardRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            DetailRotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            DetailRotate(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.R))
        {
            DetailRotate(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            DetailRotate(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.T))
        {
            DetailRotate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.G))
        {
            DetailRotate(0, 0, -1);
        }
    }

    void FixedUpdate()
    {
        KeyBoardRotation();
        if (tmpDetail is null) return;//не выбрана деталь

        Transform connectionPoint = tmpDetail.connectionPoints[0];
        Vector3 shift = -connectionPoint.position + tmpDetail.transform.position;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);         
        
       
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceRay))
        {
            Parts parts = hit.transform.GetComponent<Parts>();
            if (parts is null)
            {
                tmpDetail.transform.position = hit.point + shift;
                return;
            }

            ChangePlaceDetail(connectionPoint, hit, shift);


            //DetailRotate(0,, 0);
            if (Input.GetMouseButtonDown(0))
            {
                tmpDetail.SetNormal();
                tmpDetail.gameObject.GetComponent<Collider>().enabled = true;

                tmpDetail = null;
            }
        }
        else
        {
            Vector3 position = ray.origin + ray.direction * maxDistanceRay;
            tmpDetail.transform.position = position + shift;
        }
    }
    //Рассчёт точки, куда надо поставить деталь
    private bool ChangePlaceDetail(Transform connectionPoint, RaycastHit hit, Vector3 shift)
    {        
        //Точка пересечения луча + позиция точки привязки в префабе            
        Vector3 position = hit.point;

        //Оптимизировать при медленной работе
        //if(Vector3.Distance(oldPointPlaceDetail, position) < changePositionDistance)
        if (oldPointPlaceDetail.Equals( position) )
        {
            return false;
        }
        tmpDetail.transform.position = position + shift;
        oldPointPlaceDetail = position;

        //рассчёт поворота детали
        tmpDetail.transform.rotation =
            Quaternion.FromToRotation(connectionPoint.up, hit.normal)
             * tmpDetail.transform.rotation;

        return true;
    }

    void DetailRotate(float xAngle, float yAngle, float zAngle)
    {
        if (tmpDetail is null) return;

        float xRad = xAngle * speedRotate * Mathf.Deg2Rad;
        float yRad = yAngle * speedRotate * Mathf.Deg2Rad;
        float zRad = zAngle * speedRotate * Mathf.Deg2Rad;

        Transform connectionPoint = tmpDetail.connectionPoints[0];
        Vector3 yAxis = connectionPoint.up;
        //test, delete this
        testNormalStart = tmpDetail.transform.position;
        testNormal = yAxis;
        
        tmpDetail.transform.rotation = Quaternion.AngleAxis(yRad, yAxis)
                                        * tmpDetail.transform.rotation;

        Vector3 xAxis = Vector3.up;

        tmpDetail.transform.RotateAround(connectionPoint.position,  xAxis, xRad);
        Vector3 zAxis = Vector3.Cross(yAxis, xAxis);
        if (zAxis == Vector3.zero)
            zAxis = Vector3.forward;
        tmpDetail.transform.RotateAround(connectionPoint.position, zAxis, zRad);
    }

    private Vector3 testNormal = Vector3.up;
    private Vector3 testNormalStart = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testNormalStart, testNormalStart + 3*testNormal);
    }
}
