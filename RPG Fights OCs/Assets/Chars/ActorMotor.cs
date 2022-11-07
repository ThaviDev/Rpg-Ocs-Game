using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMotor : MonoBehaviour
{
    public ActorScOb actorScOb; // Scriptable Object con los datos est�ticos
    public int[] actorsData; // Datos del Actor
    public int[,] abilitiesData; // Datos de las habilidades
    // Lista para los estados
    List<int> States = new List<int>();

    // Llamar a todas las acciones y a Battle Manager que el personaje ya est� actuando
    public bool actorIsActing;

    public GameObject action;

    public ActionMotor myAction; // Accion siendo seleccionada y preparada para ser enviada 
    public int actionDta;
    void Start()
    {
        actionDta = 5;

        // Declarar datos de actores
        actorsData = new int[14];
        // Declarar datos de habilidades
        abilitiesData = new int[3, 4];
        InsertValuesScOb();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        actionDatatest = actorScOb.abilities[0].actionObjective[0];
        print(actionDatatest);
        */
    }
    bool canStartAbility = false;
    // Esta funcion es llamada por Battle Manager, y activa todo lo necesario para que funcione una habilidad
    public void EjecuteAbility()
    {
        if (canStartAbility == false)
        {
            canStartAbility = true;
            // Variable de chequeo de Battle MAnager
            actorIsActing = true;
            // Encontrar Hijos actions, y activar su acci�n
            for (int i = 1; i < transform.childCount; i++)
            {
                print("Hora de decirle a la accion que puede actuar");
                this.gameObject.transform.GetChild(i).GetComponent<ActionMotor>().actorIsActing = true;
            }
            StartCoroutine("animEjecution");
        }
        // Animar al personaje ejecutando la habilidad, (Tambien hay que hacer la logica de la confrontacion)
        // Esperar el tiempo necesario para spawnear las acciones
        // Spawnear las acciones, dandoles sus atributos y todo lo dem�s
        // Esperar el tiempo para acabar la habilidad
        // Acabar la habilidad (Decirle a Battle Manager cuando se inicia la siguiente actuaci�n, con una variable)
        //print(_time);
        //actorIsActing = false;
    }

    IEnumerator animEjecution()
    {
        yield return new WaitForSeconds((actorScOb.abilities[actorsData[1]].abilityData[4] / 100));
        print("PAAAM");
        // Spawnear Acciones
        for (int i = 0; i < actorScOb.abilities[actorsData[1]].actionAmount; i++)
        {
            Instantiate(action); // Instanciar la acci�n como objeto
            myAction = FindObjectOfType<ActionMotor>(); // Encontrarla y guardarla
            myAction.actionData = new int[5]; // clasificar un array con 5 elementos
            // establecer todos los datos correspondientes a la acci�n
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

        }
        yield return new WaitForSeconds((actorScOb.abilities[actorsData[1]].abilityData[5] / 100));
        // Terminar de ejecutar
        print("Ok, termine");
        actorIsActing=false;
    }

    void InsertValuesScOb()
    {
        // No tiene ninguna habilidad seleccionada
        actorsData[1] = 0;
        // Vida M�xima Presente = Vida M�xima Inicial
        actorsData[2] = actorScOb.actorsData[0];
        // Vida Presente = Vida Inicial
        actorsData[3] = actorScOb.actorsData[1];
        // Velocidad Presente = Velocidad Inicial
        actorsData[4] = actorScOb.actorsData[2];
        // Multi Da�o = Multi Inicial Da�o
        actorsData[5] = actorScOb.actorsData[3];
        // Multi Resistencia = Multi Inicial resistencia
        actorsData[6] = actorScOb.actorsData[4];
        // Multi Curacion = Multi Inicial Curacion
        actorsData[7] = actorScOb.actorsData[5];
        // Multi Aceleracion = Multi Inicial Aceleracion
        actorsData[8] = actorScOb.actorsData[6];
        // Multi Desaceleracion = Multi Inicial Desaceleracion
        actorsData[9] = actorScOb.actorsData[7];

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
