using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FirstEnterBoss : MonoBehaviour
{
    private bool isFirstEnter = true;
    [TextArea]
    public string[] lines;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果是第一次进 才显示对话 并设置为已经进入过
        if (isFirstEnter)
        {
            isFirstEnter = false;
            Player.Instance.canMove = false;
            DialogueMgr.Instance.ShowDialogue(lines, OnDialogueOver);
        }
    }


    public void OnDialogueOver()
    {
        Player.Instance.canMove = true;
        FindObjectOfType<StateMachine>().paramters.target = Player.Instance.transform;
        FindObjectOfType<StateMachine>().ChangeState(E_BossState.Dash);
    }
}
