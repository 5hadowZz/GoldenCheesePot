using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Test_FirstEnterBoss2 : MonoBehaviour
{
    public E_DialogueNPC npc;
    //public GameObject bossObj;
    [TextArea]
    public string[] firstMeet;
    [TextArea]
    public string[] againMeet;

    public GameObject boss2;
    public CinemachineVirtualCamera cam;
    //public PlayableDirector director;
    private bool curSceneMeet;


    private void Awake()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();

        if (GameDataMgr.Instance.SceneData.isKilledBoss2)
            Destroy(boss2);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameDataMgr.Instance.SceneData.isKilledBoss2 && !curSceneMeet)
        {
            curSceneMeet = true;
            cam.Follow = boss2.transform;
            //director.Play();


            Player.Instance.canMove = false;
            if (GameDataMgr.Instance.SceneData.isFirstMeetBoss2)
            {
                DialogueMgr.Instance.ShowDialogue(npc, firstMeet, OnDialogueOver);
                GameDataMgr.Instance.SceneData.isFirstMeetBoss2 = false;
            }
            else
            {
                DialogueMgr.Instance.ShowDialogue(npc, againMeet, OnDialogueOver);
            }

            // 设置Barrier阻挡  强制对战  防止玩家跳出场景
            transform.GetChild(0).gameObject.SetActive(true);
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
