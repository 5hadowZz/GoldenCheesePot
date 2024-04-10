using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;


public enum E_DialogueNPC
{
    Grandma,
    Bear,
    Fox
}


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

    [Header("NPC对话面板")]
    private E_DialogueNPC npc;
    private GameObject curDialoguePanel;
    public GameObject grandmaPanel;
    public GameObject bearPanel;
    public GameObject foxPanel;

    [HideInInspector]
    public GameObject dialogueNPC;

    [SerializeField]
    private int currentLine;    // 当前句子索引
    private bool isScrolling;   // 是否正在输出句子


    private void Awake()
    {
        instance = this;
    }


    private void OnEnable()
    {
        UIMgr.Instance.questComplete.SetActive(false);
    }


    private void OnDisable()
    {
        dialogueText.DOKill();
    }


    private void Start()
    {
        dialoguePanel.SetActive(false);
    }


    private void Update()
    {
        if (dialoguePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            if (isScrolling)
                return;

            currentLine++;

            if (currentLine < dialogueLines.Length)
            {
                dialogueText.text = "";
                //CheckName();
                isScrolling = true;
                dialogueText.DOText(dialogueLines[currentLine], dialogueLines[currentLine].Length / dialogueSpeed).OnComplete(
                    () => { isScrolling = false; });
            }
            else
            {
                dialoguePanel.SetActive(false);
                OpenCloseNpcPanel(npc, false);          // 关闭指定NPC对话面板
                currentLine = 0;
                Player.Instance.canMove = true;  // 恢复移动

                callBack?.Invoke();     // 对话关闭后  执行回调
            }
        }
    }


    public void ShowDialogue(E_DialogueNPC npc, string[] _lines, UnityAction callBack = null)
    {
        // 如果正在对话  再按E调用该函数没用
        if (dialoguePanel.activeInHierarchy)
            return;


        this.callBack = callBack;

        dialogueText.text = "";

        dialogueLines = _lines;
        currentLine = 0;

        //CheckName();

        isScrolling = true;
        dialogueText.DOText(dialogueLines[currentLine], dialogueLines[currentLine].Length / dialogueSpeed).OnComplete(
            () => { isScrolling = false; });
        dialoguePanel.SetActive(true);
        OpenCloseNpcPanel(npc, true);              // 打开指定的NPC对话面板

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


    /// <summary>
    /// 根据NPC枚举名称来显隐指定对话面板
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="OpenOrClose">true为打开面板 false为关闭面板</param>
    private void OpenCloseNpcPanel(E_DialogueNPC npc, bool OpenOrClose)
    {
        this.npc = npc;

        switch (npc)
        {
            case E_DialogueNPC.Grandma:
                curDialoguePanel = grandmaPanel;
                break;


            case E_DialogueNPC.Bear:
                curDialoguePanel = bearPanel;
                break;


            case E_DialogueNPC.Fox:
                curDialoguePanel = foxPanel;
                break;
        }

        curDialoguePanel.SetActive(OpenOrClose);
    }
}
