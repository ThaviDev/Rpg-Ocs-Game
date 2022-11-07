using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActionMotor : MonoBehaviour
{
    public int actionObjective;
    public int[] actionData;
    int turnosPresentes;
    int turnoEjecucionPresente;

    public bool actorIsActing; // Cuando el actor padre est� actuando
    bool canActivate; // Si la accion puede ser ejecutada, lo checa para que solo suceda una sola vez por actuacion
    ActorMotor fatherActor; // Padre actor de la acci�n

    void Start()
    {
        fatherActor = this.transform.parent.gameObject.GetComponent<ActorMotor>();
        print(fatherActor);
        ActionSuccesion();
    }

    void Update()
    {
        // Cuando deje de actuar activar OneHitSwitch
        if (actorIsActing == false)
            canActivate = true;
        if (actorIsActing == true && canActivate == true)
        {
            ActionSuccesion();
        }
    }

    // L�gica de la acci�n, sucede cada que le toca a un 
    void ActionSuccesion()
    {
        canActivate = false; // OneHitSwitch para que solo se active esta funcion una vez
        // Checar Turno ejecucion de Aplicaci�n
        if (actionData[3] == turnosPresentes)
        {
            print("Hice danio");
        }
        // Ejecutar si las condiciones existen
        turnosPresentes++; turnoEjecucionPresente++;
        // Si se ejecuta, turnoEjecucionPresente = 0
        // Checar Duraci�n Turnos
        if (actionData[4] < turnosPresentes)
        {
            Destroy(this.gameObject);
        }
    }
}
