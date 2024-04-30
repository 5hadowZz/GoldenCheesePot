using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr : MonoBehaviour
{
    private static MusicMgr instance;
    public static MusicMgr Instance => instance;

    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioClip music4;
    public AudioClip battle;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    public void ChangeSceneMusic()
    {
        AudioClip preMusic = audioSource.clip;

        if (SceneLoadMgr.Instance.curScene.name.StartsWith("Main"))
        {
            audioSource = null;
        }
        else if (SceneLoadMgr.Instance.curScene.name.StartsWith("Scene1"))
        {
            audioSource.clip = music1;
        }
        else if (SceneLoadMgr.Instance.curScene.name.StartsWith("Scene2"))
        {
            audioSource.clip = music2;
        }
        else if (SceneLoadMgr.Instance.curScene.name.StartsWith("Scene3"))
        {
            audioSource.clip = music3;
        }
        else if (SceneLoadMgr.Instance.curScene.name.StartsWith("Scene4"))
        {
            audioSource.clip = music4;
        }

        if (preMusic != audioSource.clip)
            audioSource.Play();
    }


    public void ChangeBattleMusic()
    {
        audioSource.clip = battle;
        audioSource.Play();
    }


    public void SetNullMusic()
    {
        audioSource.clip = null;
    }
}
