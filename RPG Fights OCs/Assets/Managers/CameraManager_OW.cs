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
        newPosition.y = target.position.y + 0.8f;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, speedFollow * Time.deltaTime);
        //print(newPosition);
    }
}
