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
        UIMgr.Instance.fadePanel.DOFade(1, 1f).OnComplete(() =>
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

            ao.completed += (param) =>
            {
                // DOFadeªÿ»•
                UIMgr.Instance.fadePanel.DOFade(0, 1f);
            };
        });
    }
}
