using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField]
    private Transform goal;

    public float maxRange = 10f;
    public float minRange = 1f;
    [Range(1, 10)]
    private float R = 4;

    [Range(1, 30)]
    public float moveSpeed = 3;
    [Range(1, 180)]
    public float rotateSpeed = 60;
    [Range(1, 180)]
    public float verticalSpeed = 30;

    [Range(0, 10)]
    public float minY = 0.2f;
    [Range(2, 20)]
    public float maxY = 10f;

    [Range(-15, 45)]
    public float minAngle = -5f;
    [Range(-15, 80)]
    public float maxAngle = 60f;
    private float minSin;
    private float maxSin;
    void Start()
    {
        if (minY > maxY)
        {
            Debug.LogError("minY > maxY!!!");
            float swap = minY;
            minY = maxY;
            maxY = swap;
        }
        if (minAngle > maxAngle)
        {
            Debug.LogError("minAngle > maxAngle!!!");
            float swap = minAngle;
            minAngle = maxAngle;
            maxAngle = swap;
        }
        minSin = Mathf.Sin(minAngle * Mathf.Deg2Rad);
        maxSin = Mathf.Sin(maxAngle * Mathf.Deg2Rad);
    }

    // Update is called once per frame
    void Update()
    {
        //поворот вокруг объекта
        float dAngle = -Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        this.transform.RotateAround(goal.position, Vector3.up, dAngle);

        //поддержка расстояния между камерой и объетом
        float dr = -Input.GetAxis("Mouse ScrollWheel") * moveSpeed ;
        R = Mathf.Clamp(R + dr, minRange, maxRange);
        Vector3 dir = this.transform.position - goal.position;
        this.transform.position = goal.position + dir.normalized * R;

        //поворот вверх-вниз
        dAngle = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;

        float x = this.gameObject.transform.position.x;
        float z = this.gameObject.transform.position.z;
        float y = this.gameObject.transform.position.y;

        float minY = Mathf.Max(this.minY, goal.position.y + R * minSin);
        float maxY = Mathf.Min(this.maxY, goal.position.y + R * maxSin);

        if ( (y <= maxY || dAngle < 0 ) && (y >= minY || dAngle > 0))
        {
            this.transform.RotateAround(goal.position, this.transform.right, dAngle);
            y = this.gameObject.transform.position.y;

            y = Mathf.Clamp(y, minY, maxY);
            this.gameObject.transform.position = new Vector3(x, y, z);
        }

        //Поворот камеры на цель
        this.transform.LookAt(goal);

    }
}
