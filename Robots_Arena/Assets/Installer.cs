using UnityEngine;

[System.Serializable]
public abstract class Installer : ScriptableObject
{
    [SerializeField]
    [Range(30, 360)]
    private float speedRotate = 90;

    public void DetailRotate(Part tmpDetail, Vector3 rotation)
    {
        DetailRotate(tmpDetail, rotation.x, rotation.y, rotation.z);
    }
    public void DetailRotate(Part tmpDetail, float xAngle, float yAngle, float zAngle)
    {
        if (tmpDetail == null)
        {
            Debug.LogError("Invalid " + nameof(tmpDetail));
            return;
        }

        float xRad = xAngle * speedRotate * Mathf.Deg2Rad;
        float yRad = yAngle * speedRotate * Mathf.Deg2Rad;
        float zRad = zAngle * speedRotate * Mathf.Deg2Rad;

        Transform connectionPoint = tmpDetail.connectionPoint;
        Vector3 yAxis = connectionPoint.up;

        tmpDetail.transform.rotation = Quaternion.AngleAxis(yRad, yAxis)
                                        * tmpDetail.transform.rotation;

        Vector3 xAxis = Vector3.up;
        tmpDetail.transform.RotateAround(connectionPoint.position, xAxis, xRad);

        Vector3 zAxis = Vector3.Cross(yAxis, xAxis);
        if (zAxis.Equals(Vector3.zero))
            zAxis = Vector3.forward;
        tmpDetail.transform.RotateAround(connectionPoint.position, zAxis, zRad);
    }

    public bool TryPlaceDetail(Part tmpDetail, Part mountedParts)
    {
        if (mountedParts != null)
        {
            tmpDetail.transform.parent = mountedParts.transform;
            tmpDetail.Install();
            return true;
        }
        else
        {
            Debug.Log("Cannot Place");
            return false;
        }
    }
    public abstract void ChangeTransformDetail(Part tmpDetail, Part parts,
                                            Vector3 position, Vector3 surfaceNormal);
    
    

}
