using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DialogueMgr : MonoBehaviour
{
    private static DialogueMgr instance;
    public static DialogueMgr Instance => instance;

    private UnityAction callBack;

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
                Player.Instance.canMove = true;  // 恢复移动

                callBack?.Invoke();     // 对话关闭后  执行回调
            }
        }
    }


    public void ShowDialogue(string[] _lines, UnityAction callBack = null)
    {
        // 如果正在对话  再按E调用该函数没用
        if (dialoguePanel.activeInHierarchy)
            return;

        this.callBack = callBack;

        dialogueText.text = "";

        dialogueLines = _lines;
        currentLine = 0;

        CheckName();

        isScrolling = true;
        dialogueText.DOText(dialogueLines[currentLine], dialogueLines[currentLine].Length / dialogueSpeed).OnComplete(
            () => { isScrolling = false; });
        dialoguePanel.SetActive(true);

        Player.Instance.canMove = false;     // 让主角不能移动
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
