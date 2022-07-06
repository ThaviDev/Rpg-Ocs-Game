using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    private GameObject[] chars; // Personajes jugables
    private Transform[] charsPos; // Posiciones de los personajes jugables
    private GameObject[] enems; // Enemigos
    private Transform[] enemsPos; // Posiciones de los enemigos
    private GameObject fatherObj_Pos, fatherObj_Actors; // Objetos padre
    [SerializeField]
    private Transform charsConfrontPos; // Posiciones de batalla para los personajes
    private Transform enemsConfrontPos; // Posiciones de batalla para los enemigos
    private float[,] actorsData; // Datos de todos los actores
    asda
    /* Primer elemento el Numero del actor - Segundo elemento es el dato
    
    0 Aliado o Enemigo
    1 Vida
    2 Vida Maxima
    3 Velocidad
    4 Velocidad Inicial
    5 Estado de Disponibilidad
    6 * Turnos Antes de Cambio De Estado (Startup - Recovery)

    7 Habilidad Siendo Ejecutada (y Seleccionada)

    * Modificadores de datos (Ejemplo; Mayor curacion cambiaría curacion)
    8 Cambio de Daño
    9 Cambio de Curación
    10 Cambio de Mayor Velocidad
    11 Cambio de Menor Velocidad
    12 Cambio de Precisión
    13 Cambio de Críticos Aleatorios
    */
    private float[,,] abilityData; // Datos de todas las habiliades de todos los actores
    /* Primer elemento Numero del actor - Segundo elemento es la habiliad del actor - Tercer elemento es el dato

    1 Clasificación de habilidad
    2  * Tipo de Habilidad
    3 Tipo de aplicación de efecto (Permanente, Temporal, Sobre Turno)
    4 Tipo de efecto (Suma, Porcentaje, Multiplicación)
    5  * Cantidad que se aplicará
    6 Turnos Startup
    7 Turnos Duración
    8 Turnos Recovery
    9 Precisión
    10 RandomCrits
    11 Objetivo Posible (AbilSelection)
    12 Objetivo Establecido (Target)
    12 Hay Confrontación
    13 Cantidad de Cooldown
    14 Cooldown Presente
    15 ID de Efecto secundario (0 = nada)
    16 ID de Efecto terciario (0 = nada) 
    */
    
    //private int[,] charsStadistics; 
    /*
    Estadisticas de los personajes:
    0 - Primera Habilidad , 1 - Segunda Habilidad, 2 - Tercera Habilidad
    3 - Puntos de Vida, 4 - Puntos de Daño, 5 - Velocidad, 6 - Disponibilidad (1:si, 0:no)
    7 - Esta muerto (1:si, 0:no), 8 - selecionabilidad guardada, 10 - Habilidad escogida
    El primer elemento es el tipo de estadística y el segundo el numero de actor
    */
    /*
    private int[,] charsAbilities;
    // 0 - Primera habilidad Establecida, 1 - Segunda Habilidad, 2 - Tercera Habilidad, 
    // 3 - Habilidad Seleccionada de las 3 que existen (1 = primera habilidad),
    private int[,] charsHealthPoints;
    // 0 - Vida Maxima, 1 - Vida Presente
    //private int[,] charsDamagePoints;
    // 0 - Puntos Predeterminados, 1 - Puntos Presentes
    private int[,] charsVelocity;
    // 0 - Velocidad Predeterminada, 1 - Velocidad Presente
    //private float [,] charsLuck; // Probabilidad de ataque critico
    // 0 - Suerte Predeterminada, 1 - Suerte Presente
    private int[] charsDisponibility;
    // En el primer elemento: 0 = Is avaliable, 1 = Is stuned, 2 = Is performing, 3 = Is dead;
    private int [] charsTarget;
    // En el primer elemento: 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 
    // 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    private int[] charsChargeStartup; // La cantidad de turnos que se va a utilizar para un aliadoprint (abilityNumber);
    private int[] charsChargeRecovery; // La cantidad de turnos que se va a utilizar para un aliadoprint (abilityNumber);
    private int[] charsSavedAbil; // Habilidades guardadas de cada aliado

    private int[,] enemsAbilities;
    // 0 - Primera habilidad Establecida, 1 - Segunda Habilidad, 2 - Tercera Habilidad, 
    // 3 - Habilidad Seleccionada de las 3 que existen (1 = primera habilidad)
    private int[,] enemsHealthPoints;
    // 0 - Vida Maxima, 1 - Vida Presente
    private int[,] enemsDamagePoints;
    // 0 - Puntos Predeterminados, 1 - Puntos Presentes
    private int[,] enemsVelocity;
    // 0 - Velocidad Predeterminada, 1 - Velocidad Presente;
    private int[] enemsDisponibility;
    // En el primer elemento: 0 = Is avaliable, 1 = Is stuned, 2 = Is performing, 3 = Is dead;
    private int [] enemsTarget;
    // En el primer elemento: 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 
    // 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    private int[] enemsChargeStartup; // La cantidad de turnos que se va a utilizar para un enemigo
    private int[] enemsChargeRecovery; // La cantidad de turnos que se va a utilizar para un enemigo
    private int[] enemsSavedAbil; // Habilidades guardadas de cada enemigo


    private float[] abilCharacteristics; // Las características de la habilidad Siendo ejecutada en el presente
    /* elementos
    0 - Tipo de habilidad == 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    1 - Cantidad de efecto == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    2 - Turnos usados == 0 - Es inmediato, 1 - Un turno extra en hacerse, 2 - Dura dos turnos en hacerse, 3 - dura tres turnos, 4...
    3 - Hay confrontacion == 0 - No, 1 - Si 
    4 - Duracion del efecto == 0 - No hay efecto(es inmediato), 1 - Dura un turno, 2 - Dura dos turnos... etc.
    5 - Tiempo del efecto == 20 - Dura veinte segundos la animacion de la habilidad
    6 - Efecto Secundario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    7 - Objetivo del efecto Secundario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    8 - Cantidad de efecto secundario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    9 - Efecto Terciario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    10 - Cantidad de efecto Terciario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    11 - Objetivo del efecto Terciario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    */
    //private int[,] enemsStadistics; 
    /*
    Estadisticas de los enemigos:
    0 - Primera Habilidad , 1 - Segunda Habilidad, 2 - Tercera Habilidad
    3 - Puntos de Vida, 4 - Puntos de Daño, 5 - Velocidad, 6 - Disponibilidad (1:si, 0:no)
    7 - Esta muerto (1:si, 0:no), 8 - selecionabilidad guardada, 10 - Habilidad escogida
    El primer elemento es el tipo de estadística y el segundo el numero de actor

    */
    private float actionTime = 0; // Tiempo para ejecutar las acciones de los personajes
    private int abilityNumber; // La habilidad seleccionada para la ejecucion y el determinio de como funciona
    private float abilSelection = 0; // 1 - Seleccion Aliado, 2 - Seleccion Enemigo, 3 - Todos los Aliados 4 - Todos los enemigos, 5 - El individuo ejecutandolo
    private int battleState; // El estado de la batalla: 0 - nada, 1 - Turno del jugador, 2 - Turno del enemigo
    private Animator[] actorAnimators = new Animator[6]; // Animadores de los actores
    int charNumbPlayerTurn = 0; // El numero para ir en orden de seleccion para los protagonistas
    private int[] actorSpeedSelector; // Actores que actuaran en batalla en orden
    //private int[] actorHealingChangerMultiplier; // El multiplicador de habilidad de danio
    //private int[] actorDamageChangerMultiplier; // El multiplicador de habilidad de curar
    //private int[] actorPositiveSpeedChangerMultiplier; // El multiplicador de cualquier cambio positivo de velocidad en habilidad
    //private int[] actorNegativeSpeedChangerMultiplier; // El multiplicador de cualquier cambio negativo de velocidad en habilidad
    public bool canBattle_gm; // PUBLICO! Permiso de el game manager para poder iniciar la batalla
    public int selAbilCharPosition_uiMan_gm = -1; // Personaje seleccionado en seleccion de habilidad
    //public ActorScOb char0Stats, char1Stats, char2Stats; // Scriptable Object que tiene las estadísticas de los personajes
    public ActorScOb[] actorStatsScOb; // Scriptable Object que tiene las estadísticas de los personajes
    private UI_Manager uiMan; // UI_Manager

    GameObject selectedActorGM; // Actor en accion
    GameObject enemyActorGM; // Enemigo en accion (cuando hay confrontacion)
    //bool abilIsEjecuted = false; // La accion que el personaje actuando esta ejecutando
    int actorStepTurn = 0; // Numero del actor que va a ejecutar en orden
    bool isEnemysTurn = false; // Si es el turno del enemigo para la ejecucion de habilidad
    bool startAction = false; // Si es momento de actuar para la ejecucion de habilidad
    Animator presentAnim;
    GameObject presentTargetGmO;
    bool targetIsIndividual = false;

    float accionDuration = 0; // Duracion de cada accion y habilidad en batalla
    int stepAction = 0; // Dicta que accion esta sucediendo en todo momento
    float animTime; // Tiempo que dura la animacion
    float timePassed = 0; // Tiempo pasado

    private bool playerStepTurn; // Paso para cada turno de cada aliado
    void Start()
    {
        // Declarar variables de arreglos
        actorsData = new float [6,13];
        abilityData = new float [6,3,16];

        chars = new GameObject [3];
        charsPos = new Transform [3];
        enems = new GameObject [3];
        enemsPos = new Transform [3];
        //charsStadistics = new int [11,3];
        //enemsStadistics = new int [11,3];
        actorSpeedSelector = new int[6] {0,0,0,0,0,0}; // Seleccionador de quien toca primero

        //actorHealingChangerMultiplier = new int[6] {0,0,0,0,0,0}; // El multiplicador de habilidad de danio
        //actorDamageChangerMultiplier = new int[6] {0,0,0,0,0,0}; // El multiplicador de habilidad de curacion
        //actorPositiveSpeedChangerMultiplier = new int[6] {0,0,0,0,0,0}; // El multiplicador de cualquier cambio positivo de velocidad en habilidad
        //actorNegativeSpeedChangerMultiplier = new int[6] {0,0,0,0,0,0}; // El multiplicador de cualquier cambio negativo de velocidad en habilidad
        /*
        charsAbilities = new int [4,3]; // Habilidades y su seleccion
        charsHealthPoints = new int [2,3]; // Puntos de vida
        //charsDamagePoints = new int [2,3]; // Puntos de danio
        charsVelocity = new int [2,3]; // Puntos de Velocidad
        charsDisponibility = new int [3]; // Si el personaje esta disponible
        charsTarget = new int [3]; // El objetivo de la habilidad seleccionada
        charsChargeStartup = new int [3]; // Turnos de carga para ejecutar una habilidad
        charsChargeRecovery = new int [3]; // Turnos de carga para recuperarse de ejecutar una habilidad
        charsSavedAbil = new int [3]; // Habilidad guardada por cada personaje (Para ejecutar la carga iniciar y de recuperacion)

        enemsAbilities = new int [4,3]; // Habilidades y su seleccion
        enemsHealthPoints = new int [2,3]; // Puntos de vida
        enemsDamagePoints = new int [2,3]; // Puntos de danio
        enemsVelocity = new int [2,3]; // Puntos de Velocidad
        enemsDisponibility = new int [3]; // Si el personaje esta disponible
        enemsTarget = new int [3]; // El objetivo de la habilidad seleccionada
        enemsChargeStartup = new int [3]; // Tiempo de carga de cada habilidad (Turnos)
        enemsChargeRecovery = new int [3]; // Tiempo de carga de cada habilidad (Turnos)
        enemsSavedAbil = new int [3]; // Habilidad guardada por cada personaje (Para ejecutar la carga iniciar y de recuperacion)
        */


        //abilCharacteristics = new float [12]; // Caracteristicas de la habilidad seleccionada (Se define en AbilDefiner())
        

        // Encontrar codigos
        uiMan = FindObjectOfType<UI_Manager>();
        // Encontrar Objetos padre
        fatherObj_Actors = GameObject.Find("Actors").gameObject;
        fatherObj_Pos = GameObject.Find("Positions").gameObject;
        // Encontrar Posiciones de batalla
        charsConfrontPos = fatherObj_Pos.transform.Find("AttackPosChars");
        enemsConfrontPos = fatherObj_Pos.transform.Find("AttackPosEnems");
        // Declarar Hijos de los padres (Solo en Start)
        GameObject fatherObj_CharsPos = fatherObj_Pos.transform.Find("CharsPositions").gameObject;
        GameObject fatherObj_EnemsPos = fatherObj_Pos.transform.Find("EnemsPositions").gameObject;
        GameObject fatherObj_Chars = fatherObj_Actors.transform.Find("Chars").gameObject;
        GameObject fatherObj_Enems = fatherObj_Actors.transform.Find("Enems").gameObject;
        // Encontrar Objetos de los hijos de los padres para los arreglos (Posiciones y Actores)
        for (int i = 0; i < 3; i++){
            chars[i] = fatherObj_Chars.gameObject.transform.Find("Char" + i).gameObject;
            charsPos[i] = fatherObj_CharsPos.gameObject.transform.Find("Char" + i + "Pos").gameObject.transform;
            enems[i] = fatherObj_Enems.gameObject.transform.Find("Enem" + i).gameObject;
            enemsPos[i] = fatherObj_EnemsPos.gameObject.transform.Find("Enem" + i + "Pos").gameObject.transform;
            //print("char: " + chars[i] + " enem: " + enems[i] + " charPos: " + charsPos[i] + " enemPos: " + enemsPos[i]);
        }
        
        for (int i = 0; i < 6; i++)
        {
            // Igualar Valores de los Scriptable Objects
            if (i < 3){
                /*
                charsAbilities[0,i] = actorStatsScOb[i].ability1;
                charsAbilities[1,i] = actorStatsScOb[i].ability2;
                charsAbilities[2,i] = actorStatsScOb[i].ability3;
                charsHealthPoints[0,i] = actorStatsScOb[i].health;
                charsDamagePoints[0,i] = actorStatsScOb[i].damage;
                charsVelocity[0,i] = actorStatsScOb[i].speed;
                */
            }
            else if (i >= 3){
                /*
                enemsAbilities[0,(i - 3)] = actorStatsScOb[i].ability1;
                enemsAbilities[1,(i - 3)] = actorStatsScOb[i].ability2;
                enemsAbilities[2,(i - 3)] = actorStatsScOb[i].ability3;
                enemsHealthPoints[0,(i - 3)] = actorStatsScOb[i].health;
                enemsDamagePoints[0,(i - 3)] = actorStatsScOb[i].damage;
                enemsVelocity[0,(i - 3)] = actorStatsScOb[i].speed;
                */
            }
            actorsData[i,2] = actorStatsScOb[i].maxHealth;
            actorsData[i,1] = actorsData[i,2];
            actorsData[i,4] = actorStatsScOb[i].startingSpeed;
            actorsData[i,3] = actorsData[i,4];
            for (int j = 0; j < 16; j++)
            {
                abilityData[i,1,j] = actorStatsScOb[i].ability1[j];
                abilityData[i,2,j] = actorStatsScOb[i].ability2[j];
                abilityData[i,3,j] = actorStatsScOb[i].ability3[j];
            }
        }

        // Declarar la vida del primer protagonista a 40
        //charsHealthPoints[1,0] = 40; // SOLO DE PRUEBA!!!
        actorsData[1,1] = 40;

        // Determinar Barras de vida
        for (int i = 0; i < 6; i++)
        {
            /*
            if (i < 3){
                //uiMan.hpActor_bm[i].value = charsStadistics[3,i];
                uiMan.hpActor_bm[i].value = charsHealthPoints[1,i];
            }
            if (i >= 3){
                uiMan.hpActor_bm[i].value = enemsHealthPoints[1,(i - 3)];
            }
            */
            uiMan.hpActor_bm[i].value = actorsData[i,1];
        }

        // Variables iniciales 
        selAbilCharPosition_uiMan_gm = -1;
        battleState = 1;
        playerStepTurn = false;
    }
    bool tempConfront = false;
    bool tempDamage = false;
    float movementSpeed = 7;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            tempConfront = true;
        }
        if (Input.GetKeyDown(KeyCode.B)){
            tempConfront = false;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            tempDamage = true;
        }
        if (tempConfront){
            chars[0].transform.position = Vector3.MoveTowards(chars[0].transform.position,
            charsConfrontPos.transform.position,Time.deltaTime * movementSpeed);
            enems[0].transform.position = Vector3.MoveTowards(enems[0].transform.position,
            enemsConfrontPos.transform.position,Time.deltaTime * movementSpeed);
        }
        if (!tempConfront){
            chars[0].transform.position = Vector3.MoveTowards(chars[0].transform.position,
            charsPos[0].transform.position,Time.deltaTime * movementSpeed);
            enems[0].transform.position = Vector3.MoveTowards(enems[0].transform.position,
            enemsPos[0].transform.position,Time.deltaTime * movementSpeed);
        }
        if (tempDamage){
            //enemsHealthPoints[1,0] -= 35;
            actorsData[3,1] -= 35;
            // Reiniciar las barras de vida
            for (int i = 0; i < 6; i++)
            {
                uiMan.hpActor_bm[i].value = actorsData[i,1];
                uiMan.hpActor_bm[i].maxValue = actorsData[i,2];
            }
            tempDamage = false;
        }
        if (battleState == 1){
            print("Es el turno del Jugador");
            PlayerTurn(); // void del turno del Jugador
        }
        else if (battleState == 2){
            print("Hora de detectar las velocidades");
            SpeedDetector(); // void de la deteccion de velocidades
        } 
        else if (battleState == 3) {
            ActionDefiner(); // void del identificar la accion
            //print("Hora de la batalla");
        } else if (battleState == 4) {
            AbilityExecution(); // void de la accion en batalla
        }
    }

    void PlayerTurn(){
        /*
        - Hacer Switch de El turno del jugador y dentro de este otro Switch para las
        opciones de cada personaje.
        - Aniadir la opcion de atacar.
        - En un ataque que ambos personajes se vayan al centro de la pantalla
        - Aniadir animador de efectos en los chars
        */
        // Para los 3 protagonistas

        selAbilCharPosition_uiMan_gm = charNumbPlayerTurn;
        //print ("Turno del personaje: " + charNumbPlayerTurn);
        if (uiMan.bSel_bm == 0){
            // Boton 1
            // Guardar Eleccion tomada (Estadisticas)
            //charsAbilities[3,charNumbPlayerTurn] = 0;
            actorsData[charNumbPlayerTurn,7] = 0;
            // Checar si hay seleccion de actores
            checkAbilSelection();
        }
        if (uiMan.bSel_bm == 1){
            // Boton 2
            // Guardar Eleccion tomada (Estadisticas)
            //charsAbilities[3,charNumbPlayerTurn] = 1;
            actorsData[charNumbPlayerTurn,7] = 1;
            // Checar si hay seleccion de actores
            checkAbilSelection();
        }
        if (uiMan.bSel_bm == 2){
            // Boton 3
            // Guardar Eleccion tomada (Estadisticas)
            //charsAbilities[3,charNumbPlayerTurn] = 2;
            actorsData[charNumbPlayerTurn,7] = 2;
            // Checar si hay seleccion de actores
            checkAbilSelection();
        }
        // Se acaba la seleccion
        // El numero que se iguala es la cantidad de personajes seleccionables que tienes (Seria bueno codificar esto para detectar cuantos hay)
        if (charNumbPlayerTurn == 3){
            // Reiniciar el turno del jugador para el siguiente turno
            //charNumbPlayerTurn = 0;
            selAbilCharPosition_uiMan_gm = -1;
            // Seleccionar habilidades aleatorias para los enemigos
            // CHECAR QUE SEAN ENEMIGOS !
            for (int i = 0; i < 3; i++)
            {
                actorsData[i + 3,7] = Random.Range(0,3);
                print(actorsData[i + 3,7]);
            }
            // Pasar al battleAccion
            battleState = 2;
        }
        if (playerStepTurn == true){
            // Reiniciar Input Abilidad
            uiMan.bSel_bm = -1;
            // Reiniciar Input Seleccion Personaje
            uiMan.bActor_bm = -1;
            // Reiniciar La seleccion de Actores
            uiMan.actorSel_State_bm = 0;
            // Pasar al siguiente personaje
            charNumbPlayerTurn++;
            // Reiniciar el step para que suceda solo una vez
            playerStepTurn = false;
        }
    }

    void checkAbilSelection(){
        /*
        int abilitySelected = 0;
        //abilitySelected = charsAbilities[3,charNumbPlayerTurn];
        // La habilidad seleccionada
        abilitySelected = actorsData[charNumbPlayerTurn,7];
        // Se toma el numero que determina la habilidad y se encuentra el tipo de seleccion de este
        //charsStadistics[charsStadistics[10,charNumbPlayerTurn],charNumbPlayerTurn]
        //abilityData[charNumbPlayerTurn,actorsData[charNumbPlayerTurn,7],11]
        //abilityData[charNumbPlayerTurn,abilitySelected,11]
        /*
        switch (abilityData[charNumbPlayerTurn,abilitySelected,11])
        {
            // 1 - Seleccion Aliado, 2 - Seleccion Enemigo, 3 - Todos los Aliados 4 - Todos los enemigos, 5 - El individuo ejecutandolo
            case 1: abilSelection = 1; break;
            case 2: abilSelection = 2; break;
            case 3: abilSelection = 3; break;
            case 4: abilSelection = 4; break;
            //case 4: abilSelection = 3; break;
            default:
            break;
        }
        */

        // Igualar la seleccion de habilidades al objetivo del personaje
        abilSelection = abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],11];

        if (abilSelection == 1){
            // Activar Seleccion de aliados
            uiMan.actorSel_State_bm = 1;
            print ("Estoy decidiendo que aliado voy a elegir");
            // Desactivar seleccion de habilidades
            selAbilCharPosition_uiMan_gm = -1;
            // Opciones de botones
            if (uiMan.bActor_bm == 0){
                //charsTarget[charNumbPlayerTurn] = 0;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 0f;
                playerStepTurn = true;
            }
            if (uiMan.bActor_bm == 1){
                //charsTarget[charNumbPlayerTurn] = 1;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 1f;
                playerStepTurn = true;
            }
            if (uiMan.bActor_bm == 2){
                //charsTarget[charNumbPlayerTurn] = 2;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 2f;
                playerStepTurn = true;
            }
            // Igualar la variable al numero que se eliguió charsStadistics[8,charNumbPlayerTurn] del 0 al 2
        }
        if (abilSelection == 2){
            // Activar Seleccion de enemigos
            uiMan.actorSel_State_bm = 2;
            print ("Estoy decidiendo que enemigo voy a elegir");
            // Desactivar seleccion de habilidades
            selAbilCharPosition_uiMan_gm = -1;
            // Opciones de botones
            if (uiMan.bActor_bm == 0){
                //charsTarget[charNumbPlayerTurn] = 3;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 3f;
                playerStepTurn = true;
            }
            if (uiMan.bActor_bm == 1){
                //charsTarget[charNumbPlayerTurn] = 4;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 4f;
                playerStepTurn = true;
            }
            if (uiMan.bActor_bm == 2){
                //charsTarget[charNumbPlayerTurn] = 5;
                abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 5f;
                playerStepTurn = true;
            }
            // Igualar la variable al numero que se eliguió charsStadistics[8,charNumbPlayerTurn] del 3 al 5
        }
        if (abilSelection == 3){
            //charsStadistics[8,charNumbPlayerTurn] = 6; // Todos los aliados
            //charsTarget[charNumbPlayerTurn] = 6; // Todos los aliados
            abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 6f;
            playerStepTurn = true;
        }
        if (abilSelection == 4){
            //charsStadistics[8,charNumbPlayerTurn] = 7; // Todos los enemigos
            //charsTarget[charNumbPlayerTurn] = 7;
            abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 7f;
            playerStepTurn = true;
        }
        if (abilSelection == 5){
            //charsStadistics[8,charNumbPlayerTurn] = 8; // Solo el individuo
            //charsTarget[charNumbPlayerTurn] = 8;
            abilityData[charNumbPlayerTurn,(int)actorsData[charNumbPlayerTurn,7],12] = 8f;
            playerStepTurn = true;
        }
    }

    void SpeedDetector(){   

        // ESTOS NUMEROS SON DE PRUEBA
        /*
        charsVelocity[1,0] = 4;// Actor 1
        charsVelocity[1,1] = 7;// Actor 2
        charsVelocity[1,2] = 3;// Actor 3
        enemsVelocity[1,0] = 1;// Actor 4
        enemsVelocity[1,1] = 2;// Actor 5
        enemsVelocity[1,2] = 9;// Actor 6
        */
        actorsData[1,3] = 4;
        actorsData[2,3] = 7;
        actorsData[3,3] = 3;
        actorsData[4,3] = 1;
        actorsData[5,3] = 2;
        actorsData[6,3] = 9;

        int[] speedInfoKeeper;
        speedInfoKeeper = new int[6] {0,0,0,0,0,0};
        int actorOrderCounter = 0;
        // Checar la velocidad de cada actor del juego (Con maximo 30)
        for (int i = 30; i > 0; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                if (actorsData[i,3] >= i){
                    actorSpeedSelector[actorOrderCounter] = j;
                    speedInfoKeeper[j] = (int)actorsData[i,3];
                    actorsData[i,3] = -100; // Para evitar rechecarlo
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
            actorsData[i,3] = speedInfoKeeper[i];
            /*
            if (i < 3){
                charsVelocity[1,i] = speedInfoKeeper[i];
            }
            if (i >= 3){
                enemsVelocity[1,(i-3)] = speedInfoKeeper[i];
            }
            */
        }
        battleState = 3;

        // Encontrar animadores de los actores
        for (int i = 0; i < 6; i++)
        {
            if (i < 3){
                actorAnimators[i] = chars[i].transform.GetChild(0).
                gameObject.GetComponent<Animator>();
                //print ("Se encontro animador de actor " + i);
            }
            if (i >= 3){
                actorAnimators[i] = enems[i - 3].transform.GetChild(0).
                gameObject.GetComponent<Animator>();
                //print ("Se encontro animador de actor " + i);
            }
        }
    }

    void ActionDefiner(){

        /*
        print ("Estoy en ActionDefiner");

        // Reiniciar las barras de vida
        for (int i = 0; i < 6; i++)
        {
            if (i < 3){
                uiMan.hpActor_bm[i].value = charsHealthPoints[1,i];
                uiMan.hpActor_bm[i].maxValue = charsHealthPoints[0,i];
            }
            if (i >= 3){
                uiMan.hpActor_bm[i].value = enemsHealthPoints[1,(i - 3)];
                uiMan.hpActor_bm[i].maxValue = enemsHealthPoints[0,(i - 3)];
            }
        }

        

        // Checar la disponibiliad de los personajes
        // Encontrar si actua el enemigo y si esta disponible
        if (actorSpeedSelector[actorStepTurn] < 3){
            if (charsDisponibility[actorSpeedSelector[actorStepTurn]] == 0){
                // Iniciar accion del personaje
                isEnemysTurn = false;
                startAction = true;
                print ("El Aliado esta disponible");
            } else if (charsDisponibility[actorSpeedSelector[actorStepTurn]] == 2){
                //
                startAction = false;
                print ("El Aliado esta ejecutando una habilidad");
                //charsChargeDuration[actorSpeedSelector[actorStepTurn]] -= 1;
            } else {
                startAction = false;
                print ("El Aliado no esta disponible");
            }
        } else if (actorSpeedSelector[actorStepTurn] >= 3){
            if (enemsDisponibility[actorSpeedSelector[actorStepTurn] - 3] == 0){
                // Iniciar accion del enemigo
                isEnemysTurn = true;
                startAction = true;
                print ("El enemigo esta disponible");
            } else if (enemsDisponibility[actorSpeedSelector[actorStepTurn] - 3] == 2){
                startAction = false;
                print ("El enemigo esta ejecutando una habilidad");
                //enemsChargeDuration[actorSpeedSelector[actorStepTurn]] -= 1;
            } else {
                startAction = false;
                print ("El enemigo no esta disponible");
            }
        }

        // Accion presente incial: Encontrar la habilidad que se tiene que ejecutar y de que personaje
        if (startAction == true){
            // CHECAR SI EL PERSONAJE ESTA VIVO
            print ("Ejecutando primera accion");
            // Identificar que personaje es el que toca
            if (!isEnemysTurn){
                // Obtener el actor que esta actuando
                for (int i = 0; i < 3; i++)
                {
                    // Igualar el GameObject al actor seleccionado en chars
                    if (actorSpeedSelector[actorStepTurn] == i){
                    selectedActorGM = chars[i];
                    }
                }

                // Obtener el animator
                presentAnim = chars[actorSpeedSelector[actorStepTurn]].transform.GetChild(0).gameObject.GetComponent<Animator>();
                presentAnim.SetInteger("AbilNum", charsAbilities[3, actorSpeedSelector[actorStepTurn]]);
                
                // Checar si el contrincante es un individuo
                // Si ninguna instancia aplica entonces Target no es individual
                targetIsIndividual = false;
                // Checar cada instancia del target
                for (int i = 0; i < 6; i++)
                {
                    if (charsTarget[actorSpeedSelector[actorStepTurn]] == i && i < 3){
                        presentTargetGmO = chars[i];
                        targetIsIndividual = true;
                    }
                    if (charsTarget[actorSpeedSelector[actorStepTurn]] == i && i >= 3){
                        presentTargetGmO = enems[i];
                        targetIsIndividual = true;
                    }
                }

                
                // Obtener el numero de la habilidad que se selecciono
                switch (charsAbilities[3, actorSpeedSelector[actorStepTurn]])
                {
                    case 0: abilityNumber = charsAbilities[0,actorSpeedSelector[actorStepTurn]]; print (abilityNumber); break;
                    case 1: abilityNumber = charsAbilities[1,actorSpeedSelector[actorStepTurn]]; print (abilityNumber); break;
                    case 2: abilityNumber = charsAbilities[2,actorSpeedSelector[actorStepTurn]]; print (abilityNumber); break;
                    default: break;
                }
                // Encontrar el animador del hijo del actor y animarlo como corresponde
                //chars[actorSpeedSelector[selectedCharacter]].transform.GetChild(0).gameObject.GetComponent<Animator>()
                //.SetInteger("State", actorSpeedSelector[selectedCharacter]);
            }
            if (isEnemysTurn) {
                
                // Obtener el actor que esta actuando
                for (int i = 0; i < 3; i++)
                {
                    // Igualar el GameObject al actor seleccionado en chars
                    if (actorSpeedSelector[actorStepTurn] - 3 == i){
                    selectedActorGM = enems[i];
                    }
                }

                // Checar si el contrincante es un individuo
                // Si ninguna instancia aplica entonces Target no es individual
                targetIsIndividual = false;
                // Checar cada instancia del target
                for (int i = 0; i < 6; i++)
                {
                    if (enemsTarget[actorSpeedSelector[actorStepTurn] - 3] == i && i < 3){
                        presentTargetGmO = chars[i];
                        targetIsIndividual = true;
                    }
                    if (enemsTarget[actorSpeedSelector[actorStepTurn] - 3] == i && i >= 3){
                        presentTargetGmO = enems[i];
                        targetIsIndividual = true;
                    }
                }
                // Obtener animator
                presentAnim = enems[actorSpeedSelector[actorStepTurn] - 3].transform.GetChild(0).gameObject.GetComponent<Animator>();
                presentAnim.SetInteger("AbilNum", enemsAbilities[3, actorSpeedSelector[actorStepTurn] - 3]);

                // Obtener el numero de la habilidad que se selecciono
                switch (enemsAbilities[3, (actorSpeedSelector[actorStepTurn] - 3)])
                {
                    case 0: abilityNumber = enemsAbilities[0,actorSpeedSelector[actorStepTurn] - 3]; print (abilityNumber); break;
                    case 1: abilityNumber = enemsAbilities[1,actorSpeedSelector[actorStepTurn] - 3]; print (abilityNumber); break;
                    case 2: abilityNumber = enemsAbilities[2,actorSpeedSelector[actorStepTurn] - 3]; print (abilityNumber); break;
                    default: break;
                }
                // Encontrar el animador del hijo del actor y animarlo como corresponde
                //chars[actorSpeedSelector[selectedCharacter]].transform.GetChild(0).gameObject.GetComponent<Animator>()
                //.SetInteger("State", actorSpeedSelector[selectedCharacter]);

            }

            // Buscar la habilidad que se va a ejecutar
            AbilitysData();

                    // Si hay confrontacion
            if (abilCharacteristics[3] == 1){
                //stepAction = 4;
            } else {
                //stepAction = 1;
            }

            // Igualar el tiempo de la habilidad a animTime
            animTime = abilCharacteristics[5];

            // Ejecutar la habilidad
            AbilityExecution();
            
            // ESTA PARTE HACERLA PARA DESPUES CUANDO SE TRABAJE EN LAS HABILIDADES DE VARIOS TURNOS
            // Si la habiliad dura varios turnos:
            /*
            if (abilCharacteristics[4] == 0){
                // Ejecutar la habilidad
                AbilityExecution();
            } else {
                // 

            }
            */

            // Iniciar la habilidad
            //battleState = 4;
            //startAction = true;
        //}
        /*
        - Numero del actor
        actorSpeedSelector[selectedCharacter]
        */
    }
    void AbilityExecution(){
        /*
        print("Estoy ejecutando la habilidad");
        switch (stepAction)
        {
            // Del caso 1 al 3 es una ejecucion sin confrontacion pero del 4 al 8 es una ejecucion con confrontacion
            case 1: 
            // Animar al personaje ejecutando la habilidad
            presentAnim.SetBool("isActing", true);
            // Esperar tiempo para la animacion
            //timePassed = animTime *= -Time.deltaTime;
            animTime -= Time.deltaTime;
            print (animTime);
            print ("Habilidad sin confrontacion");
            if (animTime <= 0){
                //stepAction = 2;
            }
            break;
            case 2:
            // Aplicar el efecto al grupo o individuo
            print ("Hora de aplicar el efecto");
            

            // Checar si el actor acuando es enemigo o aliado
            
            // Checar que tipo de efecto es
            
            // Checar que tipo de ecuacion es

            // Checar quien es el que recibe el efecto
            //enemsTarget[actorSpeedSelector[actorStepTurn] - 3]
            //charsTarget[actorSpeedSelector[actorStepTurn]]

            // Checar el multiplicador de efecto (si el modificador de numeros [como vida o velocidad] es afectado por otra habilidad, se vera aplicado aqui)

            // Checar posible efecto secundario o terciario
            // Aplicar el efecto secundario o terciario al grupo o individuo
            break;
            case 3: // Animar idle de nuevo
            // Checar la disponibilidad de ambos personajes afectados
            // Animarlos como corresponde
            // END - Continuar al siguiente personaje
            battleState = 3;
            actorStepTurn += 1;
            break;
            case 4: 
            // Mover a los personajes a las posiciones centrarles

            // Animar su caminar
            // Cuando lleguen al punto central (aproximado) entonces ir al siguiente paso
            print ("Habilidad con confrontacion");
            break;
            case 5: // Detenerlos y Animarlos en idle
            // Animar al que ejecutara el ataque
            // Esperar el tiempo para la animacion
            break;
            case 6: // Infringir efecto
            // Checar posible efecto secundario o terciario
            break;
            case 7: // checar la mortalidad de los personajes
            // A los personajes muertos - Animar al personaje muerto en confrontacion
            // - Teletransportarlo a su posicion original
            // - Voltear visual del personaje vivo (si hay alguno)
            break;
            case 8: // Mover a los personajes vivos a sus posiciones originales
            // Animar a los personajes vivos
            // Checar si ambos estan en sus posiciones originales
            // Si es verdad animar el idle del personaje vivo
            // END - Continuar al siguiente personaje
            battleState = 3;
            actorStepTurn += 1;
            break;
            default: print ("numero de step action equivocado en Ability Execution");
            break;
        }
        
        /*
        - Confrontar si hay confrontacion
        Si hay confrontacion tomar tiempo de la confrontacion ademas de la animacion
        Empezar a animar
        Tomar tiempo de la animacion

        Checar si algun personaje esta vivo
        Reiniciar las barras de vida

        Sumar selectedCharacter para ir al siguiente personaje
        Reiniciar Present accion para el siguiente personaje

        chars[0].transform.position = Vector3.MoveTowards(chars[0].transform.position,charsConfrontPos.transform.position,Time.deltaTime * movementSpeed);
        enems[0].transform.position = Vector3.MoveTowards(enems[0].transform.position,enemsConfrontPos.transform.position,Time.deltaTime * movementSpeed);
        */
    }

    void AbilitysData(){
        
        switch (abilityNumber)
        {
            /* elementos
            0 - Tipo de habilidad == 1 - Afecta vida, 2 - Afecta Velocidad, 3 - Cambio de Aplicacion de vida, 4 - Cambio de Aplicacion de velocidad
            1 - Cantidad de efecto == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
            2 - Turnos necesitados == 0 - Es inmediato, 1 - Un turno extra en hacerse, 2 - Dura dos turnos en hacerse, 3 - dura tres turnos, 4...
            3 - Hay confrontacion == 0 - No, 1 - Si 
            4 - Duracion del efecto == 0 - No hay efecto(es inmediato), 1 - Dura un turno, 2 - Dura dos turnos... etc.
            5 - Tiempo del efecto == 20 - Dura veinte segundos la animacion de la habilidad
            6 - Metodo de ecuacion de la habilidad == 0 - Suma o Resta, 1 - Multiplicacion o Division
            7 - Efecto Secundario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
            8 - Objetivo del efecto Secundario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
            9 - Cantidad de efecto secundario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
            10 - Efecto Terciario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
            11 - Cantidad de efecto Terciario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
            12 - Objetivo del efecto Terciario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
            */
            /*
            case 1:
            // Escoba: Ataque mediocre o malo directamente
            abilCharacteristics[0] = 1; // Ataque
            abilCharacteristics[1] = 1.2f; // Danio aflingido
            abilCharacteristics[2] = 0; // Es inmediato
            abilCharacteristics[3] = 1; // Hay confrontacion
            abilCharacteristics[4] = 0; // No hay duracion de efecto
            abilCharacteristics[5] = 6.25f; // La habiliad dura 6.25 segundos
            abilCharacteristics[6] = 0; // Suma o resta
            break;
            case 2:
            // EEEEEE: Reduce drasticamente el daño de un enemigo
            abilCharacteristics[0] = 4; // Reduccion de danio
            abilCharacteristics[1] = 0.75f; // Lo que se reduce
            abilCharacteristics[2] = 0; // Es inmediato
            abilCharacteristics[3] = 0; // No hay confrontacion
            abilCharacteristics[4] = 0; // No hay duracion de efecto
            abilCharacteristics[5] = 3.25f; // La habiliad dura 3.25 segundos
            abilCharacteristics[6] = 1; // Suma o resta
            break;
            case 3:
            // Gigacado Chadvocado: Aumenta el ataque y velocidad de todo el equipo aliado
            abilCharacteristics[0] = 4; // Aumento de danio
            abilCharacteristics[1] = 2; // Lo que se aumenta
            abilCharacteristics[2] = 0; // Es inmediato
            abilCharacteristics[3] = 0; // No hay confrontacion
            abilCharacteristics[4] = 2; // Dura 2 turnos
            abilCharacteristics[5] = 6.25f; // La habiliad dura 6.25 segundos
            abilCharacteristics[7] = 3; // Aumento de velocidad Efecto Secundaro (EfSe)
            abilCharacteristics[8] = 1.3f; // Lo que se aumenta
            break;
            */
            default:
            break;
        }
    }
}
