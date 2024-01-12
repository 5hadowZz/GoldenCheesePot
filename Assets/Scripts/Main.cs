using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Scene1");
    }
}
