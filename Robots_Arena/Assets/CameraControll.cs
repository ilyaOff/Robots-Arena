using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField]
    private Transform goal;

    public float maxRange = 10f;
    public float minRange = 1f;
    [SerializeField, Range(1, 10)]
    float R = 4;

    [Range(1, 180)]
    public float rotateSpeed = 2;
    [Range(1, 20)]
    public float moveSpeed = 3;
   
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //поворот вокруг объекта
        float dAngle = -Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        this.transform.RotateAround(goal.position, Vector3.up, dAngle);

        //поддержка расстояния между камерой и объетом
        float dr = -Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        R = Mathf.Clamp(R + dr, minRange, maxRange);
        Vector3 dir = this.transform.position - goal.position;
        this.transform.position = goal.position + dir.normalized * R;

        //Поворот камеры на цель
        this.transform.LookAt(goal);
    }
}
