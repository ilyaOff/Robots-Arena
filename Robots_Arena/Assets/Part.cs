using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] 
    private Transform[] connectionPoints;

    [SerializeField]
    private Renderer meshRenderer;
    
    [SerializeField]
    private Collider[] colliders;
    public Transform connectionPoint { get; private set; }
    void Awake()
    {
        colliders = this.gameObject.GetComponentsInChildren<Collider>();
        if (connectionPoints.Length == 0)
        {
            connectionPoint = this.transform;
        }
        else
        {
            connectionPoint = connectionPoints[0];
        }
    }

    //Может быть стоит использовать паттерн Visitor
    public void Taked()
    {
        var childs = GetComponentsInChildren<Part>();
        foreach (var child in childs)
        {
            child.Selected();            
        }
    }
    private void Selected()
    {
        if (connectionPoints.Length > 0)
            foreach (Transform point in connectionPoints)
            {
                point.gameObject.SetActive(true);
            }
        
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        
        meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                    meshRenderer.material.color.g,
                                    meshRenderer.material.color.b,
                                    meshRenderer.material.color.a / 2);
    }
        
    public void Install()
    {        
        var childs = GetComponentsInChildren<Part>();
        foreach (var child in childs)
        {
            child.Unselected();            
        }
    }
    private void Unselected()
    {
        if (connectionPoints.Length > 0)
            foreach (Transform point in connectionPoints)
            {
                point.gameObject.SetActive(false);
            }

        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        //Чтобы передать материал по ссылке, используется свойство gameObject
        meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                     meshRenderer.material.color.g,
                                     meshRenderer.material.color.b,
                                     meshRenderer.material.color.a * 2);
    }

    
    //Change connection points
}
