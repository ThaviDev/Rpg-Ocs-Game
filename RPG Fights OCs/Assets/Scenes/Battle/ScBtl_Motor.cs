using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScBtl_Motor : MonoBehaviour
{
    public BattleManager BtlMan;
    public UI_Manager UIMan;
    public Camera cam;
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
        BtlMan = FindObjectOfType<BattleManager>();
        UIMan = FindObjectOfType<UI_Manager>();
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
        BtlMan.abilCheckInput = true;
        BtlMan.abilitySelected = _button;
    }
    public void AimInput(int _button)
    {
        BtlMan.aimedActor = _button;
    }
    public void UpdateUI_Variables(bool canAct, int actorSel, int possAim)
    {
        UIMan.charCanAct = canAct;
        UIMan.selectedActor = actorSel;
        UIMan.possibleAimedActor = possAim;
        UIMan.UpdateUI_Elements();
    }

    void Update()
    {
        
    }
}
