using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acting_Manager : MonoBehaviour
{
    public GameObject player;
    //public int actNum;
    void Start()
    {
        player = FindObjectOfType<PlayerMotor_OW>().gameObject;
    }

    void Update()
    {

    }

    void Acting(int actNum)
    {
        print("Estoy acuando mama, mirame soy: " + actNum);
       // player.transform.position = Vector3.MoveTowards(transform.position, Vector3.up, 1 * Time.deltaTime);
    }

    public void StartActing(int id) {
        Acting(id);
    }

}
