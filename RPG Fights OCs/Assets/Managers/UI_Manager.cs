using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    public ScBtl_Motor battleMotor;
    [SerializeField] RectTransform sel_Abilities; // Transform of Sel_Abil
    //private Battle_Manager bm; // Battle Manager
    //public int bActor_bm; // Public - Determina la seleccion de habilidades
    public int bActorSel_bm; // Public - Determina la seleccion de enemigo cuando se requiere
    public Slider[] hpActor_bm; // Public - Slider de la vida de los actores
    public Button[] b_SelActor; // Public - Boton para seleccionar un actor
    public RectTransform[] pos_SelActor; // Public - posicion para elegir a un actor
    public int actorSel_State_bm; // Public - Decide si se puede seleccionar a los aliados o enemigos o ninguno
    private bool canAim;

    // Variables publicas

    public RectTransform[] pos_SelAbilities; // Posiciones donde SelAbilities se moveria
    public int butSelAbil = 0; // Public - Determina la seleccion de habilidades
    public int selectedActor = 0; // Actor siendo seleccionado
    public int possibleAimedActor; // Posible personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public int aimedActor; // Personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public bool charCanAct; // Determina si el personaje seleccionado puede actuar o no

    void Start()
    {
        // Encontrar battle motor
        battleMotor = FindObjectOfType<ScBtl_Motor>();

        //sel_Abilities.position = new Vector3(1f, -0.5f, 0f); // Pos for char2
        //sel_Abilities.position = new Vector3(1.5f, 3f, 0f); // Pos for char0
        //sel_Abilities.position = new Vector3(3.7f, 1.3f, 0f); // Pos for char1
        sel_Abilities.position = new Vector3(15f, 0f, 0f); // Pos for Empty
        //bm = FindObjectOfType<Battle_Manager>();
        possibleAimedActor = -1;
        //bActor_bm = -1;
        butSelAbil = -1;
        aimedActor = -1;
        //canAim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){ selectedActor = 0; }
        if (Input.GetKeyDown(KeyCode.T)){ selectedActor = 1; }
        if (Input.GetKeyDown(KeyCode.Y)){ selectedActor = 2; }
        if (Input.GetKeyDown(KeyCode.U)){ selectedActor = -1; }


        //battleMotor.MoveCamaraOnTarget();
        // Datos para Battle Motor
        /*
        battleMotor.m_ui_butSelAbil = butSelAbil;
        selectedActor = battleMotor.m_bm_actorsSel;
        battleMotor.m_ui_aimedActor = aimedActor;
        possibleAimedActor = battleMotor.m_bm_possibleAimedActor;
        */
        /*
        if (battleMotor.m_bm_RestartValues1 == true)
        {
            print("Estoy en Restart Values de UI Manager");
            butSelAbil = -1;
            aimedActor = -1;
        }
        */
        // Mover el seleccionador de habilidades al personaje o al vac�o

        /*
        if (bm.selAbilCharPosition_uiMan_gm == 0){
            sel_Abilities.position = pos_SelAbilities[0].position;
            //sel_Abilities.position = new Vector3(1.5f, 3f, 0f); // Pos for char0
        }
        else if (bm.selAbilCharPosition_uiMan_gm == 1){
            sel_Abilities.position = pos_SelAbilities[1].position;
            //sel_Abilities.position = new Vector3(3.7f, 1.3f, 0f); // Pos for char1
        } 
        else if (bm.selAbilCharPosition_uiMan_gm == 2){
            sel_Abilities.position = pos_SelAbilities[2].position;
            //sel_Abilities.position = new Vector3(1f, -0.5f, 0f); // Pos for char2
        }
        else {
            sel_Abilities.position = new Vector3(15f, 0f, 0f); // Pos for Empty
        }
        */


        /*
        - Volver a checar la habilidad de hacer algo cuando el mouse esta encima de un boton
        - Botones para las habilidades
            - Obtener que habilidades son cada boton dependiendo de charStadistics de Battle_Manager
            - Cambiar el texto y el color dependiendo de la habilidad
        
        - Botones de Seleccion de personajes
            - Si no hay seleccion de personajes que las posiciones de los botones sea fuera del escenario
            - Encontrar y acomodar las posiciones de los botones cuando son seleccionables
            - Separarlos por Chars y Enems cuando Battle_Manager lo necesite
        */
    }

    public void UpdateUI_Elements() {
        // Si el actor seleccionado esta disponible
        if (selectedActor >= 0 && charCanAct == true)
        {
            // Mover la posici�n de la seleccion de habilidades al numero del actor siendo seleccionado
            sel_Abilities.position = pos_SelAbilities[selectedActor].position;
        }
        else
        {
            // Mover la posicion a un lugar "inexistente"
            sel_Abilities.position = new Vector3(15f, 0f, 0f);
        }

        // Si posible punteria es 0 entonces apunto a los aliados y no esta actuando
        if (possibleAimedActor == 0 && charCanAct == false)
        {
            // UI: Apunto a los aliados
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = pos_SelActor[i].position;
            }
        }
        // Si posible punteria es 1 entonces apunto a los enemigos y no esta actuando
        else if (possibleAimedActor == 1 && charCanAct == false)
        {
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = pos_SelActor[i + 3].position;
            }
        }
        // Si ninguna de las anteriores condiciones son verdaderas entonces no apunto a ningun lado
        else
        {
            // UI: Apunto a ningun lado
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = new Vector3(15f, 0f, 0f);
            }
        }
    }

    public void ButtonSelection1(){
        // Se presiono el boton 1
        butSelAbil = 0;
        battleMotor.AbilInput(butSelAbil);
    }
    public void ButtonSelection2(){
        // Se presiono el boton 2
        butSelAbil = 1;
        battleMotor.AbilInput(butSelAbil);
    }
    public void ButtonSelection3(){
        // Se presiono el boton 3
        butSelAbil = 2;
        battleMotor.AbilInput(butSelAbil);
    }

    public void ButtonActor1(){
        // Se presiono el boton 1
        aimedActor = 0;
        battleMotor.AimInput(aimedActor);
    }
    public void ButtonActor2(){
        // Se presiono el boton 2
        aimedActor = 1;
        battleMotor.AimInput(aimedActor);
    }
    public void ButtonActor3(){
        // Se presiono el boton 3
        aimedActor = 2;
        battleMotor.AimInput(aimedActor);
    }
}
