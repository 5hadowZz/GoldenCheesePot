using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_Scene
{
    Scene1,
    Scene1_HideScene,
    Scene1_Shovel,
    Test
}


public class Main : MonoBehaviour
{
    public E_Scene scene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(scene.ToString());
    }
}
