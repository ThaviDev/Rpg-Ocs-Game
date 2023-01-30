using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class BattleManager : MonoBehaviour
{
    public ScBtl_Motor sceneBtl_Motor;
    //private GameObject[] actors; // Objetos de los actores
    //public ActorMotor[] actorsMotor; // Codigos de los objetos de los actores
    public List<ActorMotor> actorsMotor = new List<ActorMotor>();
    public Transform[] actorsTransPos; // Posiciones de los actores, las que deben de tener
    //public List<ActorMotor> listActorsMotor = new List<ActorMotor>();
    //public List<Transform> listActorsPos = new List<Transform>();
    private Transform confrontPosAllies; // Posicion de confrontacion de los aliados
    private Transform confrontPosEnemies; // Posicion de confrontacion de los enemigos

    private GameObject fatherObj; // OBJETO PADRE DE TODOS OBJETOS (ACTORES Y POSICIONES)
    //public ActorScOb[] actorSCOB; // SCRIPTABLE OBJECTS DE LOS ACTORES
    public int[] SCOBactorPos; // Posicion del personaje segun el objeto del actor y el objeto de la posicion
                               // (0 - 2 para los aliados, 3 - 5 para los enemigos)

    public int quantityOfAllies = 0; // Cantidad de aliados en la escena
    public int quantityOfEnemies = 0; // Cantidad de enemigos en la escena

    public GameObject actionObj; // SCOB de la acción, todas las acciones son iguales, se declara diferencias en el codigo

    public ActionMotor myAction; // La acción que establece el código y la mueve hacia su padre objeto

    // Variables Publicas

    public int currentActor = 0; // Variable que determina cual es el actor en ese momento que está actuando
    public int abilitySelected = -1; // HABILIDAD SELECCIONADA
    public int aimedActor = -1; // PERSONAJE APUNTADO PARA LA HABILIDAD
    public int currentActingActor; // El personaje que en este momento está actuando (Es modificable por la variable de velocidad)
    public float possibleAimedActor = -1; // POSIBLE PERSONAJE APUNTADO (PARA ESTABLECER EL PERSONAJE APUNTADO)
    public bool abilCheckInput = false; // Checar si existió un input para la habilidad de un aliado
    public bool isAvaliable = false; // Variable para checar si el personaje puede actuar
    public bool canAct = true; // Si el actor que está ejecutando habilidad lo está haciendo
    public bool isActing; // Si el actor está actuando en este momento, sirve para no repetir la misma accion una y otra vez

    public int battleSuccesion = 0; // SUCCESION DE EVENTOS DE LA BATALLA EN GENERAL
    public int actorSuccesion = 0; // SUCCESION DE EVENTOS DE LA SELECCIÓN Y MODIFICACIÓN DE DATOS DE LOS ACTORES
    private int[] actorSpeedSelector; // VARIABLE QUE GUARDA EL ORDEN DE QUIEN ACTUA PRIMERO A QUIEN ACTUA ULTIMO EN LOS PERSONAJES

    void Start()
    {
        // Encontrar Battle Motor
        sceneBtl_Motor = FindObjectOfType<ScBtl_Motor>();

        // Declarar variables
        isAvaliable = false; // El siguiente personaje no puede actuar hasta que sea revisado
        aimedActor = -1; // No hay personaje apuntado
        possibleAimedActor = -1; // lo de arriba xd
        currentActor = 0; // El primer actor es el que se checa (el 0)
        currentActingActor = -1; // No hay ningun actor actuando en este momento 

        actorSpeedSelector = new int[6] { 0, 0, 0, 0, 0, 0 }; // Seleccionador de quien toca primero

        // Declarar Objetos
        //actorsMotor = new ActorMotor[6];
        actorsTransPos = new Transform[6];

        //Encontrar Objeto padre
        fatherObj = GameObject.Find("BattleAssets");
        //Declarar Hijos de los padres
        Transform fatherObj_Actors = fatherObj.gameObject.transform.Find("Actors");
        Transform fatherObj_Positions = fatherObj.gameObject.transform.Find("Positions");

        // Encontrar Objetos de las posiciones de confrontación
        confrontPosAllies = fatherObj_Positions.gameObject.transform.Find("AttackPosAllies");
        confrontPosEnemies = fatherObj_Positions.gameObject.transform.Find("AttackPosEnems");

        for (int i = 0; i < 6; i++)
        {
            // HACER UN TRY CATCH PARA CUANDO EXISTAN MÁS O MENOS ACTORES
            // Encontrar actores
            // actorsMotor[i] = fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject.GetComponent<ActorMotor>();
            try
            {
                //listActorsMotor.Add(fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject.GetComponent<ActorMotor>());
                actorsMotor.Add(fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject.GetComponent<ActorMotor>());
            }
            catch
            {
            }
            // Encontrar posiciones
            if (i < 3)
            {
                //listActorsPos.Add(fatherObj_Positions.gameObject.transform.Find("Ally" + i + "Pos"));
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Ally" + i + "Pos");
            }
            else
            {
                //listActorsPos.Add(fatherObj_Positions.gameObject.transform.Find("Enemy" + i + "Pos"));
                actorsTransPos[i] = fatherObj_Positions.gameObject.transform.Find("Enemy" + i + "Pos");
            }
        }
        /*
        actorsMotor = new ActorMotor[listActorsMotor.Count];
        actorsMotor = listActorsMotor.ToArray();
        /*
        for (int i = 0; i < listActorsMotor.Count; i++)
        {
            actorsMotor[i] = listActorsMotor[i];
        }*/

        for (int i = 0; i < actorsMotor.Count; i++)
        {
            if (actorsMotor[i].actorScOb.isEnemy == false)
            {
                actorsMotor[i].actorsData[0] = quantityOfAllies;
                quantityOfAllies++;
            }
            else
            {
                actorsMotor[i].actorsData[0] = quantityOfEnemies + quantityOfAllies;
                quantityOfEnemies++;
            }
        }
    }

    void Update()
    {
        /* Succesion de la batalla: 
         * Los aliados eligen sus habilidades y a que personaje apuntan con ayuda del input del jugador y los enemigos eligen sus habilidades aleatoriamente (por ahora)
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
                SpeedDetector();
                //print("Hay que calcular la velocidad de los actores");
                break;
            case 2:
                CombatMomentFunction();
                //print("Que comience la batalla");
                break;
                /*
            default:
                if (battleSuccesion == 0 || battleSuccesion == 1)
                    ActorsSelection();
                break;
                */
        }
    }
    void ActorsSelection()
    {
        switch (actorSuccesion)
        {
            case 0:
                /*
                // Se checa la seleccion de habilidades
                if (battleSuccesion == 0)
                {
                    // Si es para los personajes aliados
                    AbilitySelection();

                } else
                {
                    // Si es para los personajes enemigos
                    EnemiesSelections();
                }
                */
                AbilitySelection();
                //AbilitySelection();
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
        if (quantityOfAllies + quantityOfEnemies > currentActor)
        {
            // Checar si el actor esta disponible
            //if (_actorsData[currentActor, 9] == 0) // si el actor esta disponible para actuar
            if (actorsMotor[currentActor].actorsData[13] == 0) // si el actor esta disponible para actuar
            {
                //print("El personaje puede actuar");
                isAvaliable = true;

                // ESTO NO ES LO MEJOR PORQUE ESTÁ REINICIANDO LOS VALORES DEL UI CADA FRAME
                // HAY QUE ENCONTRAR LA MANERA DE HACER QUE UNA VEZ QUE CHEQUE QUE EL PERSONAJE PUEDA ACTUAR
                // ENTONCES ACCEDA A ESTO UNA VEZ
                //sceneBtl_Motor.UpdateUI_Variables(isAvaliable, currentActor, (int)possibleAimedActor);
            }
            else // si el actor no esta disponible para actuar
            {
                //print("El personaje no puede actuar, reiniciar valores");
                // REINICIA LOS VALORES PARA PASAR AL SIGUIENTE PERSONAJE
                RestartValues_Selection();
            }
        }
        else
        {
            // terminar seleccion de habilidades de aliados y pasar a seleccion de habilidades de enemigos
            battleSuccesion = 1;
            currentActor = 0;
        }

        if (isAvaliable == true)
        {
            // INPUTS TEMPORALES
            if (Input.GetKeyDown(KeyCode.Q)) { abilCheckInput = true; abilitySelected = 0; }
            if (Input.GetKeyDown(KeyCode.W)) { abilCheckInput = true; abilitySelected = 1; }
            if (Input.GetKeyDown(KeyCode.E)) { abilCheckInput = true; abilitySelected = 2; }
            // Checa si el actor es un enemigo
            if (actorsMotor[currentActor].actorScOb.isEnemy)
            {
                // Es aleatorio que habilidad selecciona
                abilitySelected = UnityEngine.Random.Range(0, 3);
                // Es aleatorio a que personaje apunta (si no apunta a ninguno no importa)
                // porque se reinicia en Restart Values Selections
                aimedActor = UnityEngine.Random.Range(0, quantityOfAllies);
                // Acceder a las opciones de Input
                abilCheckInput = true;
            }
            // Esperar Input
            if (abilCheckInput)
            {
                // No puede actuar (No puede seleccionar otra habilidad)
                isAvaliable = false;
                // Reiniciar esta variable para el siguiente check
                abilCheckInput = false;
                // La habilidad seleccionada se establece en los valores del actor
                actorsMotor[currentActor].actorsData[1] = abilitySelected;
                // La posible punteria se guarda en Posible Aimed Actor
                possibleAimedActor = actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1];
                // Imprimir la habilidad elegida
                //print("La habilidad de elijió el aliado es " + actorsMotor[currentActor].actorsData[1]);
                // Imprimir posible puntería
                //print("El posible personaje apuntado es: " + actorsMotor[currentActor].actorScOb.abilities[abilitySelected].abilityData[1]);
                // Reiniciar variables del UI
                //sceneBtl_Motor.UpdateUI_Variables(isAvaliable, currentActor, (int)possibleAimedActor);
                // IR A LA SIGUIENTE PARTE DE ACTOR SUCCESION
                actorSuccesion = 1;
            }
        }
    }

    void CheckAim()
    {
        //print("Estoy en checar puntería");
        // INPUTS TEMPORALES
        if (Input.GetKeyDown(KeyCode.A)) { aimedActor = 0; }
        if (Input.GetKeyDown(KeyCode.S)) { aimedActor = 1; }
        if (Input.GetKeyDown(KeyCode.D)) { aimedActor = 2; }

        switch (possibleAimedActor)
        {
            /*
            case 0:
                // Apunta a todos los aliados
                // El personaje apuntado se guarda en los datos de la habilidad como objetivo seleccionado
                //_abilitysData[currentActor, abilitySelected, 6] = 0;
                actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = 0;
                print("Apunta a todos los aliados");
                // Se reinician los valores
                RestartValues_Selection();
                break;
            case 1:
                // Apunta a todos los enemigos
                // El personaje apuntado se guarda en los datos de la habilidad
                //_abilitysData[currentActor, abilitySelected, 6] = 1;
                actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = 1;
                print("Apunta a todos los enemigos");
                // Se reinician los valores
                RestartValues_Selection();
                break;
            case 2:
                // Apunta a si mismo
                // El personaje apuntado se guarda en los datos de la habilidad
                //_abilitysData[currentActor, abilitySelected, 6] = 2;
                actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = 2;
                print("Apunta a si mismo");
                // Se reinician los valores
                RestartValues_Selection();
                break;
            */
            case 3:
                // Apunta a un aliado
                if (aimedActor > -1)
                {
                    //Debug.Log("Personaje apuntado: " + aimedActor);
                    // El personaje apuntado se guarda en los datos de la habilidad
                    actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = aimedActor + 3;
                    // Se reinician los valores
                    RestartValues_Selection();
                }
                break;
            case 4:
                // Apunta a un enemigo
                if (aimedActor > -1)
                {
                    //Debug.Log("Personaje apuntado: " + aimedActor + quantityOfAllies);
                    // El personaje apuntado se guarda en los datos de la habilidad
                    actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = aimedActor + quantityOfAllies + 3;
                    // Se reinician los valores
                    RestartValues_Selection();
                }
                break;
            default:
                // El objetivo posible de la habilidad seleccionada del personaje seleccionado

                //print("Apunte a algun general");
                //0 - Apunta a todos los aliados, 1 - Apunta a todos los enemigos, 2 - Apunta a si mismo
                actorsMotor[currentActor].abilitiesData[abilitySelected, 3] = possibleAimedActor;
                // Se reinician los valores
                RestartValues_Selection();
                break;
        }
    }

    void RestartValues_Selection()
    {
        //print("Estoy en Restart Values de Battle Manager");
        isAvaliable = false; // El siguiente personaje no puede actuar hasta que sea revisado
        currentActor++; // Pasar al siguiente actor para checar sus habilidades
        actorSuccesion = 0; // reiniciar la sucesion a la seleccion de habilidades
        aimedActor = -1; // Reiniciar actor apuntado
        possibleAimedActor = -1; // Reiniciar posible actor siendo apuntado
        abilitySelected = -1; // Reiniciar habilidad seleccionada
        // Reiniciar valores de UI
        sceneBtl_Motor.UpdateUI_Data();
    }

    void SpeedDetector()
    {
        /*
        actorsMotor[0].actorsData[7] = 5;
        actorsMotor[1].actorsData[7] = 5;
        actorsMotor[2].actorsData[7] = 5;
        actorsMotor[3].actorsData[7] = 3;
        actorsMotor[4].actorsData[7] = 3;
        actorsMotor[5].actorsData[7] = 3;
        */
        

        //actorsMotor[6].actorsData[4] = 10;
        float[] speedInfoKeeper;
        speedInfoKeeper = new float[actorsMotor.Count];
        int actorOrderCounter = 0;
        // Checar la velocidad de cada actor del juego (Con maximo 30)
        for (int i = 30; i > 0; i--)
        {
            for (int j = 0; j < (quantityOfAllies + quantityOfEnemies); j++)
            {
                if (actorsMotor[j].actorsData[7] == i)
                {
                    print("Entre a checar la velocidad");
                    actorSpeedSelector[actorOrderCounter] = j;
                    speedInfoKeeper[j] = actorsMotor[j].actorsData[7];
                    actorsMotor[j].actorsData[7] = -100; // Para evitar rechecarlo
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
        for (int i = 0; i < actorsMotor.Count; i++)
        {
            //print("ActorNumber: " + actorSpeedSelector[i]);
        }
        for (int i = 0; i < actorsMotor.Count; i++)
        {
            //print("Spd: " + speedInfoKeeper[i]);
            actorsMotor[i].actorsData[7] = speedInfoKeeper[i];
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
        battleSuccesion = 2;
    }
    void CombatMomentFunction()
    {
        if ((quantityOfAllies + quantityOfEnemies) > currentActor)
        {
            //print("Estoy en el combate Oh si, y soy el actor numero: " + actorSpeedSelector[currentActor] + " osea el actor numero " + currentActor + " de la lista");
            //print(actorSpeedSelector[0] + " " + actorSpeedSelector[1] + " " + actorSpeedSelector[2] + " " + actorSpeedSelector[3]);
            currentActingActor = actorSpeedSelector[currentActor];
            // Si el actor está disponible
            if (actorsMotor[currentActingActor].actorsData[13] == 0)
            {
                canAct = true;
            } else
            {
                canAct = false;
                currentActor++;
            }
            // Lista de personajes en orden de velocidad : actorSpeedSelector
        }
        else
        {
            canAct = false;
            if (isActing == false)
            {
                // Reiniciar valores de UI
                sceneBtl_Motor.UpdateUI_Data();
                // Reiniciar el actor actual y la sucesión de batalla
                currentActor = 0;
                battleSuccesion = 0;
                // Checar si se logro la victoria o la derrota
                BattleEndCheck();
                //print("Se acabaron las acciones, hora de seleccionar de nuevo");
            }
        }

        if (canAct == true && isActing == false)
        {
            isActing = true;
            StartCoroutine("animEjecution");
        }
    }

    IEnumerator animEjecution()
    {
        ActorScOb _actorScOb = actorsMotor[currentActingActor].actorScOb;
        float[] _actorsData = actorsMotor[currentActingActor].actorsData;
        actorsMotor[currentActingActor].actorIsActing = true;
        isActing = true;

        //yield return new WaitForSeconds((_actorScOb.abilities[_actorsData[1]].abilityData[4] / 100) + 2);
        //print("La cantidad de acciones de esta habilidad es: " + _actorScOb.abilities[(int)_actorsData[1]].actionAmount);
        // Spawnear Acciones
        for (int i = 0; i < _actorScOb.abilities[(int)_actorsData[1]].actionAmount; i++)
        {
            //print("Estoy empezando a spawnear acciones, y mi numero es " + _actorScOb.abilities[(int)_actorsData[1]].actionAmount);
            // Checar si alguna de estas acciones se spawnea mas de una vez, viendo el objetivo de la acción
            switch (_actorScOb.abilities[(int)_actorsData[1]].actionObjective[i])
            {
                // Objeto spawnea en todos los aliados
                case 0:
                    for (int j = 0; j < quantityOfAllies; j++)
                    {
                        StartCoroutine(InstantiateAnAction(j, i));
                    }
                    break;
                // Objeto espawnea en todos los enemigos
                case 1:
                    for (int j = 0; j < quantityOfEnemies; j++)
                    {
                        StartCoroutine(InstantiateAnAction(j + quantityOfAllies, i));
                    }
                    break;
                // Objeto espawnea en el personaje el cual lo invocó
                case 2:
                    StartCoroutine(InstantiateAnAction(currentActingActor, i));
                    break;
                // Objeto espawnea en el objetivo seleccionado por la habilidad
                case 3:
                    // De los datos de la habilidad se busca la habilidad seleccionada en los datos del actor y el objetivo seleccionado de los datos de la habilidad (3)
                    // actorsMotor[currentActingActor].abilitiesData[actorsMotor[currentActor].actorsData[1], 3]
                    float objSelAbil = actorsMotor[currentActingActor].abilitiesData[(int)_actorsData[1], 3];
                    // Si el objetivo seleccionado es menor a 3 (0,1,2) entonces es una generalización
                    if (objSelAbil < 3)
                    {
                        // Esto es una copia de lo de arriba, no se como ahorrar esto o de alguna manera repetir lo de arriba sin joder todo así que equis de
                        switch (objSelAbil)
                        {
                            // Objeto spawnea en todos los aliados
                            case 0:
                                for (int j = 0; j < quantityOfAllies; j++)
                                {
                                    StartCoroutine(InstantiateAnAction(j, i));
                                }
                                break;
                            // Objeto espawnea en todos los enemigos
                            case 1:
                                for (int j = 0; j < quantityOfEnemies; j++)
                                {
                                    StartCoroutine(InstantiateAnAction(j + quantityOfAllies, i));
                                }
                                break;
                            // Objeto espawnea en el personaje el cual lo invocó
                            case 2:
                                StartCoroutine(InstantiateAnAction(currentActingActor, i));
                                break;
                        }
                    }
                    else
                    {
                        // El objetivo seleccionado de la habilidad siendo mayor o igual a 3 entonces tiene que ser un actor
                        // Entonces se instancia la acción al actor siendo seleccionado y ya
                        StartCoroutine(InstantiateAnAction((int)objSelAbil - 3, i));
                    }
                    break;
            }

            /*
            Instantiate(actionObj); // Instanciar la acción como objeto
            actionObj = FindObjectOfType<ActionMotor>(); // Encontrarla y guardarla
            actionObj.actionData = new int[5]; // clasificar un array con 5 elementos
            // establecer todos los datos correspondientes a la acción
            actionObj.actionData[0] = _actorScOb.abilities[_actorsData[1]].classification[i];
            actionObj.actionData[1] = _actorScOb.abilities[_actorsData[1]].applic_Type[i];
            actionObj.actionData[2] = _actorScOb.abilities[_actorsData[1]].quantity[i];
            actionObj.actionData[3] = _actorScOb.abilities[_actorsData[1]].applic_Turn[i];
            actionObj.actionData[4] = _actorScOb.abilities[_actorsData[1]].duration[i];
            actionObj.transform.SetParent(this.gameObject.transform);
            //actionObj.actionData[0] = actionDta;
            //actionObj.transform.SetParent(actorsMotor[i].gameObject.transform);

        }

        yield return new WaitForSeconds((_actorScOb.abilities[_actorsData[1]].abilityData[5] / 100) + 2);
        // Terminar de ejecutar
        //actorIsActing = false;
            */
            // Esperar a que termine de ejecutarse la habilidad para continuar

        }
        yield return new WaitForSeconds((_actorScOb.abilities[(int)_actorsData[1]].abilityData[4] / 100) +
        (_actorScOb.abilities[(int)_actorsData[1]].abilityData[5] / 100));
        //print("Ok termine x2");
        // Permitir acciones accionar en el actor
        actorsMotor[currentActingActor].StartCoroutine("CanAction");
        // Actualizar los datos de la interfaz
        sceneBtl_Motor.UpdateUI_Data();
        // Ya no se está actuando
        isActing = false;
        actorsMotor[currentActingActor].actorIsActing = false;
        currentActor++;

    }

    /* CREO QUE SERÍA BUENA IDEA UTILIZAR EL ANIMADOR PARA PODER INSTANCIAR LAS ACCIONES, ASÍ SE PUEDE SER EXACTAMENTE PRECISO EN EL 
     * INSTANTE EN EL QUE SE DEBERÍA DE INSTANCIAR, SE PUEDE EJECUTAR UN CÓDIGO O ALGUN TIPO DE TRIGGER EN EL ANIMATOR
     * 
     * ENTONCES QUIZÁ SEA BUENA IDEA EL LLAMAR AL ANIMADOR CON EL animEjecution() Y MANDARLE LA INFORMACIÓN A UNA FUNCION DE AQUÍ MISMO CUANDO SEA EJECUTADA
     * ASÍ NO SERÍA OTRA CORRUTINA QUE PUEDA LLEGAR A MOLESTAR CON LOS TIEMPOS DE LA PRIMER CORRUTINA
    */
    IEnumerator InstantiateAnAction(int spawnPos, int actionID)
    {
        sceneBtl_Motor.UpdateUI_Data();
        // Esperar a que se pueda ejecutar la habilidad
        yield return new WaitForSeconds(actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].abilityData[4] / 100);
        //print("PAAAM");

        Instantiate(actionObj); // Instanciar la acción como objeto
        myAction = FindObjectOfType<ActionMotor>(); // Encontrarla y guardarla
        myAction.actionData = new float[6]; // clasificar un array con 5 elementos
                                          // establecer todos los datos correspondientes a la acción

        // Clasificación
        myAction.actionData[0] = actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].classification[actionID];
        // Tipo de aplicacion o efecto
        myAction.actionData[1] = actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].applic_Type[actionID];
        // Cantidad que se aplicara
        myAction.actionData[2] = actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].quantity[actionID];
        // Turno para aplicar
        myAction.actionData[3] = actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].applic_Turn[actionID];
        // Duración en turnos para desaparecer
        myAction.actionData[4] = actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].duration[actionID];
        // Valor de daño del personaje
        myAction.actionData[5] = actorsMotor[currentActingActor].actorsData[5];
        // Objeto del actor al que se le va a aplicar la acción
        myAction.transform.SetParent(actorsMotor[spawnPos].gameObject.transform);
        yield return new WaitForSeconds((actorsMotor[currentActingActor].actorScOb.abilities[(int)actorsMotor[currentActingActor].actorsData[1]].abilityData[5] / 100));
        //print("Ok, termine");
    }

    void BattleEndCheck()
    {
        /*
         * 1 Buscar todos los aliados y ver si todos estan muertos (Si es verdad es derrota)
         * 2 Buscar todos los enemigos y ver si todos estan muertos (Si es verdad es victoria)
         * 3 Si ninguno de los anteriores es verdad entonces no hacer nada
         * */
        bool isDefeat = true;
        bool isVictory = true;
        // Buscar la vida de todos los aliados
        for (int i = 0; i < quantityOfAllies; i++)
        {
            // Si la vida de algun aliado es mayor a 0
            if (actorsMotor[i].actorsData[3] > 0)
                // No pierdes
                isDefeat = false;
        }
        // Si hay derrota
        if (isDefeat == true)
        {
            // Se comunica con Scene Battle Motor para avisarle
            sceneBtl_Motor.CheckEndBattle(isDefeat);
            // Terminar de leer el código
            return;
        }
        // Buscar la vida de todos los enemigos
        for (int i = 0; i < quantityOfEnemies; i++)
        {
            // Si la vida de algun enemigo es mayor a 0
            if (actorsMotor[i + quantityOfAllies].actorsData[3] > 0)
            {
                // No ganas
                isVictory = false;
            }
        }
        // Si ganas
        if (isVictory == true)
        {
            // Se comunica con Scene Battle Motor para avisarle
            sceneBtl_Motor.CheckEndBattle(isVictory);
        }
    }
}