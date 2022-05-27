using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager_OW : MonoBehaviour
{
    private float speedFollow = 5;
    private Transform target;

    void Start(){
        target = FindObjectOfType<PlayerMotor_OW>().transform;
    }
    void Update()
    {
        Vector3 newPosition = target.position;
        //newPosition.x = target.position.x - 10;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, speedFollow * Time.deltaTime);
        //print(newPosition);
    }
}
