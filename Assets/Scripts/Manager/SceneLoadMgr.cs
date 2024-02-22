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
            };
        });
    }
}
