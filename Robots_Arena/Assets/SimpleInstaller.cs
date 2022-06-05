using UnityEngine;

[System.Serializable]
public class SimpleInstaller : Installer
{
    Vector3 oldPointPlaceDetail = Vector3.zero;//Problems??
    //Добавить модифицированный режим, в котором будет использоваться
    //[Range(0.005f, 0.4f)]
    //public float changePositionDistance = 0.1f;
    public override void ChangeTransformDetail(Part tmpDetail, Part parts, Vector3 position, Vector3 surfaceNormal)
    {
        Transform connectionPoint = tmpDetail.connectionPoint;
        Vector3 shift = -connectionPoint.position + tmpDetail.transform.position;
        //Точка пересечения луча + позиция точки привязки в префабе
        tmpDetail.transform.position = position + shift;

        if (parts != null)
        {
            //Оптимизировать при медленной работе
            //if(Vector3.Distance(oldPointPlaceDetail, position) < changePositionDistance)
            if (!oldPointPlaceDetail.Equals(position))
            {
                oldPointPlaceDetail = position;
                tmpDetail.transform.rotation =
                    Quaternion.FromToRotation(connectionPoint.up, surfaceNormal)
                    * tmpDetail.transform.rotation;
            }
        }
    }
}
