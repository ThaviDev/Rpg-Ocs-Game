using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Explore : MonoBehaviour
{
    int myEnemID;
    Collider2D myCollider;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Encontrar Overworld Motor
        ScOw_Motor owMotor = FindObjectOfType<ScOw_Motor>();
        // Llamarlo para decirle que el jugador chocó contra un enemigo
        owMotor.EnemyEncountered(myEnemID);
        myCollider.enabled = false;
    }
}
