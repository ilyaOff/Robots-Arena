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

        //Поворот камеры на цель
        this.transform.LookAt(goal);

        //поворот вверх-вниз
        dAngle = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;
        float newAngle = this.transform.eulerAngles.x  + dAngle * Mathf.Rad2Deg;
        if (newAngle < 180 && newAngle > maxAngle) newAngle = maxAngle;
        //if (newAngle < 360+minAngle) newAngle =360+ minAngle;
        if( newAngle > 180 && (newAngle-360 < minAngle)) newAngle = minAngle+360;        
        dAngle = (newAngle - this.transform.eulerAngles.x) * Mathf.Deg2Rad;
        Debug.Log(newAngle);
        Debug.Log(dAngle);
        //if (newAngle > maxAngle * Mathf.Deg2Rad || newAngle < minAngle * Mathf.Deg2Rad) return;
        this.transform.RotateAround(goal.position, this.transform.right, dAngle);

        this.gameObject.transform.position = new Vector3(
                                        this.gameObject.transform.position.x,
                                        Mathf.Clamp(this.gameObject.transform.position.y,
                                            Mathf.Max(minY, goal.position.y + R * Mathf.Sin(minAngle * Mathf.Deg2Rad)),
                                            Mathf.Min(maxY, goal.position.y + R * Mathf.Sin(maxAngle * Mathf.Deg2Rad))),
                                        this.gameObject.transform.position.z);

        
    }
}
