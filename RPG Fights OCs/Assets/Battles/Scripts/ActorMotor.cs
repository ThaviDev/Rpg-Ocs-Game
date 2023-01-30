using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMotor : MonoBehaviour
{
    public ActorScOb actorScOb; // Scriptable Object con los datos estáticos
    public float[] actorsData; // Datos del Actor
    public float[,] abilitiesData; // Datos de las habilidades
    // Primer dato es el numero de la habilidad / El segundo es el dato

    // El objeto hijo visual del actor
    private GameObject visualGM;
    // El animador del hijo visual
    private Animator animator;
    // Lista para los estados
    List<int> States = new List<int>();

    // Llamar a Battle Manager que el personaje ya está actuando
    public bool actorIsActing;

    // LLamar a todas las acciones de que es hora de acccionarse, se llama a través de Battle Manager
    public bool canActionActivate;

    public GameObject action;

    private ActionMotor myAction; // SCOB de la acción, todas las acciones son iguales, se declara diferencias en el codigo
    void Start()
    {
        // Declarar datos de actores
        actorsData = new float[14];
        // Declarar datos de habilidades
        abilitiesData = new float[3, 4];
        visualGM = this.gameObject.transform.GetChild(0).gameObject;
        animator = visualGM.GetComponent<Animator>();
        //myAnimator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        InsertValuesScOb();
    }
    void Update()
    {
        if (actorIsActing)
        {
            animator.SetInteger("BattleAbil", (int)actorsData[1] + 1);
        } else
        {
            animator.SetInteger("BattleAbil", 0);
        }
        /*
        actionDatatest = actorScOb.abilities[0].actionObjective[0];
        print(actionDatatest);
        */
    }

    public IEnumerator CanAction()
    {
        canActionActivate = true;
        yield return new WaitForSeconds(0.4f);
        canActionActivate = false;
    }
    /*
    bool canStartAbility = false;
    // Esta funcion es llamada por Battle Manager, y activa todo lo necesario para que funcione una habilidad
    public void EjecuteAbility()
    {
        if (canStartAbility == false)
        {
            canStartAbility = true;
            // Variable de chequeo de Battle MAnager
            actorIsActing = true;
            // Encontrar Hijos actions, y activar su acción
            for (int i = 1; i < transform.childCount; i++)
            {
                print("Hora de decirle a la accion que puede actuar");
                this.gameObject.transform.GetChild(i).GetComponent<ActionMotor>().actorIsActing = true;
            }
            StartCoroutine("animEjecution");
            StartCoroutine("animEjecution");
            StartCoroutine("animEjecution");
        }
        // Animar al personaje ejecutando la habilidad, (Tambien hay que hacer la logica de la confrontacion)
        // Esperar el tiempo necesario para spawnear las acciones
        // Spawnear las acciones, dandoles sus atributos y todo lo demás
        // Esperar el tiempo para acabar la habilidad
        // Acabar la habilidad (Decirle a Battle Manager cuando se inicia la siguiente actuación, con una variable)
        //print(_time);
        //actorIsActing = false;
    }

    IEnumerator animEjecution()
    {
        yield return new WaitForSeconds((actorScOb.abilities[actorsData[1]].abilityData[4] / 100) + 2);
        print("PAAAM");
        // Determinar en donde va a spawnear la acción


        // Spawnear Acciones
        for (int i = 0; i < actorScOb.abilities[actorsData[1]].actionAmount; i++)
        {
            // Checar si alguna de estas acciones se spawnea mas de una vez, viendo el objetivo de la acción
            switch (actorScOb.abilities[actorsData[1]].actionObjective[i])
            {
                case 0:

                    break;
                case 1:

                    break;

            }
            Instantiate(action); // Instanciar la acción como objeto
            myAction = FindObjectOfType<ActionMotor>(); // Encontrarla y guardarla
            myAction.actionData = new int[5]; // clasificar un array con 5 elementos
            // establecer todos los datos correspondientes a la acción
            myAction.actionData[0] = actorScOb.abilities[actorsData[1]].classification[i]; 
            myAction.actionData[1] = actorScOb.abilities[actorsData[1]].applic_Type[i];
            myAction.actionData[2] = actorScOb.abilities[actorsData[1]].quantity[i];
            myAction.actionData[3] = actorScOb.abilities[actorsData[1]].applic_Turn[i];
            myAction.actionData[4] = actorScOb.abilities[actorsData[1]].duration[i];
            myAction.transform.SetParent(this.gameObject.transform);
            //myAction.actionData[0] = actionDta;
            /*
            switch (myAction.actionObjective)
            {
                case 2:
                    myAction.transform.SetParent(this.gameObject.transform);
                    break;
                    /*
                case 3:
                    myAction.transform.SetParent(abilitiesData[actorsData[1], 3])
                    break;
                    
            }
            */
            //myAction.transform.SetParent(actorsMotor[i].gameObject.transform);
            /*
        }
        yield return new WaitForSeconds((actorScOb.abilities[actorsData[1]].abilityData[5] / 100) + 2);
        // Terminar de ejecutar
        print("Ok, termine");
        actorIsActing=false;
    }
    */
    void InsertValuesScOb()
    {
        visualGM.GetComponent<Animator>().runtimeAnimatorController = actorScOb.animControl;
        // No tiene ninguna habilidad seleccionada
        actorsData[1] = 0;

        /*
        // Vida Máxima Presente = Vida Máxima Inicial
        actorsData[2] = actorScOb.actorsData[0];
        // Vida Presente = Vida Inicial
        actorsData[3] = actorScOb.actorsData[1];
        // Danio Presente = Danio Inicial
        actorsData[4] = actorScOb.actorsData[2];
        // Resistencia Presente = Resistencia Inicial
        actorsData[5] = actorScOb.actorsData[3];
        // Curacion Presente = Curacion Inicial
        actorsData[6] = actorScOb.actorsData[4];
        // Velocidad Presente = Velocidad Inicial
        actorsData[7] = actorScOb.actorsData[5];
        // Multi de Danio = Multi Inicial de danio
        actorsData[8] = actorScOb.actorsData[6];
        // Multi de Resistencia = Multi Inicial resistencia
        actorsData[9] = actorScOb.actorsData[7];
        // Multi Curacion = Multi Inicial Curacion
        actorsData[10] = actorScOb.actorsData[8];
        // Multi Aceleracion = Multi Inicial Aceleracion
        actorsData[11] = actorScOb.actorsData[9];
        // Multi Desaceleracion = Multi Inicial Desaceleracion
        actorsData[12] = actorScOb.actorsData[10];
        */
        // PARA SIMPLIFICAR LO DE ARRIBA
        for (int i = 0; i < 10; i++)
        {
            actorsData[i + 2] = actorScOb.actorsData[i];
        }

        // Para todas las habilidades
        for (int i = 0; i < 3; i++)
        {
            // Duracion Cooldown = Duracion Inicial Cooldown
            abilitiesData[i, 0] = actorScOb.abilities[i].abilityData[3];
            /*
            // Cooldown Presente = 0
            abilitiesData[i, 1] = 0;
            // Turnos Presentes = 0
            abilitiesData[i, 2] = 0;
            // Objetivo Seleccionado = 0
            abilitiesData[i, 3] = 0;
            */
        }
    }
}
