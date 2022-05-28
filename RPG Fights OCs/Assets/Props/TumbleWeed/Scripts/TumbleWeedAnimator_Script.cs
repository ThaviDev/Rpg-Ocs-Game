using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleWeedAnimator_Script : MonoBehaviour
{
    // -- Este codigo es para la animacion y aparicion de un arbusto moviendose en el escenario --
    private Animator anim;
    private float timeToApear;
    void Start()
    {
        anim = GetComponent<Animator>();
        timeToApear = Random.Range(20,50);
    }

    void Update()
    {
        timeToApear -= Time.deltaTime;
        if (timeToApear <= 0){
            anim.SetBool("canMove",true);
            timeToApear = Random.Range(20,50);
        } else {
            anim.SetBool("canMove",false);
        }
    }
}
