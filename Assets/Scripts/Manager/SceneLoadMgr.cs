using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadMgr : MonoBehaviour
{
    private static SceneLoadMgr instance;
    public static SceneLoadMgr Instance => instance;

    public string sourceScene;
    public Scene curScene;


    private void Awake()
    {
        instance = this;
    }



    public void Load(string sceneName)
    {

        // 屏幕变黑
        UIMgr.Instance.fadePanel.DOFade(1, 1f).OnComplete(() =>
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);     // 加载场景

            ao.completed += (param) =>
            {
                //加载完成后 存储数据
                // 这里到时候新建Scene给主标题场景  判断  如果是主标题场景  就不执行下面的Save
                //if (GameDataMgr.Instance != null)
                //{
                //    GameDataMgr.Instance.Save();
                //}

                // DOFade回去
                UIMgr.Instance.fadePanel.DOFade(0, 1f);
                curScene = SceneManager.GetActiveScene();
                MusicMgr.Instance.ChangeSceneMusic();
            };
        });
    }


    public void OnPlayerDeadFromBoss1()
    {
        FindObjectOfType<StateMachine>().paramters.target = null;
        DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Boss, new string[] { "沉睡吧..." }, InnerFunc);

        // Player死亡 Boss对话完毕后调用
        void InnerFunc()
        {
            Player.Instance.canMove = false;

            UIMgr.Instance.fadePanel.DOFade(1, 2f).OnComplete(() =>
            {
                AsyncOperation ao = SceneManager.LoadSceneAsync("Scene3_Main");

                ao.completed += (param) =>
                {
                    curScene = SceneManager.GetActiveScene();
                    Player.Instance.transform.position = new Vector3(-8f, -49f, 0f);
                    UIMgr.Instance.fadePanel.DOFade(0, 1f).OnComplete(() =>
                    {
                        MusicMgr.Instance.ChangeSceneMusic();
                        DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[]
                        {"......",
                         "你醒了吗。",
                         "我在野外发现你...",
                         "那时你已经倒下了...",
                         "嗯...不用说，我知道发生了什么。",
                         "......",
                         "继续上路吧，孩子。"
                        });
                        Player.Instance.curHP = Player.Instance.maxHP;
                        UIMgr.Instance.UpdateHP(0, Player.Instance.curHP, true);
                    });
                };
            });
        }
    }


    public void OnPlayerDeadFromBoss2()
    {
        FindObjectOfType<StateMachine>().paramters.target = null;

        List<string> playerDeadLines = new() { "真是没用……", "你顶多就是条小狗吧。", "迷惘可是会没命的。" };
        DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Boss, new string[] { playerDeadLines[Random.Range(0, playerDeadLines.Count)] }, InnerFunc);

        // Player死亡 Boss对话完毕后调用
        void InnerFunc()
        {
            Player.Instance.canMove = false;

            UIMgr.Instance.fadePanel.DOFade(1, 2f).OnComplete(() =>
            {
                AsyncOperation ao = SceneManager.LoadSceneAsync("Scene3_Main");

                ao.completed += (param) =>
                {
                    curScene = SceneManager.GetActiveScene();
                    Player.Instance.transform.position = new Vector3(-8f, -49f, 0f);
                    UIMgr.Instance.fadePanel.DOFade(0, 1f).OnComplete(() => 
                    {
                        MusicMgr.Instance.ChangeSceneMusic();
                        Player.Instance.canMove = true;
                        Player.Instance.curHP  = Player.Instance.maxHP;
                        UIMgr.Instance.UpdateHP(0, Player.Instance.curHP, true);
                    });
                };
            });
        }
    }
}
