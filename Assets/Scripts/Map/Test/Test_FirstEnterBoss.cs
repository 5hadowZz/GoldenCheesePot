using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Test_FirstEnterBoss : MonoBehaviour
{
    public E_DialogueNPC npc;
    //public GameObject bossObj;
    [TextArea]
    public string[] lines;
    public CinemachineVirtualCamera cam;

    public PlayableDirector director;
    public GameObject boss;


    private void Awake()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果是第一次进 才显示对话 并设置为已经进入过
        if (GameDataMgr.Instance.SceneData.isFirstMeetBoss1)
        {
            //GameObject boss = Instantiate(bossObj);
            //boss.transform.position = new Vector3(-5.29f, -32.27f, 0f);

            Player.Instance.canMove = false;
            GameDataMgr.Instance.SceneData.isFirstMeetBoss1 = false;
            // 设置Barrier阻挡  强制对战  防止玩家跳出场景
            transform.GetChild(0).gameObject.SetActive(true);

            boss.SetActive(true);
            cam.Follow = boss.transform;
            director.Play();
            director.stopped += (param) => 
            {
                DialogueMgr.Instance.ShowDialogue(npc, lines, OnDialogueOver);
            };   
        }
    }


    public void OnDialogueOver()
    {
        cam.Follow = Player.Instance.gameObject.transform;
        Player.Instance.canMove = true;
        FindObjectOfType<StateMachine>().paramters.target = Player.Instance.transform;
        FindObjectOfType<StateMachine>().ChangeState(E_BossState.Dash);
    }
}
