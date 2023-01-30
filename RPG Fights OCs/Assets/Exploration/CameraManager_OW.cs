using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraManager_OW : MonoBehaviour
{
    private float speedFollow = 5;
    private Transform target;
    // Determina si pueda seguír al jugador en Exploracion
    public bool canFollowPlayer;

    void Start(){
        DontDestroyOnLoad(this.gameObject);
        try
        {
            target = FindObjectOfType<PlayerMotor_OW>().transform;
        }
        catch {
            //target = FindObjectOfType<PlayerMotor_OW>().transform;
            print("No hay jugador");
            canFollowPlayer = false;
        }
        // Si hay un objetivo desde el try catch entonces se puede seguír al objetivo (jugador)
        if (target != null)
        {
            canFollowPlayer = true;
        }
    }
    void Update()
    {
        if (canFollowPlayer == true)
        {
            Vector3 newPosition = target.position;
            newPosition.y = target.position.y + 0.8f;
            newPosition.z = -10;
            transform.position = Vector3.Slerp(transform.position, newPosition, speedFollow * Time.deltaTime);
            //print(newPosition);
        }
    }

    public void UpdateVariables()
    {

    }
}
