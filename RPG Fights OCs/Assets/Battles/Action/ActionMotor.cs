using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActionMotor : MonoBehaviour
{
    public int actionObjective;
    public float[] actionData;
    public int turnosPresentes;
    int turnoEjecucionPresente;

    public bool actorIsActing; // Cuando el actor padre est� actuando
    bool canActivate; // Si la accion puede ser ejecutada, lo checa para que solo suceda una sola vez por actuacion
    //bool checkFatherActing; // Si se puede revisar el estado de acci�n del actor padre, para checar si actua o no
    ActorMotor fatherActor; // Padre actor de la acci�n

    void Start()
    {
        fatherActor = this.transform.parent.gameObject.GetComponent<ActorMotor>();
        // Se declara falsa esta variable para que pueda iniciarse el bucle de checar si el actor padre est� actuando
        //checkFatherActing = true;
        //print(fatherActor);
        ActionSuccesion();
    }

    void Update()
    {
        //print((actionData[2] * actionData[5]) * fatherActor.actorsData[5]);
        //print("Hola, soy accion");
        // Cuando deje de actuar activar OneHitSwitch
        /*
        if (actorIsActing == true && canActivate == true)
        {
            //Activarlo cuando empecemos a utilizar acciones
            ActionSuccesion();
        }
        if (actorIsActing == false)
            canActivate = true;
        // Si el check de variable de padre es verdadero iniciar la corrutina
        if (checkFatherActing == true)
        {
            StartCoroutine("checkActorIsActingVariable");
        }
        */
        if (fatherActor.canActionActivate == false)
        {
            canActivate = true;
        }

        if (fatherActor.canActionActivate == true && canActivate)
        {
            ActionSuccesion();
        }
    }

    /*IEnumerator checkActorIsActingVariable()
    {
        // Hacer check para que solo sea una vez
        checkFatherActing = false;
        // Esperar segundos
        yield return new WaitForSeconds(.4f);
        // Si la variable no es igual, igualarla
        if (actorIsActing != fatherActor.actorIsActing)
        actorIsActing = fatherActor.actorIsActing;
        // Hacer el check para que lo vuelva a checar
        checkFatherActing = true;
    }*/

    // L�gica de la acci�n, sucede cada que le toca a un 
    void ActionSuccesion()
    {
        canActivate = false; // OneHitSwitch para que solo se active esta funcion una vez

        // Checar Turno ejecucion de Aplicaci�n
        if (actionData[3] == turnoEjecucionPresente)
        {
            // Ejecutar si las condiciones existen
            // Si se ejecuta, turnoEjecucionPresente = 0
            // Checar si es suma o multiplicaci�n
            ActionAplicationAddition();
            turnoEjecucionPresente = -1;
        }
        turnosPresentes++; turnoEjecucionPresente++;
        // Checar Duraci�n Turnos
        if (actionData[4] <= turnosPresentes)
        {
            Destroy(this.gameObject);
        }
    }
    void ActionAplicationAddition()
    {
        // POR AHORA TODO SOLO FUNCIONA EN SUMAS, NO EN MULTIPLICACIONES
        // actionData[2] es la cantidad de la accion
        switch (actionData[0])
        {
            case 0:
                // ATAQUE: Le quita la vida al personaje, se multiplica por el da�o del actor original y por la resistencia del personaje
                // da�o del actor original : actionData[5]
                // Vic-Vida Presente += (Atc-Cantidad * Atc-Valor de Danio) / Vic-resistencia presente
                fatherActor.actorsData[3] += (actionData[2] * actionData[5]) / fatherActor.actorsData[5];
                break;
            case 1:
                // CURACI�N: Le aumenta la vida al personaje, se multiplica por la curaci�n del personaje
                fatherActor.actorsData[3] += (actionData[2] * fatherActor.actorsData[6]);
                break;
            case 2:
                // CAMBIO RESISTENCIA: Cambia el valor de resistencia del personaje, se multiplica por el multiplicador de resistencia
                fatherActor.actorsData[5] += (actionData[2] * fatherActor.actorsData[9]);
                break;
            case 3:
                // AUMENTO DE VELOCIDAD: Aumenta la velocidad del personaje, se multiplica por el multiplicador de aceleraci�n
                fatherActor.actorsData[7] += (actionData[2] * fatherActor.actorsData[11]);
                break;
            case 4:
                // REDUCCI�N DE VELOCIDAD: Reduce la velocidad del personaje, se multiplica por el multiplicador de desaceleraci�n
                fatherActor.actorsData[7] += (actionData[2] * fatherActor.actorsData[12]);
                break;
            case 5:
                // CAMBIO DE CURACI�N: Cambia el valor de la curaci�n del personaje, se multiplica por el multiplicador de curaci�n
                fatherActor.actorsData[6] += (actionData[2] * fatherActor.actorsData[10]);
                break;
            case 6:
                // CAMBIO DE DA�O: Cambia el valor del da�o del personaje, se multiplica por el multiplicador de da�o
                fatherActor.actorsData[4] += (actionData[2] * fatherActor.actorsData[8]);
                break;
            case 7:
                // CAMBIO COOLDOWN: Cambia la duraci�n del cooldown despues de utilizar una habilidad
                // Duraci�n del cooldown: fatherActor.abilitiesData[fatherActor.actorsData[1], 0]
                fatherActor.abilitiesData[(int)fatherActor.actorsData[1], 0] += actionData[2];
                break;
            default:
                // CAMBIO DE MULTIPLICADORES: Cambia el valor de los multiplicadores de datos
                // 8 - Multi de Da�o, 9 - Multi de Resistencia, 10 - Multi de Curaci�n, 11 - Multi de aceleraci�n, 12 - Multi de Desacerleraci�n
                fatherActor.actorsData[(int)actionData[0]] += actionData[2];
                break;
        }
    }
}
