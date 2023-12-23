using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueMgr : MonoBehaviour
{
    private static DialogueMgr instance;
    public static DialogueMgr Instance => instance;

    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text nameText;

    public float dialogueSpeed;
    public string[] dialogueLines;
    [SerializeField]
    private int currentLine;    // 当前句子索引
    private bool isScrolling;   // 是否正在输出句子


    private void Awake()
    {
        instance = this;    
    }


    private void Start()
    {
        dialoguePanel.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (dialoguePanel.activeInHierarchy && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isScrolling)
                return;

            currentLine++;       

            if (currentLine < dialogueLines.Length)
            {
                dialogueText.text = "";
                CheckName();
                isScrolling = true;
                dialogueText.DOText(dialogueLines[currentLine], dialogueLines[currentLine].Length / dialogueSpeed).OnComplete(
                    () =>{ isScrolling = false; });
            }
            else
            {
                dialoguePanel.SetActive(false);
                currentLine = 0;
                FindObjectOfType<Player>().canMove = true;  // 恢复移动
            }
        }
    }


    public void ShowDialogue(string[] _lines)
    {
        // 如果正在对话  再按E调用该函数没用
        if (dialoguePanel.activeInHierarchy)
            return;

        dialogueText.text = "";

        dialogueLines = _lines;
        currentLine = 0;

        CheckName();

        isScrolling = true;
        dialogueText.DOText(dialogueLines[currentLine], dialogueLines[currentLine].Length / dialogueSpeed).OnComplete(
            () => { isScrolling = false; });
        dialoguePanel.SetActive(true);

        FindObjectOfType<Player>().canMove = false;     // 让主角不能移动
    }


    private void CheckName()
    {
        if (dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
}
