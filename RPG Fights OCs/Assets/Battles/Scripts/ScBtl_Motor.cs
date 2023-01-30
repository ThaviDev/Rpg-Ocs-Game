using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScBtl_Motor : MonoBehaviour
{
    public BattleManager btlMan;
    public Btl_UI_Manager uIMan;
    public Camera cam;
    //public List<ActorMotor> listActorsMotor = new List<ActorMotor>();
    /*
    public int m_ui_butSelAbil; // boton de la selección de habilidades
    public int m_bm_actorsSel; // actor siendo seleccionado en UI manager
    public int m_ui_aimedActor; // Personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public int m_bm_possibleAimedActor;// Posible personaje seleccionado en la parte de apuntar a un personaje en la interfaz
                                       // (cuando tienes que elegir a un personaje para aplicar la habilidad)
    public bool m_bm_RestartValues1; // Reiniciar Valores
    */
    //public int m_btl_actorsSel; // actor siendo seleccionado en battle manager

    void Start()
    {
        cam = FindObjectOfType<Camera>();
        btlMan = FindObjectOfType<BattleManager>();
        uIMan = FindObjectOfType<Btl_UI_Manager>();
        StartCoroutine("LateStart");
        //Time.timeScale = 5;
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        uIMan.actorsAmount = btlMan.actorsMotor.Count;
        uIMan.alliesAmount = btlMan.quantityOfAllies;
        uIMan.enemiesAmount = btlMan.quantityOfEnemies;
    }

    // Reinicia los valores del seleccionador de habilidades
    /*
    public void RestartValues_BattleSelection()
    {
        print("Estoy en restart values");
        //UIMan.butSelAbil = -1;
        //UIMan.aimedActor = -1;
        BtlMan.aimedActor = -1; // Personaje apuntado reinicia
        BtlMan.abilitySelected = -1; // Boton seleccionado reinicia
    }
    */
    /* MOVER LA CAMARA A LA POSICIÓN DEL ACTOR PARA VOLVERLO MAS CINEMATICO
     * TIENE EL PROBLEMA DE HACER LA POSICION DE LA SELECCION DE HABILIDADES SE MUEVA MIENTRAS INTENTAS TOCARLA
     * ADEMAS DE QUE PUEDE DESORIENTAR
    public void MoveCamaraOnTarget()
    {
        cam.transform.position = new Vector3(BtlMan.actorsTransPos[UIMan.selectedActor].transform.position.x,
            BtlMan.actorsTransPos[UIMan.selectedActor].transform.position.y, -10);
    }
    */
    // Cuando se selecciona una habilidad, por aquí se pasa el input
    public void AbilInput(int _button)
    {
        btlMan.abilCheckInput = true;
        btlMan.abilitySelected = _button;
    }
    public void AimInput(int _button)
    {
        btlMan.aimedActor = _button;
    }
    public void UpdateUI_Data()
    {
        uIMan.UpdateUI_Data();
    }

    void Update()
    {
        // Igualar las posiciones de la selección de habilidades con la cantidad de aliados que existen (hay que pensarle)
        /* Si battle manager esta en Selección de habilidades
         * igualar uIMan(actorSelectionStatus) a btlMan(actorSuccesion)
         * igualar uIMan(currentActor) a btlMan(currentActor)
         * Si btlMan(possibleAimedActor) es 0 o 1 entonces uIMan(aimStatus) se iguala
         * 
         * Else
         * igualar uIMan(actorSelectionStatus) = -1
         * 
         */
        /*

        */

        // Si las batallas estan en selección de habilidades de los personajes
        
        if (btlMan.battleSuccesion == 0)
        {
            uIMan.actorSelectionStatus = btlMan.actorSuccesion;
            uIMan.currentActor = btlMan.currentActor;
            uIMan.aimStatus = (int)btlMan.possibleAimedActor;

            
            //if (btlMan.possibleAimedActor >= 2)
            //{
              //  uIMan.aimStatus = (int)btlMan.possibleAimedActor;
            //}


        }
        else
        {
            uIMan.actorSelectionStatus = -1;
        }
        
        
    }
    public void CheckEndBattle(bool didVictory)
    {

        // Si se gana
        if (didVictory)
        {

        } 
        // Si se pierde
        else
        {

        }
    }
}
