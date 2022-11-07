using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public ScBtl_Motor sceneBtl_Motor;
    //private GameObject[] actors; // Objetos de los actores
    private ActorMotor[] actorsMotor; // Codigos de los objetos de los actores
    public Transform[] actorsTransPos; // Posiciones de los actores, las que deben de tener
    private Transform confrontPosAllies; // Posicion de confrontacion de los aliados
    private Transform confrontPosEnemies; // Posicion de confrontacion de los enemigos

    private GameObject fatherObj; // OBJETO PADRE DE TODOS OBJETOS (ACTORES Y POSICIONES)
    //public ActorScOb[] actorSCOB; // SCRIPTABLE OBJECTS DE LOS ACTORES
    public int[] SCOBactorPos; // Posicion del personaje segun el objeto del actor y el objeto de la posicion
                               // (0 - 2 para los aliados, 3 - 5 para los enemigos)

    int quantityOfAllies = 0; // Cantidad de aliados en la escena
    int quantityOfEnemies = 0; // Cantidad de enemigos en la escena

    // Variables Publicas

    public int currentActor = 0; // Variable que determina cual es el actor en ese momento que está actuando
    public int abilitySelected = -1; // HABILIDAD SELECCIONADA
    public int aimedActor = -1; // PERSONAJE APUNTADO PARA LA HABILIDAD
    public int currentActingActor; // El personaje que en este momento está actuando (Es modificable por la variable de velocidad)
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
        currentActingActor = -1; // No hay ningun actor actuando en este momento 

        actorSpeedSelector = new int[6] { 0, 0, 0, 0, 0, 0 }; // Seleccionador de quien toca primero

        // Declarar Objetos
        actorsMotor = new ActorMotor[20];
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
            actorsMotor[i] = fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject.GetComponent<ActorMotor>();
            // Encontrar posiciones
            if (i < 3) {
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Ally" + i + "Pos");
            } else
            {
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Enemy" + i + "Pos");
            }
        }

    }

    void Update()
    {

        //print("Personaje apuntado general: " + aimedActor);
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
                EnemiesSelections();
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
        // Checar si el personaje puede actuar
        if (quantityOfAllies > currentActor)
        {
            // Checar si el actor esta disponible
            //if (_actorsData[currentActor, 9] == 0) // si el actor esta disponible para actuar
            if (actorsMotor[currentActor].actorsData[10] == 0) // si el actor esta disponible para actuar
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
                actorsMotor[currentActor].actorsData[1] = abilitySelected;
                // La posible punteria se guarda en Posible Aimed Actor
                possibleAimedActor = actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1];
                // Imprimir la habilidad elegida
                print("La habilidad de elijió el aliado es " + actorsMotor[currentActor].actorsData[1]);
                // Imprimir posible puntería
                print("El posible personaje apuntado es: " + actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1]);
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
        if (possibleAimedActor < 3)
        {
            // 0 - Apunta a todos los aliados, 1 - Apunta a todos los enemigos, 2 - Apunta a si mismo
            //_abilitysData[currentActor, abilitySelected, 6] = possibleAimedActor;
            actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = possibleAimedActor;
        } else {
            switch (possibleAimedActor)
            {
                /*
                case 0:
                    // Apunta a todos los aliados
                    // El personaje apuntado se guarda en los datos de la habilidad
                    _abilitysData[currentActor, abilitySelected, 6] = 0;
                    // Se reinician los valores
                    RestartValues_Selection();
                    break;
                case 1:
                    // Apunta a todos los enemigos
                    // El personaje apuntado se guarda en los datos de la habilidad
                    _abilitysData[currentActor, abilitySelected, 6] = 1;
                    // Se reinician los valores
                    RestartValues_Selection();
                    break;
                case 2:
                    // Apunta a si mismo
                    // El personaje apuntado se guarda en los datos de la habilidad
                    _abilitysData[currentActor, abilitySelected, 6] = 2;
                    // Se reinician los valores
                    RestartValues_Selection();
                    break;
                */
                case 3:
                    // Apunta a un aliado
                    if (aimedActor > -1)
                    {
                        Debug.Log("Personaje apuntado: " + aimedActor);
                        // Se reinicia el valor de la puntería para el siguiente input
                        aimedActor = -1;
                        // El personaje apuntado se guarda en los datos de la habilidad
                        actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = aimedActor + 2/*Este numero es para no contradecir los numeros de arriba*/;
                        // Se reinician los valores
                        RestartValues_Selection();
                    }
                    break;
                case 4:
                    // Apunta a un enemigo
                    if (aimedActor > -1)
                    {
                        Debug.Log("Personaje apuntado: " + aimedActor + quantityOfAllies);
                        // Se reinicia el valor de la puntería para el siguiente input
                        aimedActor = -1;
                        // El personaje apuntado se guarda en los datos de la habilidad
                        actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = aimedActor + quantityOfAllies + 2/*Este numero es para no contradecir los numeros de arriba*/;
                        // Se reinician los valores
                        RestartValues_Selection();
                    }
                    break;
            }
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

    void EnemiesSelections()
    {
        // Checar si el personaje puede actuar
        if (quantityOfAllies + quantityOfEnemies > currentActor)
        {
            // Checar si el actor esta disponible
            //if (_actorsData[currentActor, 9] == 0) // si el actor esta disponible para actuar
            if (actorsMotor[currentActor].actorsData[10] == 0) // si el actor esta disponible para actuar
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
            abilitySelected = UnityEngine.Random.Range(0, 3);
            print("Habilidad seleccionada del enemigo: " + abilitySelected);
            // No puede actuar (No puede seleccionar otra habilidad)
            canAct = false;
            // Reiniciar esta variable para el siguiente check
            abilCheckInput = false;
            // La habilidad seleccionada se establece en los valores del actor
            actorsMotor[currentActor].actorsData[1] = abilitySelected;
            // La posible punteria se guarda en Posible Aimed Actor
            possibleAimedActor = actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1];
            // Imprimir la habilidad elegida
            print("La habilidad de elijió el aliado es " + actorsMotor[currentActor].actorsData[1]);
            // Imprimir posible puntería
            print("El posible personaje apuntado es: " + actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1]);
            // Reiniciar variables del UI
            sceneBtl_Motor.UpdateUI_Variables(canAct, currentActor, possibleAimedActor);
            // IR A LA SIGUIENTE PARTE DE ACTOR SUCCESION
            actorSuccesion = 1;
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

        actorsMotor[0].actorsData[4] = 4;
        actorsMotor[1].actorsData[4] = 7;
        actorsMotor[2].actorsData[4] = 3;
        actorsMotor[3].actorsData[4] = 1;
        actorsMotor[4].actorsData[4] = 2;
        actorsMotor[5].actorsData[4] = 9;

        //actorsMotor[6].actorsData[4] = 10;
        int[] speedInfoKeeper;
        speedInfoKeeper = new int[6] { 0, 0, 0, 0, 0, 0 };
        int actorOrderCounter = 0;
        // Checar la velocidad de cada actor del juego (Con maximo 30)
        for (int i = 30; i > 0; i--)
        {
            for (int j = 0; j < (quantityOfAllies + quantityOfEnemies); j++)
            {
                if (actorsMotor[j].actorsData[4] == i)
                {
                    print("Entre a checar la velocidad");
                    actorSpeedSelector[actorOrderCounter] = j;
                    speedInfoKeeper[j] = actorsMotor[j].actorsData[4];
                    actorsMotor[j].actorsData[4] = -100; // Para evitar rechecarlo
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
            actorsMotor[i].actorsData[4] = speedInfoKeeper[i];
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
        if ((quantityOfAllies + quantityOfEnemies) > currentActor)
        {
            // Lista de personajes : actorSpeedSelector
            // 
        }
        if (Input.GetKeyDown(KeyCode.K)) { currentActingActor++; }
        if (currentActingActor == 2)
        {
            actorsMotor[0].EjecuteAbility();
        }
        if (actorsMotor[0].actorIsActing == false && currentActingActor == 2)
        {
            currentActingActor++;
        }
        for (int i = 0; i < 10; i++)
        {

        }
    }
}