using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public ScBtl_Motor battleMotor;
    private GameObject[] actors; // Objetos de los actores
    private Transform[] actorsTransPos; // Posiciones de los actores, las que deben de tener
    private Transform confrontPosAllies; // Posicion de confrontacion de los aliados
    private Transform confrontPosEnemies; // Posicion de confrontacion de los enemigos
    private int[,] _actorsData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es el dato
    
    0 ID de personaje
    1 Aliado o Enemigo (Aliado 0, Enemigo 1)
    2 Posición en la escena
    3 Habilidad seleccionada
    4 Vida
    5 Vida Maxima
    6 Vida Inicial
    7 Velocidad
    8 Velocidad Inicial
    9 Estado de Disponibilidad (0 Normal, 1 Muerto, 2 Ejecutando, 3 Aturdido, 4 Dormido)
    * Modificadores de datos (Ejemplo; Mayor curacion cambiaría curacion)
    10 Modificador de Daño
    11 Modificador de Curación
    12 Modificador de Mayor Velocidad (aceleracion)
    13 Modificador de Menor Velocidad (desaceleracion)
    14 Modificador de Precisión
    15 Modificador de Críticos Aleatorios
    */
    private int[,,] _abilitysData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es la habilidad
     * Tercer elemento es el dato
    
    0 Turnos de la habilidad
    1 Turnos Utilizados
    2 Precisión
    3 Criticos Aleatorios
    4 Es apuntable // Osea si puedes dirigirlo a un personaje en específico
    5 Objetivo Posible (0 al enemigo seleccionado, 1 al aliado seleccionado, 2 a todos el enemigo, 3 a todos los aliados, 4 a uno mismo)
    6 Objetivo Seleccionado (0/5 actor, 6 a todos el enemigo, 7 a todos los aliados, 8 a uno mismo)
    7 Hay Confrontación
    8 Cantidad de Cooldown
    9 Cooldown Presente
    10 Cantidad de acciones de la habilidad
    11 ID de Efecto secundario (0 = nada)
    12 ID de Efecto terciario (0 = nada) 
    */
    private int[,,,] _actionsData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es la habilidad
     * Tercer elemento es la accion de la habilidad
     * Cuarto elemento es el dato
    
    0 Clasificación de la accion (Daño, Curación, Cambio de velocidad, etc.)
    1 Tipo de aplicacion (Suma, Porcentaje, Multiplicación)
    2 Cantidad que se aplicará
    3 Turno para aplicar
    4 Objetivo Establecido (Target) [Dato de esta variable es el numero del actor siendo apuntado]
    */

    private GameObject fatherObj; // Objeto padre de todos los actores y posiciones
    public ActorScOb[] actorSCOB; // Scriptable Object que tiene las estadísticas de los personajes
    public int[] SCOBactorPos; // Posicion del personaje segun el objeto del actor y el objeto de la posicion
                               // (0 - 2 para los aliados, 3 - 5 para los enemigos)

    // Variables Publicas
    public int actorSelected = 0; // Actor seleccionado
    public int butSelAbil; // Boton seleccionado
    public int aimedActor = -1; // Personaje apuntado
    public int possibleAimedActor; // Posible personaje apuntado

    public int battleSuccesion = 0; // Numero que determina los sucesos de la batalla del 0 al 2

    int numOfAlies = 0; // Cantidad de aliados en la escena
    int numOfEnemies = 0; // Cantidad de enemigos en la escena

    int stp = 0; // El siguiente paso para que continue la batalla

    void Start()
    {
        // Encontrar Battle Motor
        battleMotor = FindObjectOfType<ScBtl_Motor>();

        // Declarar variables
        actorSelected = -1; // No hay actor siendo seleccionado

        // Declarar Arreglos
        _actorsData = new int[6, 16];
        _abilitysData = new int[6, 3, 12];
        _actionsData = new int[6, 3, 5, 5];

        // Declarar Objetos
        actors = new GameObject[6];
        actorsTransPos = new Transform[6];

        //Encontrar Objeto padre
        fatherObj = GameObject.Find("BattleAssets");
        //Declarar Hijos de los padres
        Transform fatherObj_Actors = fatherObj.gameObject.transform.Find("Actors");
        Transform fatherObj_Positions = fatherObj.gameObject.transform.Find("Positions");

        // Encontrar Objetos de las posiciones de confrontación
        confrontPosAllies = fatherObj_Positions.gameObject.transform.Find("AttackPosChars");
        confrontPosEnemies = fatherObj_Positions.gameObject.transform.Find("AttackPosEnems");

        for (int i = 0; i < 6; i++) {
            // Encontrar actores
            actors[i] = fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject;
            // Encontrar posiciónes
            actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Actor" + i + "Pos");
        }
        // Clasificar SCOB 
        SCOBvaluesToArrays();
    }

    void Update()
    {
        print(aimedActor);

        // Datos de Battle Motor
        battleMotor.m_bm_actorsSel = actorSelected;
        butSelAbil = battleMotor.m_ui_butSelAbil;

        aimedActor = battleMotor.m_ui_aimedActor;
        battleMotor.m_bm_possibleAimedActor = possibleAimedActor;

        battleMotor.m_bm_step = stp;

        // Hay que ser lo más sencillos posibles y luego nos preocupamos en añadir lo demas
        switch (battleSuccesion)
        {
            case 0:
                // Si la batalla está en estado 0 entonces entra en la primera etapa
                BattleSuccesion1();
                break;
            case 1:
                // Si la batalla está en estado 1 entonces entra en la segunda etapa
                //BattleSuccesion2();
                break;
        }


    }
    void BattleSuccesion1()
    {
        _actorsData[0, 8] = 1;
        bool canAct = false; // Variable para checar si el personaje puede actuar
        int currentActor = 0; // Variable que determina cual es el actor en ese momento que está actuando
        // Battle Selection
        // Checar si el personaje puede actuar
        if (numOfAlies > currentActor)
        {
            if (_actorsData[currentActor, 8] == 0)
            {
                canAct = true;
            }
        } else
        {
            Debug.Log("Aliado numero " + currentActor + " no esta disponible");
            currentActor++;
        }
        if (canAct == true)
        {
            // El actor seleccionado es el actor actual
            actorSelected = currentActor; // actor selected activa la interfaz de selección del personaje
                                          //battleMotor.m_ui_butSelAbil;
            butSelAbil = -1;
            // Esperar Input
            if (butSelAbil > 0 && butSelAbil < 2)
            {
                print("El Aliado " + currentActor + " Esta disponible");
                // Guardar Eleccion tomada
                // La habilidad seleccionada es igual al input del boton para seleccionar dicha habilidad
                _actorsData[actorSelected, 14] = butSelAbil;
                // Checar puntería y con o sin Input apuntar a dicho personaje
                ChecarPunteria();
            }
        }
    }



    void ChecarPunteria()
    {
        possibleAimedActor = _abilitysData[actorSelected, butSelAbil, 5];
        // El objetivo posible de la habilidad seleccionada del personaje seleccionado
        switch(possibleAimedActor)
        {
            case 0:
                if (aimedActor !< 0)
                {
                    _abilitysData[actorSelected, butSelAbil, 6] = aimedActor;
                }
                break;
            case 1:
                if (aimedActor! < 0)
                {
                    _abilitysData[actorSelected, butSelAbil, 6] = aimedActor + 3;
                }
                break;
            case 2:
                _abilitysData[actorSelected, butSelAbil, 6] = 6;
                break;
            case 3:
                _abilitysData[actorSelected, butSelAbil, 6] = 7;
                break;
            case 4:
                _abilitysData[actorSelected, butSelAbil, 6] = 8;
                break;
        }
        //_abilitysData[i, 1, 5]
    }

    void SCOBvaluesToArrays()
    {
        for (int i = 0; i < 6; i++)
        {
            bool e; // Checa si es verdad que el SCOB existe
            e = true;
            try
            {
                // Buscar si el objeto existe igualando a una variable
                _actorsData[i, 2] = actorSCOB[i].actorsData[0];
            }
            catch
            {
                e = false;
                Debug.Log("No hay SCOB para el actor numero: " + i);
            }
            if (e == true)
            {

                // Datos del jugador
                _actorsData[i, 0] = actorSCOB[i].actorID; // nombre del actor
                // Identificar si el personaje es enemigo o aliado y sumarlo a la cantidad de enemigos/aliados ademas de establecer su posicion
                if (actorSCOB[i].isEnemy == true) {
                    // Igualar el personaje como enemigo
                    _actorsData[i, 1] = 0;
                    // Asignar una posicion para el enemigo dependiendo de la cantidad de enemigos
                    _actorsData[i, 2] = numOfEnemies + 3;
                    // Se aumenta la cantidad de enemigos por 1
                    numOfEnemies++;
                }
                else {
                    // Igualar el personaje como aliado
                    _actorsData[i, 1] = 1;
                    // Asignar una posicion para el aliado dependiendo de la cantidad de aliados
                    _actorsData[i, 2] = numOfAlies;
                    // Se aumenta la cantidad de aliados por 1
                    numOfAlies++;
                }
                _actorsData[i, 3] = -1; // Habilidad seleccionada, ninguna
                _actorsData[i, 5] = actorSCOB[i].actorsData[0]; // Vida Máxima
                _actorsData[i, 6] = actorSCOB[i].actorsData[1]; // Vida inicial
                _actorsData[i, 4] = _actorsData[i, 5]; // Igualar Vida inicial con Vida
                _actorsData[i, 8] = actorSCOB[i].actorsData[2]; // Velocidad inicial
                _actorsData[i, 7] = _actorsData[i, 8]; // Igualar Velocidad inicial con Velocidad
                _actorsData[i, 9] = 0; // Estado de disponibilidad es normal
                _actorsData[i, 10] = actorSCOB[i].actorsData[3]; // Modificador de daño
                _actorsData[i, 11] = actorSCOB[i].actorsData[4]; // Modificador de curación
                _actorsData[i, 12] = actorSCOB[i].actorsData[5]; // Modificador de aceleracion
                _actorsData[i, 13] = actorSCOB[i].actorsData[6]; // Modificador de desaceleracion
                _actorsData[i, 14] = actorSCOB[i].actorsData[7]; // Modificador de precision
                _actorsData[i, 15] = actorSCOB[i].actorsData[8]; // Modificador de criticos aleatorios

                // Datos de la primer habilidad
                _abilitysData[i, 0, 0] = actorSCOB[i].abilities[0].abilityData[0]; // Turnos de la habilidad
                _abilitysData[i, 0, 1] = 0; // Igualar turnos utilizados a 0
                _abilitysData[i, 0, 2] = actorSCOB[i].abilities[0].abilityData[1]; // Precison
                _abilitysData[i, 0, 3] = actorSCOB[i].abilities[0].abilityData[2]; // Criticos aleatorios
                _abilitysData[i, 0, 4] = actorSCOB[i].abilities[0].abilityData[3]; // Es apuntable
                _abilitysData[i, 0, 5] = actorSCOB[i].abilities[0].abilityData[6]; // Objetivo posible
                _abilitysData[i, 0, 6] = -1; // Objetivo seleccionado, ninguno
                _abilitysData[i, 0, 7] = actorSCOB[i].abilities[0].abilityData[4]; // Hay confrontacion
                _abilitysData[i, 0, 8] = actorSCOB[i].abilities[0].abilityData[5]; // Cantidad de cooldown

                // Datos de la segunda habilidad
                _abilitysData[i, 1, 0] = actorSCOB[i].abilities[1].abilityData[0]; // Turnos de la habilidad
                _abilitysData[i, 1, 1] = 0; // Igualar turnos utilizados a 0
                _abilitysData[i, 1, 2] = actorSCOB[i].abilities[1].abilityData[1]; // Precison
                _abilitysData[i, 1, 3] = actorSCOB[i].abilities[1].abilityData[2]; // Criticos aleatorios
                _abilitysData[i, 1, 4] = actorSCOB[i].abilities[1].abilityData[3]; // Es apuntable
                _abilitysData[i, 1, 5] = actorSCOB[i].abilities[1].abilityData[6]; // Objetivo posible
                _abilitysData[i, 1, 6] = -1; // Objetivo seleccionado, ninguno
                _abilitysData[i, 1, 7] = actorSCOB[i].abilities[1].abilityData[4]; // Hay confrontacion
                _abilitysData[i, 1, 8] = actorSCOB[i].abilities[1].abilityData[5]; // Cantidad de cooldown

                // Datos de la tercer habilidad
                _abilitysData[i, 2, 0] = actorSCOB[i].abilities[2].abilityData[0]; // Turnos de la habilidad
                _abilitysData[i, 2, 1] = 0; // Igualar turnos utilizados a 0
                _abilitysData[i, 2, 2] = actorSCOB[i].abilities[2].abilityData[1]; // Precison
                _abilitysData[i, 2, 3] = actorSCOB[i].abilities[2].abilityData[2]; // Criticos aleatorios
                _abilitysData[i, 2, 4] = actorSCOB[i].abilities[2].abilityData[3]; // Es apuntable
                _abilitysData[i, 2, 5] = actorSCOB[i].abilities[2].abilityData[6]; // Objetivo posible
                _abilitysData[i, 2, 6] = -1; // Objetivo seleccionado, ninguno
                _abilitysData[i, 2, 7] = actorSCOB[i].abilities[2].abilityData[4]; // Hay confrontacion
                _abilitysData[i, 2, 8] = actorSCOB[i].abilities[2].abilityData[5]; // Cantidad de cooldown

                // Datos de la primer accion de la primer habilidad
                _actionsData[i, 0, 0, 0] = actorSCOB[i].abilities[0].action1[0]; // Clasificación de la accion
                _actionsData[i, 0, 0, 1] = actorSCOB[i].abilities[0].action1[1]; // Tipo de aplicacion
                _actionsData[i, 0, 0, 2] = actorSCOB[i].abilities[0].action1[2]; // Cantidad de aplicacion
                _actionsData[i, 0, 0, 3] = actorSCOB[i].abilities[0].action1[3]; // Turno para aplicar
                _actionsData[i, 0, 0, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la segunda accion de la primer habilidad
                _actionsData[i, 0, 1, 0] = actorSCOB[i].abilities[0].action2[0]; // Clasificación de la accion
                _actionsData[i, 0, 1, 1] = actorSCOB[i].abilities[0].action2[1]; // Tipo de aplicacion
                _actionsData[i, 0, 1, 2] = actorSCOB[i].abilities[0].action2[2]; // Cantidad de aplicacion
                _actionsData[i, 0, 1, 3] = actorSCOB[i].abilities[0].action2[3]; // Turno para aplicar
                _actionsData[i, 0, 1, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la tercer accion de la primer habilidad
                _actionsData[i, 0, 2, 0] = actorSCOB[i].abilities[0].action3[0]; // Clasificación de la accion
                _actionsData[i, 0, 2, 1] = actorSCOB[i].abilities[0].action3[1]; // Tipo de aplicacion
                _actionsData[i, 0, 2, 2] = actorSCOB[i].abilities[0].action3[2]; // Cantidad de aplicacion
                _actionsData[i, 0, 2, 3] = actorSCOB[i].abilities[0].action3[3]; // Turno para aplicar
                _actionsData[i, 0, 2, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la primer accion de la segunda habilidad
                _actionsData[i, 1, 0, 0] = actorSCOB[i].abilities[1].action1[0]; // Clasificación de la accion
                _actionsData[i, 1, 0, 1] = actorSCOB[i].abilities[1].action1[1]; // Tipo de aplicacion
                _actionsData[i, 1, 0, 2] = actorSCOB[i].abilities[1].action1[2]; // Cantidad de aplicacion
                _actionsData[i, 1, 0, 3] = actorSCOB[i].abilities[1].action1[3]; // Turno para aplicar
                _actionsData[i, 1, 0, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la segunda accion de la segunda habilidad
                _actionsData[i, 1, 1, 0] = actorSCOB[i].abilities[1].action2[0]; // Clasificación de la accion
                _actionsData[i, 1, 1, 1] = actorSCOB[i].abilities[1].action2[1]; // Tipo de aplicacion
                _actionsData[i, 1, 1, 2] = actorSCOB[i].abilities[1].action2[2]; // Cantidad de aplicacion
                _actionsData[i, 1, 1, 3] = actorSCOB[i].abilities[1].action2[3]; // Turno para aplicar
                _actionsData[i, 1, 1, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la tercera accion de la segunda habilidad
                _actionsData[i, 1, 2, 0] = actorSCOB[i].abilities[1].action3[0]; // Clasificación de la accion
                _actionsData[i, 1, 2, 1] = actorSCOB[i].abilities[1].action3[1]; // Tipo de aplicacion
                _actionsData[i, 1, 2, 2] = actorSCOB[i].abilities[1].action3[2]; // Cantidad de aplicacion
                _actionsData[i, 1, 2, 3] = actorSCOB[i].abilities[1].action3[3]; // Turno para aplicar
                _actionsData[i, 1, 2, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la primera accion de la tercera habilidad
                _actionsData[i, 2, 0, 0] = actorSCOB[i].abilities[2].action1[0]; // Clasificación de la accion
                _actionsData[i, 2, 0, 1] = actorSCOB[i].abilities[2].action1[1]; // Tipo de aplicacion
                _actionsData[i, 2, 0, 2] = actorSCOB[i].abilities[2].action1[2]; // Cantidad de aplicacion
                _actionsData[i, 2, 0, 3] = actorSCOB[i].abilities[2].action1[3]; // Turno para aplicar
                _actionsData[i, 2, 0, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la segunda accion de la tercera habilidad
                _actionsData[i, 2, 1, 0] = actorSCOB[i].abilities[2].action2[0]; // Clasificación de la accion
                _actionsData[i, 2, 1, 1] = actorSCOB[i].abilities[2].action2[1]; // Tipo de aplicacion
                _actionsData[i, 2, 1, 2] = actorSCOB[i].abilities[2].action2[2]; // Cantidad de aplicacion
                _actionsData[i, 2, 1, 3] = actorSCOB[i].abilities[2].action2[3]; // Turno para aplicar
                _actionsData[i, 2, 1, 4] = 0; // Igualar objetivo establecido a 0

                // Datos de la primera accion de la tercera habilidad
                _actionsData[i, 2, 2, 0] = actorSCOB[i].abilities[2].action3[0]; // Clasificación de la accion
                _actionsData[i, 2, 2, 1] = actorSCOB[i].abilities[2].action3[1]; // Tipo de aplicacion
                _actionsData[i, 2, 2, 2] = actorSCOB[i].abilities[2].action3[2]; // Cantidad de aplicacion
                _actionsData[i, 2, 2, 3] = actorSCOB[i].abilities[2].action3[3]; // Turno para aplicar
                _actionsData[i, 2, 2, 4] = 0; // Igualar objetivo establecido a 0
            }
        }
    }
}