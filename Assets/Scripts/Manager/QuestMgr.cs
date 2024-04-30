using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMgr : MonoBehaviour
{
    private static QuestMgr instance;
    public static QuestMgr Instance => instance;

    public List<Quest> quests = new();
    public bool isOpen = false;
    public GameObject zeroQuestPanel;


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        zeroQuestPanel.SetActive(transform.childCount == 1);
    }


    private void Start()
    {
        gameObject.SetActive(false);
    }   
}
