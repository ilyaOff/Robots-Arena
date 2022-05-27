using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorManager : MonoBehaviour
{
    public Camera camera = null;
    [Range(1, 15)]
    public float maxDistanceRay = 10f;

    public GameObject[] details;
    GameObject tmpDetail = null;
    public int detailNumber = -1; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //заглушка
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            detailNumber = 0;
            if (tmpDetail != null)
                Destroy(tmpDetail);
            tmpDetail = Instantiate(details[detailNumber]);
            tmpDetail.GetComponent<Collider>().enabled = false;
            tmpDetail.GetComponent<Parts>().IsSelected = true;
            //Чтобы передать материал по ссылке, используется свойство gameObject
            Material material = tmpDetail.gameObject.GetComponent<Renderer>().material;
            material.color = new Color(material.color.r,
                                        material.color.g,
                                        material.color.b,
                                        material.color.a/2);
            //tmpDetail.GetComponent<Renderer>().material = material;
        }
            
    }
    void FixedUpdate()
    {
        if (details.Length < 1) return;
        if (detailNumber < 0) return;//не выбрана деталь
        
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 position = ray.origin + ray.direction* maxDistanceRay/2;
        tmpDetail.transform.position = position;
        if (Physics.Raycast(ray, out hit, maxDistanceRay))
        {
            position = hit.point + details[detailNumber].GetComponent<Parts>().connectionPoints[0].position;
            Parts parts = hit.transform.GetComponent<Parts>();
            if (parts is null)
            {                
                tmpDetail.transform.position = position;
                return;
            }

            //Рассчёт точки, куда надо поставить деталь
            //Точка пересечения луча + позиция точки привязки в префабе
            Transform connectionPoint = tmpDetail.GetComponent<Parts>().connectionPoints[0];

            //GameObject newDetails = Instantiate(details[0]);

            //рассчёт поворота детали
            tmpDetail.transform.rotation = Quaternion.Euler(0, 0, 0);
            tmpDetail.transform.rotation *=
                Quaternion.FromToRotation(connectionPoint.up, hit.normal);

            position = hit.point 
                + tmpDetail.GetComponent<Parts>().connectionPoints[0].position
                - tmpDetail.transform.position;

            tmpDetail.transform.position = position;


            //test, delete this
            testNormalStart = hit.point;
            testNormal = tmpDetail.transform.position - hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                detailNumber = -1;

                Material material = tmpDetail.gameObject.GetComponent<Renderer>().material;
                material.color = new Color(material.color.r,
                                        material.color.g,
                                        material.color.b,
                                         material.color.a * 2);

                tmpDetail.GetComponent<Collider>().enabled = true;
                tmpDetail.GetComponent<Parts>().IsSelected = false;
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
