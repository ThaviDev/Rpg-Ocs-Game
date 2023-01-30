using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor_OW : MonoBehaviour
{
    private float movementSpeedIG = 4;
    private Vector3 target;
    private float axisX;// El valor X del jugador
    private float axisY;// El valor Y del jugador
    public float axisIntervention = 0.6f; // El valor de desviación para que caminar en diagonal quede bien
    private Animator anim;
    private SpriteRenderer sprite;
    private Collider2D myCol;
    void Start()
    {
        myCol = GetComponent<Collider2D>();
        target = transform.position;
        // Encontrar el animador en el hijo
        anim = this.transform.Find("Ow Player Visual").gameObject.GetComponent<Animator>();
        // Encontrar el sprite del hijo
        sprite = this.transform.Find("Ow Player Visual").gameObject.GetComponent<SpriteRenderer>();
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
        Vector3 mov = new Vector3(axisX,axisY * axisIntervention, 0);

        //transform.position = Vector3.MoveTowards(transform.position, transform.position + mov, movementSpeedIG * Time.deltaTime );
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y) + mov, movementSpeedIG * Time.deltaTime);

        if (Mathf.Abs(axisX) > 0.1 || Mathf.Abs(axisY) > 0.1){
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsMoving",true);
        } else {
            anim.SetBool("IsIdle", true);
            anim.SetBool("IsMoving",false);
        }

        if (axisX < 0)
        {
            sprite.flipX = true;
        } else if (axisX > 0)
        {
            sprite.flipX = false;
        }

    }

    void OnCollisionEnter2D(Collision2D col){
        //target = transform.position;
    }
}
