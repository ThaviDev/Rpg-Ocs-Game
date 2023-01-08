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

    public bool actorIsActing; // Cuando el actor padre está actuando
    bool canActivate; // Si la accion puede ser ejecutada, lo checa para que solo suceda una sola vez por actuacion
    //bool checkFatherActing; // Si se puede revisar el estado de acción del actor padre, para checar si actua o no
    ActorMotor fatherActor; // Padre actor de la acción

    void Start()
    {
        fatherActor = this.transform.parent.gameObject.GetComponent<ActorMotor>();
        // Se declara falsa esta variable para que pueda iniciarse el bucle de checar si el actor padre está actuando
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

    // Lógica de la acción, sucede cada que le toca a un 
    void ActionSuccesion()
    {
        canActivate = false; // OneHitSwitch para que solo se active esta funcion una vez

        // Checar Turno ejecucion de Aplicación
        if (actionData[3] == turnoEjecucionPresente)
        {
            // Ejecutar si las condiciones existen
            // Si se ejecuta, turnoEjecucionPresente = 0
            // Checar si es suma o multiplicación
            ActionAplicationAddition();
            turnoEjecucionPresente = -1;
        }
        turnosPresentes++; turnoEjecucionPresente++;
        // Checar Duración Turnos
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
                // ATAQUE: Le quita la vida al personaje, se multiplica por el daño del actor original y por la resistencia del personaje
                // daño del actor original : actionData[5]
                // Vic-Vida Presente += (Atc-Cantidad * Atc-Valor de Danio) / Vic-resistencia presente
                fatherActor.actorsData[3] += (actionData[2] * actionData[5]) / fatherActor.actorsData[5];
                break;
            case 1:
                // CURACIÓN: Le aumenta la vida al personaje, se multiplica por la curación del personaje
                fatherActor.actorsData[3] += (actionData[2] * fatherActor.actorsData[6]);
                break;
            case 2:
                // CAMBIO RESISTENCIA: Cambia el valor de resistencia del personaje, se multiplica por el multiplicador de resistencia
                fatherActor.actorsData[5] += (actionData[2] * fatherActor.actorsData[9]);
                break;
            case 3:
                // AUMENTO DE VELOCIDAD: Aumenta la velocidad del personaje, se multiplica por el multiplicador de aceleración
                fatherActor.actorsData[7] += (actionData[2] * fatherActor.actorsData[11]);
                break;
            case 4:
                // REDUCCIÓN DE VELOCIDAD: Reduce la velocidad del personaje, se multiplica por el multiplicador de desaceleración
                fatherActor.actorsData[7] += (actionData[2] * fatherActor.actorsData[12]);
                break;
            case 5:
                // CAMBIO DE CURACIÓN: Cambia el valor de la curación del personaje, se multiplica por el multiplicador de curación
                fatherActor.actorsData[6] += (actionData[2] * fatherActor.actorsData[10]);
                break;
            case 6:
                // CAMBIO DE DAÑO: Cambia el valor del daño del personaje, se multiplica por el multiplicador de daño
                fatherActor.actorsData[4] += (actionData[2] * fatherActor.actorsData[8]);
                break;
            case 7:
                // CAMBIO COOLDOWN: Cambia la duración del cooldown despues de utilizar una habilidad
                // Duración del cooldown: fatherActor.abilitiesData[fatherActor.actorsData[1], 0]
                fatherActor.abilitiesData[(int)fatherActor.actorsData[1], 0] += actionData[2];
                break;
            default:
                // CAMBIO DE MULTIPLICADORES: Cambia el valor de los multiplicadores de datos
                // 8 - Multi de Daño, 9 - Multi de Resistencia, 10 - Multi de Curación, 11 - Multi de aceleración, 12 - Multi de Desacerleración
                fatherActor.actorsData[(int)actionData[0]] += actionData[2];
                break;
        }
    }
}
