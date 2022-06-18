using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newCircularSymmetricalInstaller", menuName = "Installer/CircularSymmetricalInstaller")]
public class CircularSymmetricalInstaller : Installer
{    
    [SerializeField]
    private int countClones = 2;

    Part[] clones;

    protected override void DetailTaked(Part prefab, Vector3 position, Quaternion rotation)
    {        
        clones = new Part[countClones];
        for (int i = 0; i < clones.Length; i++)
        {
            clones[i] = Instantiate(prefab);
            clones[i].Taked();
            clones[i].gameObject.SetActive(false);
        }
    }    

    protected override void MissingFastener()
    {
        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].gameObject.SetActive(false);
        }
    }

    protected override void OptionChangePositionDetail(Part tmpDetail, Part parts)
    {
        float angle = 360 / (countClones + 1);
        Vector3 pointToRotation = parts.transform.position;
        Vector3 axisRotation = parts.transform.up;

        for (int i = 0; i < clones.Length; i++)
        {
            var clon = clones[i];
            clon.gameObject.SetActive(true);
            clon.transform.position = tmpDetail.transform.position;
            clon.transform.rotation = tmpDetail.transform.rotation;

            clon.transform.RotateAround(pointToRotation, axisRotation, angle * (i + 1));
        }
    }

    protected override void OptionPlaceDetail(Part fasteningPart)
    {
        foreach (var clon in clones)
        {
            this.PlaceDetail(clon, fasteningPart);
        }
    }
}
