using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public ScBtl_Motor sceneBtl_Motor;
    private GameObject[] actors; // Objetos de los actores
    public Transform[] actorsTransPos; // Posiciones de los actores, las que deben de tener
    private Transform confrontPosAllies; // Posicion de confrontacion de los aliados
    private Transform confrontPosEnemies; // Posicion de confrontacion de los enemigos

    private int[,] _actorsData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es el dato
    
    0 ID de personaje
    1 Aliado o Enemigo (Aliado 0, Enemigo 1)
    2 Posición en la escena
    3 Habilidad seleccionada
    4 Vida Presente
    5 Vida Maxima
    6 Vida Inicial
    7 Velocidad Presente
    8 Velocidad Inicial
    9 Estado de Disponibilidad (0 Normal, 1 Muerto, 2 Ejecutando, 3 Aturdido, 4 Dormido)
    * Modificadores de datos (Ejemplo; Mayor curacion cambiaría curacion)
    10 Modificador de Daño (Resistencia)
    11 Modificador de Curación
    12 Modificador de Mayor Velocidad (aceleracion)
    13 Modificador de Menor Velocidad (desaceleracion)
    */
    private int[,,] _abilitysData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es la habilidad
     * Tercer elemento es el dato
    
    0 Turnos de la habilidad
    1 Turnos Presentes (Utilizados)
    2 Objetivo Posible (0 al enemigo seleccionado, 1 al aliado seleccionado, 2 a todos el enemigo, 3 a todos los aliados, 4 a uno mismo)
    3 Objetivo Seleccionado (0/5 actor, 6 a todos el enemigo, 7 a todos los aliados, 8 a uno mismo)
    4 Hay Confrontación
    5 Cantidad de Cooldown
    6 Cooldown Presente
    // 10 Cantidad de acciones de la habilidad
    // 11 ID de Efecto secundario (0 = nada)
    // 12 ID de Efecto terciario (0 = nada) 
    */
    private int[,,,] _actionsData;
    /* Primer elemento el Numero del actor
     * Segundo elemento es la habilidad
     * Tercer elemento es la accion de la habilidad
     * Cuarto elemento es el dato
    
    0 Clasificación de la accion (Daño, Curación, Cambio de velocidad, etc.)
    1 Tipo de aplicacion (0 Suma y 1 Porcentaje(Multiplicacion))
    2 Cantidad que se aplicará
    3 Turno para aplicar
    */

    private GameObject fatherObj; // OBJETO PADRE DE TODOS OBJETOS (ACTORES Y POSICIONES)
    public ActorScOb[] actorSCOB; // SCRIPTABLE OBJECTS DE LOS ACTORES
    public int[] SCOBactorPos; // Posicion del personaje segun el objeto del actor y el objeto de la posicion
                               // (0 - 2 para los aliados, 3 - 5 para los enemigos)

    int quantityOfAlies = 0; // Cantidad de aliados en la escena
    int quantityOfEnemies = 0; // Cantidad de enemigos en la escena

    // Variables Publicas

    public int currentActor = 0; // Variable que determina cual es el actor en ese momento que está actuando
    public int abilitySelected = -1; // HABILIDAD SELECCIONADA
    public int aimedActor = -1; // PERSONAJE APUNTADO PARA LA HABILIDAD
    public int possibleAimedActor = -1; // POSIBLE PERSONAJE APUNTADO (PARA ESTABLECER EL PERSONAJE APUNTADO)
    public bool abilCheckInput = false; // Checar si existió un input para la habilidad de un aliado
    public bool canAct = false; // Variable para checar si el personaje puede actuar

    public int battleSuccesion = 0; // SUCCESION DE EVENTOS DE LA BATALLA EN GENERAL
    public int actorSuccesion = 0; // SUCCESION DE EVENTOS DE LA SELECCIÓN Y MODIFICACIÓN DE DATOS DE LOS ACTORES
    private int[] actorSpeedSelector; // VARIABLE QUE GUARDA EL ORDEN DE QUIEN ACTUA PRIMERO A QUIEN ACTUA ULTIMO EN LOS PERSONAJES


    void Start()
    {
        // Encontrar Battle Motor
        sceneBtl_Motor = FindObjectOfType<ScBtl_Motor>();

        // Declarar variables
        canAct = false; // El siguiente personaje no puede actuar hasta que sea revisado
        aimedActor = -1; // No hay personaje apuntado
        possibleAimedActor = -1; // lo de arriba xd
        currentActor = 0; // El primer actor es el que se checa (el 0)

        // Declarar Arreglos
        _actorsData = new int[6, 16];
        _abilitysData = new int[6, 3, 12];
        _actionsData = new int[6, 3, 5, 5];

        actorSpeedSelector = new int[6] { 0, 0, 0, 0, 0, 0 }; // Seleccionador de quien toca primero

        // Declarar Objetos
        actors = new GameObject[6];
        actorsTransPos = new Transform[6];

        //Encontrar Objeto padre
        fatherObj = GameObject.Find("BattleAssets");
        //Declarar Hijos de los padres
        Transform fatherObj_Actors = fatherObj.gameObject.transform.Find("Actors");
        Transform fatherObj_Positions = fatherObj.gameObject.transform.Find("Positions");

        // Encontrar Objetos de las posiciones de confrontación
        confrontPosAllies = fatherObj_Positions.gameObject.transform.Find("AttackPosAllies");
        confrontPosEnemies = fatherObj_Positions.gameObject.transform.Find("AttackPosEnems");

        for (int i = 0; i < 6; i++) {
            // HACER UN TRY CATCH PARA CUANDO EXISTAN MÁS O MENOS ACTORES
            // Encontrar actores
            actors[i] = fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject;
            // Encontrar posiciones
            if (i < 3) {
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Ally" + i + "Pos");
            } else
            {
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Enemy" + i + "Pos");
            }
        }
        // Clasificar SCOBs
        SCOBvaluesToArrays();
    }

    void Update()
    {
        print("Personaje apuntado general: " + aimedActor);
        // BATTLE SUCCESION SIRVE PARA PASAR DE ESTADO DE BATALLA A ESTADO DE BATALLA
        // ACTOR SUCCESION SIRVE PARA MODIFICAR TODOS LOS DATOS NECESARIOS (INCLUYENDO INTPUT) PARA PASAR AL SIGUIENTE PERSONAJE
        // Hay que ser lo más sencillos posibles y luego nos preocupamos en añadir lo demas

        // Inputs temporales (Hasta aplicar lo de la interfaz)

        /* Succesion de la batalla: 
         * Los aliados eligen sus habilidades y a que personaje apuntan con ayuda del input del jugador
         * Los enemigos eligen sus habilidade de forma aleatoria (Luego sera con un algoritmo o algo asi)
         * Se calculará la velocidad de todos los personajes
         * Se aplicarán las habilidades dependiendo de la velocidad de cada personaje (osea, iniciara el combate)
         */
        switch (battleSuccesion)
        {
            case 0:
                // Funcion que determina las habilidades establecidas o seleccionadas de todos los actores de la escena
                ActorsSelection();
                break;
            case 1:
                battleSuccesion = 2;
                //print("Inicia la seleccion de los enemigos");
                break;
            case 2:
                SpeedDetector();
                //print("Hay que calcular la velocidad de los actores");
                break;
            case 3:
                CombatMomentFunction();
                //print("Que comience la batalla");
                break;
        }
    }
    void ActorsSelection()
    {
        switch (actorSuccesion)
        {
            case 0:
                // Se checa la seleccion de habilidades
                AbilitySelection();
                break;
            case 1:
                // Se checa la punteria de la seleccion anterior
                CheckAim();
                break;
        }
    }
    void AbilitySelection()
    {
        //_actorsData[0, 9] = 1;
        // Checar si el personaje puede actuar
        if (quantityOfAlies > currentActor)
        {
            // Checar si el actor esta disponible
            if (_actorsData[currentActor, 9] == 0) // si el actor esta disponible para actuar
            {
                print("El personaje puede actuar");
                canAct = true;

                // ESTO NO ES LO MEJOR PORQUE ESTÁ REINICIANDO LOS VALORES DEL UI CADA FRAME
                // HAY QUE ENCONTRAR LA MANERA DE HACER QUE UNA VEZ QUE CHEQUE QUE EL PERSONAJE PUEDA ACTUAR
                // ENTONCES ACCEDA A ESTO UNA VEZ
                sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
            }
            else // si el actor no esta disponible para actuar
            {
                print("El personaje no puede actuar, reiniciar valores");
                // REINICIA LOS VALORES PARA PASAR AL SIGUIENTE PERSONAJE
                RestartValues_Selection();
            }
        }
        else
        {
            // terminar seleccion de habilidades de aliados y pasar a seleccion de habilidades de enemigos
            battleSuccesion = 1;
        }

        if (canAct == true)
        {
            // INPUTS TEMPORALES
            if (Input.GetKeyDown(KeyCode.Q)) { abilCheckInput = true; abilitySelected = 0; }
            if (Input.GetKeyDown(KeyCode.W)) { abilCheckInput = true; abilitySelected = 1; }
            if (Input.GetKeyDown(KeyCode.E)) { abilCheckInput = true; abilitySelected = 2; }
            // Esperar Input
            if (abilCheckInput)
            {
                // No puede actuar (No puede seleccionar otra habilidad)
                canAct = false;
                // Reiniciar esta variable para el siguiente check
                abilCheckInput = false;
                // La habilidad seleccionada se establece en los valores del actor
                _actorsData[currentActor, 14] = abilitySelected;
                // La posible punteria se guarda en Posible Aimed Actor
                possibleAimedActor = _abilitysData[currentActor, abilitySelected, 5];
                // Imprimir la habilidad elegida
                print("La habilidad de elijió el aliado es " + _actorsData[currentActor, 14]);
                // Imprimir posible puntería
                print("El posible personaje apuntado es: " + _abilitysData[currentActor, abilitySelected, 5]);
                // Reiniciar variables del UI
                sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
                // IR A LA SIGUIENTE PARTE DE ACTOR SUCCESION
                actorSuccesion = 1;
            }
        }
    }

    void CheckAim()
    {
        // INPUTS TEMPORALES
        if (Input.GetKeyDown(KeyCode.A)) { aimedActor = 0; }
        if (Input.GetKeyDown(KeyCode.S)) { aimedActor = 1; }
        if (Input.GetKeyDown(KeyCode.D)) { aimedActor = 2; }
        // El objetivo posible de la habilidad seleccionada del personaje seleccionado
        switch (possibleAimedActor)
        {
            case 0:
                if (aimedActor > -1)
                {
                    Debug.Log("Personaje apuntado: " + aimedActor);
                    // Se reinicia el valor de la puntería para el siguiente input
                    aimedActor = -1;
                    // El personaje apuntado se guarda en los datos de la habilidad
                    _abilitysData[currentActor, abilitySelected, 6] = aimedActor;
                    // Se reinician los valores
                    RestartValues_Selection();
                }
                break;
            case 1:
                if (aimedActor > -1)
                {
                    Debug.Log("Personaje apuntado: " + aimedActor + quantityOfAlies);
                    // Se reinicia el valor de la puntería para el siguiente input
                    aimedActor = -1;
                    // El personaje apuntado se guarda en los datos de la habilidad
                    _abilitysData[currentActor, abilitySelected, 6] = aimedActor + quantityOfAlies;
                    // Se reinician los valores
                    RestartValues_Selection();
                }
                break;
            case 2:
                // El personaje apuntado se guarda en los datos de la habilidad
                _abilitysData[currentActor, abilitySelected, 6] = 6;
                // Se reinician los valores
                RestartValues_Selection();
                break;
            case 3:
                // El personaje apuntado se guarda en los datos de la habilidad
                _abilitysData[currentActor, abilitySelected, 6] = 7;
                // Se reinician los valores
                RestartValues_Selection();
                break;
            case 4:
                // El personaje apuntado se guarda en los datos de la habilidad
                _abilitysData[currentActor, abilitySelected, 6] = 8;
                // Se reinician los valores
                RestartValues_Selection();
                break;
        }
    }

    void RestartValues_Selection()
    {
        print("Estoy en Restart Values de Battle Manager");
        canAct = false; // El siguiente personaje no puede actuar hasta que sea revisado
        currentActor++; // Pasar al siguiente actor para checar sus habilidades
        actorSuccesion = 0; // reiniciar la sucesion a la seleccion de habilidades
        aimedActor = -1; // Reiniciar actor apuntado
        possibleAimedActor = -1; // Reiniciar posible actor siendo apuntado
        abilitySelected = -1; // Reiniciar habilidad seleccionada
        // Reiniciar valores de UI
        sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
    }

    void EnemiesAbilitiesSelections()
    {
        //_actorsData[0, 9] = 1;
        // Checar si el personaje puede actuar
        if (quantityOfEnemies + quantityOfAlies > currentActor)
        {
            // Checar si el actor esta disponible
            if (_actorsData[currentActor, 9] == 0) // si el actor esta disponible para actuar
            {
                print("El personaje puede actuar");
                canAct = true;

                // ESTO NO ES LO MEJOR PORQUE ESTÁ REINICIANDO LOS VALORES DEL UI CADA FRAME
                // HAY QUE ENCONTRAR LA MANERA DE HACER QUE UNA VEZ QUE CHEQUE QUE EL PERSONAJE PUEDA ACTUAR
                // ENTONCES ACCEDA A ESTO UNA VEZ
                sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
            }
            else // si el actor no esta disponible para actuar
            {
                print("El personaje no puede actuar, reiniciar valores");
                // REINICIA LOS VALORES PARA PASAR AL SIGUIENTE PERSONAJE
                RestartValues_Selection();
            }
        }
        else
        {
            // terminar seleccion de habilidades de aliados y pasar a seleccion de habilidades de enemigos
            battleSuccesion = 1;
        }

        if (canAct == true)
        {
            // INPUTS TEMPORALES
            if (Input.GetKeyDown(KeyCode.Q)) { abilCheckInput = true; abilitySelected = 0; }
            if (Input.GetKeyDown(KeyCode.W)) { abilCheckInput = true; abilitySelected = 1; }
            if (Input.GetKeyDown(KeyCode.E)) { abilCheckInput = true; abilitySelected = 2; }
            // Esperar Input
            if (abilCheckInput)
            {
                // No puede actuar (No puede seleccionar otra habilidad)
                canAct = false;
                // Reiniciar esta variable para el siguiente check
                abilCheckInput = false;
                // La habilidad seleccionada se establece en los valores del actor
                _actorsData[currentActor, 14] = abilitySelected;
                // La posible punteria se guarda en Posible Aimed Actor
                possibleAimedActor = _abilitysData[currentActor, abilitySelected, 5];
                // Imprimir la habilidad elegida
                print("La habilidad de elijió el aliado es " + _actorsData[currentActor, 14]);
                // Imprimir posible puntería
                print("El posible personaje apuntado es: " + _abilitysData[currentActor, abilitySelected, 5]);
                // Reiniciar variables del UI
                sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
                // IR A LA SIGUIENTE PARTE DE ACTOR SUCCESION
                actorSuccesion = 1;
            }
        }
    }

    void SpeedDetector()
    {
        // ESTOS NUMEROS SON DE PRUEBA
        /*
        charsVelocity[1,0] = 4;// Actor 1
        charsVelocity[1,1] = 7;// Actor 2
        charsVelocity[1,2] = 3;// Actor 3
        enemsVelocity[1,0] = 1;// Actor 4
        enemsVelocity[1,1] = 2;// Actor 5
        enemsVelocity[1,2] = 9;// Actor 6
        */
        _actorsData[0, 7] = 4;
        _actorsData[1, 7] = 7;
        _actorsData[2, 7] = 3;
        _actorsData[3, 7] = 1;
        _actorsData[4, 7] = 2;
        _actorsData[5, 7] = 9;

        _actorsData[0, 7] = 30;
        int[] speedInfoKeeper;
        speedInfoKeeper = new int[6] { 0, 0, 0, 0, 0, 0 };
        int actorOrderCounter = 0;
        // Checar la velocidad de cada actor del juego (Con maximo 30)
        for (int i = 30; i > 0; i--)
        {
            for (int j = 0; j < (quantityOfAlies + quantityOfEnemies); j++)
            {
                if (_actorsData[j, 7] == i)
                {
                    print("Entre a checar la velocidad");
                    actorSpeedSelector[actorOrderCounter] = j;
                    speedInfoKeeper[j] = _actorsData[j, 7];
                    _actorsData[j, 7] = -100; // Para evitar rechecarlo
                    actorOrderCounter++;
                }
                /*
                if (j < 3){
                    if (charsVelocity[1,j] >= i){
                        actorSpeedSelector[actorOrderCounter] = j;
                        speedInfoKeeper[j] = charsVelocity[1,j];
                        charsVelocity[1,j] = -100;
                        actorOrderCounter++;
                        //print(actorSpeedSelector[actorOrderCounter]);
                    }
                } else if (j >= 3){
                    if (enemsVelocity[1,(j-3)] >= i){
                        actorSpeedSelector[actorOrderCounter] = j;
                        speedInfoKeeper[j] = enemsVelocity[1,(j-3)];
                        enemsVelocity[1,(j-3)] = -100;
                        actorOrderCounter++;
                        //print(actorSpeedSelector[actorOrderCounter]);
                    }
                }
                */
            }
        }
        for (int i = 0; i < 6; i++)
        {
            print("ActorNumber: " + actorSpeedSelector[i]);
        }
        for (int i = 0; i < 6; i++)
        {
            print("Spd: " + speedInfoKeeper[i]);
            _actorsData[i, 7] = speedInfoKeeper[i];
            /*
            if (i < 3){
                charsVelocity[1,i] = speedInfoKeeper[i];
            }
            if (i >= 3){
                enemsVelocity[1,(i-3)] = speedInfoKeeper[i];
            }
            */
        }
        //battleState = 3;

        // Encontrar animadores de los actores
        /*
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                actorAnimators[i] = chars[i].transform.GetChild(0).
                gameObject.GetComponent<Animator>();
                //print ("Se encontro animador de actor " + i);
            }
            if (i >= 3)
            {
                actorAnimators[i] = enems[i - 3].transform.GetChild(0).
                gameObject.GetComponent<Animator>();
                //print ("Se encontro animador de actor " + i);
            }
        }
        */
        battleSuccesion = 3;
    }

    void CombatMomentFunction()
    {

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
                    _actorsData[i, 2] = quantityOfEnemies + 3;
                    // Se aumenta la cantidad de enemigos por 1
                    quantityOfEnemies++;
                }
                else {
                    // Igualar el personaje como aliado
                    _actorsData[i, 1] = 1;
                    // Asignar una posicion para el aliado dependiendo de la cantidad de aliados
                    _actorsData[i, 2] = quantityOfAlies;
                    // Se aumenta la cantidad de aliados por 1
                    quantityOfAlies++;
                }
                _actorsData[i, 3] = -1; // Habilidad seleccionada, ninguna
                _actorsData[i, 5] = actorSCOB[i].actorsData[0]; // Vida Máxima
                print(_actorsData[i, 5]);
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