using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newSimpleInstaller", menuName = "Installer/SimpleInstaller")]
public class SimpleInstaller : Installer
{
    //Добавить модифицированный режим, в котором будет использоваться
    //[Range(0.005f, 0.4f)]
    //public float changePositionDistance = 0.1f;
    protected override void DetailTaked(Part prefab, Vector3 position, Quaternion rotation)
    {
        return;
    }

    protected override void MissingFastener()
    {
        return;
    }

    protected override void OptionChangePositionDetail(Part tmpDetail, Part parts)
    {
        return;
    }

    protected override void OptionPlaceDetail(Part fasteningPart)
    {
        return;
    }
}
