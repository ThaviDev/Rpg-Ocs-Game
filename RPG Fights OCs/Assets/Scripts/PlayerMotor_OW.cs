using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor_OW : MonoBehaviour
{
    private float movementSpeedIG = 4;
    private Vector3 target;
    private float axisX;// El valor X del jugador
    private float axisY;// El valor Y del jugador
    private Animator anim;
    private Collider2D myCol;
    void Start()
    {
        myCol = GetComponent<Collider2D>();
        target = transform.position;
        // Encontrar el animador en el hijo
        anim = this.transform.Find("Ow Player Visual").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0)){
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            myCol.enabled = false;
        }
        myCol.enabled = true;
        transform.position = Vector3.MoveTowards(transform.position, target, movementSpeedIG * Time.deltaTime);
        */
        // -- Movimiento con Inputs
        
        //Axis de movimiento
        axisX = Input.GetAxisRaw("Horizontal");
        axisY = Input.GetAxisRaw("Vertical");
        Vector3 mov = new Vector3(axisX,axisY,0);
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + mov,
            movementSpeedIG * Time.deltaTime );

        if (Mathf.Abs(axisX) > 0.1 || Mathf.Abs(axisY) > 0.1){
            anim.SetBool("isMoving",true);
        } else {
            anim.SetBool("isMoving",false);
        }

    }

    void OnCollisionEnter2D(Collision2D col){
        target = transform.position;
    }
}
