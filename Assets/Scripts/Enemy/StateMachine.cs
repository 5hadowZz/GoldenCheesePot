using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_EnemyState
{
    Wait,
    Attack
}


public class StateMachine : MonoBehaviour
{
    public Dictionary<E_EnemyState, BaseState> stateDic;
    public BaseState curState;


    private void Start()
    {
        
    }
}
