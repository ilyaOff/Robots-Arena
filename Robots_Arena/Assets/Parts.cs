using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public Transform[] connectionPoints;

    private bool isSelected = false;
    public bool IsSelected
    {
        get 
        { 
            return isSelected;
        }
        set 
        {
            isSelected = value;
            if(connectionPoints.Length > 0)
                foreach (Transform point in connectionPoints)
                {
                    point.gameObject.SetActive(value);
                }
        }
    }

    public Renderer meshRenderer;

    void Start()
    {
        
    }

    public void SetTransparent()
    {        
        IsSelected = true;
        //Чтобы передать материал по ссылке, используется свойство gameObject
       
        meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                    meshRenderer.material.color.g,
                                    meshRenderer.material.color.b,
                                    meshRenderer.material.color.a / 2);
    }

    public void SetNormal()
    {
        IsSelected = false;
        //Чтобы передать материал по ссылке, используется свойство gameObject
        meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                     meshRenderer.material.color.g,
                                     meshRenderer.material.color.b,
                                     meshRenderer.material.color.a * 2);
    }


}
