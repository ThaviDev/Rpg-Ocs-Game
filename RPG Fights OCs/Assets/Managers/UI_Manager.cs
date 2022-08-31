using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    public ScBtl_Motor battleMotor;
    [SerializeField] RectTransform sel_Abilities; // Transform of Sel_Abil
    public RectTransform[] pos_SelAbilities; // Posiciones donde SelAbilities se moveria
    //private Battle_Manager bm; // Battle Manager
    public int bActor_bm; // Public - Determina la seleccion de habilidades
    public int bActorSel_bm; // Public - Determina la seleccion de enemigo cuando se requiere
    public Slider[] hpActor_bm; // Public - Slider de la vida de los actores
    public Button[] b_SelActor; // Public - Boton para seleccionar un actor
    public RectTransform[] pos_SelActor; // Public - posicion para elegir a un actor
    public int actorSel_State_bm; // Public - Decide si se puede seleccionar a los aliados o enemigos o ninguno
    private bool canAim;

    // Variables publicas
    public int butSelAbil = 0; // Public - Determina la seleccion de habilidades
    public int selectedActor = 0; // Actor siendo seleccionado
    public int possibleAimedActor; // Posible personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public int aimedActor; // Personaje seleccionado en la parte de apuntar a un personaje en la interfaz

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
        bActor_bm = -1;
        butSelAbil = -1;
        aimedActor = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Datos para Battle Motor
        battleMotor.m_ui_butSelAbil = butSelAbil;
        selectedActor = battleMotor.m_bm_actorsSel;
        battleMotor.m_ui_aimedActor = aimedActor;
        possibleAimedActor = battleMotor.m_bm_possibleAimedActor;

        if (selectedActor >= 0)
        {
            // Mover la posición de la seleccion de habilidades al numero del actor siendo seleccionado
            sel_Abilities.position = pos_SelAbilities[selectedActor].position;
            print("Seleccion de habilidades va al actor seleccionado");
        } else
        {
            // Pos for Empty
            sel_Abilities.position = new Vector3(15f, 0f, 0f);
            print("Seleccion de habilidades no existe");
        }
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
        if (canAim == true)
        {
            if (possibleAimedActor == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = pos_SelActor[i].position;
                }
            }
            else if (possibleAimedActor == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = pos_SelActor[i + 3].position;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = new Vector3(15f, 0f, 0f);
                }
            }
        }
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

    public void ButtonSelection1(){
        // Se presiono el boton 1
        butSelAbil = 0;
        canAim = true;
    }
    public void ButtonSelection2(){
        // Se presiono el boton 2
        butSelAbil = 1;
        canAim = true;
    }
    public void ButtonSelection3(){
        // Se presiono el boton 3
        butSelAbil = 2;
        canAim = true;
    }

    public void ButtonActor1(){
        // Se presiono el boton 1
        aimedActor = 0;
        canAim = false;
    }
    public void ButtonActor2(){
        // Se presiono el boton 2
        aimedActor = 1;
        canAim = false;
    }
    public void ButtonActor3(){
        // Se presiono el boton 3
        aimedActor = 2;
        canAim = false;
    }
}
