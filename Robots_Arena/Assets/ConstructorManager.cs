using System;
using UnityEngine;
using UnityEngine.UI;

public class ConstructorManager : MonoBehaviour
{
    public Camera camera = null;
    [Range(1, 15)]
    public float maxDistanceRay = 10f;

    public Button button;//Кнопка добавления детали (исправить)

    Part tmpDetail = null;

    [SerializeField]
    private Installer placer;
    
    private Vector3 testDirection = Vector3.up;
    private Vector3 testFrom = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testFrom, testFrom + 3 * testDirection);
    }
    private void Start()
    {
        placer = (Installer)ScriptableObject.CreateInstance(nameof(SimpleInstaller));
        //placer = new SimpleInstaller();
    }
    public void StartPlacingPart(Part prefab)
    {
        Vector3 position = Vector3.zero;
        if (tmpDetail != null)
        {
            position = tmpDetail.transform.position;
            Destroy(tmpDetail.gameObject);
        }

        tmpDetail = placer.StartInstalling(prefab, position, Quaternion.identity);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            button.onClick.Invoke();
        }
    }

    void FixedUpdate()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Part parts = null;
        Vector3 position = ray.origin + ray.direction * maxDistanceRay;

        if (Physics.Raycast(ray, out hit, maxDistanceRay))
        {
            parts = hit.transform.GetComponentInParent<Part>();
            position = hit.point;
        }

        if (tmpDetail is null)
        {
            if (Input.GetMouseButtonDown(0))
                TryTakeDetail(parts);
        }
        else
        {
            placer.DetailRotate(tmpDetail, KeyBoardRotation());

            if (parts != null)
            {
                testFrom = parts.transform.position;
                testDirection = parts.transform.up;
            }
            
            placer.ChangePositionDetail(tmpDetail, parts, position, hit.normal);

            if (Input.GetMouseButtonDown(0))
            {
                if (placer.TryPlaceDetail(tmpDetail, parts))
                {
                    tmpDetail = null;
                }
            }
        }
    }

    bool TryTakeDetail(Part parts)
    {
        bool result = parts != null;

        if (result)
        {
            tmpDetail = parts;
            tmpDetail.Taked();
        }
        return result;
    }

    private Vector3 KeyBoardRotation()
    {
        Vector3 rotation = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            rotation.y = -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotation.y = 1;
        }

        if (Input.GetKey(KeyCode.R))
        {
            rotation.x = 1;
        }
        if (Input.GetKey(KeyCode.F))
        {
            rotation.x = -1;
        }

        if (Input.GetKey(KeyCode.T))
        {
            rotation.z = 1;
        }
        if (Input.GetKey(KeyCode.G))
        {
            rotation.z = -1;
        }

        return rotation;
    }

    public void ChangeInstaller(Installer tunedInstaller)
    {
        if (tunedInstaller is null)
        {
            throw new ArgumentNullException("Tuned Installer must be not null!");
        }
        placer = (Installer)tunedInstaller;

        //Исправить позже
        if (tmpDetail != null)
        {
            Destroy(tmpDetail.gameObject);
            tmpDetail = null;
        }
            
    }
}
