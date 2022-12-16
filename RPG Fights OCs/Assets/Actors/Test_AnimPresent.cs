using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AnimPresent : MonoBehaviour
{
    Animator anim;

    public bool isMoving;
    public bool isIdle;
    public int state;
    public int abilNum;
    public int abilTurn;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsIdle", isIdle);
        anim.SetInteger("State", state);
        anim.SetInteger("AbilTurn", abilTurn);
        anim.SetInteger("AbilNum", abilNum);
    }
    void Update()
    {
        
    }
}
