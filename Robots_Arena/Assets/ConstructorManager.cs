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

    public Button button;
    // Start is called before the first frame update

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
        //заглушка
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            button.onClick.Invoke();
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
            
            position = hit.point
                + connectionPoint.position - tmpDetail.transform.position;

            tmpDetail.transform.position = position;

            //рассчёт поворота детали
            tmpDetail.transform.rotation = Quaternion.identity;
            tmpDetail.transform.rotation *=
                Quaternion.FromToRotation(connectionPoint.up, hit.normal);

            //test, delete this
            testNormalStart = hit.point;
            testNormal = tmpDetail.transform.position - hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                tmpDetail.SetNormal();
                tmpDetail.gameObject.GetComponent<Collider>().enabled = true;  
                
                tmpDetail = null;
            }
        }
    }
    
    private Vector3 testNormal = Vector3.up;
    private Vector3 testNormalStart = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testNormalStart, testNormalStart + 3*testNormal);
    }
}
