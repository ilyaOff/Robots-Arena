using UnityEngine;

[System.Serializable]
public abstract class Installer : ScriptableObject
{
    [SerializeField]
    [Range(30, 360)]
    private float speedRotate = 90;

    Vector3 oldPlace = Vector3.zero;
    public Part StartInstalling(Part prefab, Vector3 position, Quaternion rotation)
    {
        Part tmpDetail = Instantiate(prefab, position, rotation);
        DetailTake(tmpDetail, prefab, position, rotation);
        return tmpDetail;
    }
    public void DetailTake(Part tmpDetail, Part prefab, Vector3 position, Quaternion rotation)
    {
        tmpDetail.Taked();
        DetailTaked(prefab, position, rotation);
    }
    protected abstract void DetailTaked(Part prefab, Vector3 position, Quaternion rotation);
    public void DetailRotate(Part tmpDetail, Vector3 rotation)
    {
        DetailRotate(tmpDetail, rotation.x, rotation.y, rotation.z);
    }
    public void DetailRotate(Part tmpDetail, float xAngle, float yAngle, float zAngle)
    {
        if (tmpDetail is null)
        {
            Debug.LogError("Null Reflection " + nameof(tmpDetail));
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
    public bool TryPlaceDetail(Part tmpDetail, Part fasteningPart)
    {
        if (fasteningPart != null)
        {
            PlaceDetail(tmpDetail, fasteningPart);
            OptionPlaceDetail(fasteningPart);
            return true;
        }
        else
        {
            Debug.Log("Cannot Place");
            return false;
        }
    }
    protected void PlaceDetail(Part tmpDetail, Part fasteningPart)
    {
        tmpDetail.transform.parent = fasteningPart.transform;
        tmpDetail.Install();
    }

    protected abstract void OptionPlaceDetail(Part fasteningPart);
    public void ChangePositionDetail(Part tmpDetail, Part fasteningPart,
                                            Vector3 position, Vector3 surfaceNormal)
    {
        Transform connectionPoint = tmpDetail.connectionPoint;
        Vector3 shift = -connectionPoint.position + tmpDetail.transform.position;
        //Точка пересечения луча + позиция точки привязки в префабе
        tmpDetail.transform.position = position + shift;

        if (fasteningPart is null)
        {
            MissingFastener();
            return;            
        }

        if (!oldPlace.Equals(position))
        {
            oldPlace = position;
            tmpDetail.transform.rotation =
                Quaternion.FromToRotation(connectionPoint.up, surfaceNormal)
                * tmpDetail.transform.rotation;

            OptionChangePositionDetail(tmpDetail, fasteningPart);
        }
    }
    protected abstract void MissingFastener();
    protected abstract void OptionChangePositionDetail(Part tmpDetail, Part parts);
}
