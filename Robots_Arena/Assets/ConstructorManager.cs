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
    public Installer placer;

    [SerializeField]
    private Installer[] placers;

    /*private Vector3 testNormal = Vector3.up;
    private Vector3 testNormalStart = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(testNormalStart, testNormalStart + 3 * testNormal);
    }*/
    private void Start()
    {
        //placer = (Placer)Placer.CreateInstance(nameof(Placer));
        placer = new SimpleInstaller();
    }
    public void StartPlacingPart(Part prefab)
    {
        Vector3 position = Vector3.zero;
        if (tmpDetail != null)
        {
            position = tmpDetail.transform.position;
            Destroy(tmpDetail.gameObject);
        }

        tmpDetail = Instantiate(prefab, position, Quaternion.identity);
        tmpDetail.Taked();
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

        if (tmpDetail == null)
        {
            if (Input.GetMouseButtonDown(0))
                TryTakeDetail(parts);
        }
        else
        {
            placer.DetailRotate(tmpDetail, KeyBoardRotation());
            placer.ChangeTransformDetail(tmpDetail, parts, position, hit.normal);

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

}
