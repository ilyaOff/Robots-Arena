using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    //[SerializeField] 
    public Transform[] connectionPoints;

    public Renderer meshRenderer;
    
    [SerializeField]
    private Collider[] colliders;
    void Awake()
    {
        colliders = this.gameObject.GetComponentsInChildren<Collider>();
    }

    public void SetTransparent()
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
        //Чтобы передать материал по ссылке, используется свойство gameObject
        meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                    meshRenderer.material.color.g,
                                    meshRenderer.material.color.b,
                                    meshRenderer.material.color.a / 2);

        this.transform.parent = null;
    }

    public void SetNormal(Transform parent)
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

        this.transform.parent = parent;
    }


}
