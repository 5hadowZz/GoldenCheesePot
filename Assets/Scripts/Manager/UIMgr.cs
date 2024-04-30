using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIMgr : MonoBehaviour
{
    private static UIMgr instance;
    public static UIMgr Instance => instance;
    private GameObject questLine;
    private GameObject questLineInfo;

    [Header("状态栏")]
    public Image head;
    public Image state;
    public GameObject power;
    public GameObject hp;
    [Header("待使用Sprite")]
    public Sprite stateMaxHP;
    public Sprite stateNormal;
    [Space(10)]
    public GameObject bagPanel;
    public Image bagIcon;
    public Sprite bagOpen;
    public Sprite bagClose;
    [Space(10)]
    public GameObject questPanel;
    public Image questIcon;
    public Sprite questOpen;
    public Sprite questClose;
    public GameObject questComplete;
    [Space(10)]
    public GameObject potIcon;
    [Space(10)]
    public Image fadePanel;


    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  // TAB键开关背包
        {
            OpenCloseBag();
        }

        if (Input.GetKeyDown(KeyCode.Q))    // Q键开关任务面板
        {
            OpenCloseQuest();
        }
    }


    private void Start()
    {
        questLine = Resources.Load<GameObject>("Quest/QuestLine");
        questLineInfo = Resources.Load<GameObject>("Quest/QuestLineInfo");
    }


    /// <summary>
    /// 更新血量UI
    /// </summary>
    /// <param name="preHP">传入血量数值更新前的HP</param>
    /// <param name="curHP">传入血量数值更新后的HP</param>
    /// <param name="isAddHP">是否是加血</param>
    public void UpdateHP(int preHP, int curHP, bool isAddHP)
    {
        if (!isAddHP)
        {
            for (int i = preHP; i > curHP; i--)
            {
                hp.transform.GetChild(i - 1).gameObject.SetActive(false);
            }

            if (state.sprite == stateMaxHP)
            {
                state.sprite = stateNormal;
                state.SetNativeSize();
            }
        }
        else
        {
            for (int i = preHP; i < curHP; i++)
            {
                hp.transform.GetChild(i).gameObject.SetActive(true);
            }

            if (Player.Instance.curHP == Player.Instance.maxHP)
            {
                state.sprite = stateMaxHP;
                state.SetNativeSize();
            }
        }
    }


    /// <summary>
    /// 更新能量UI
    /// </summary>
    /// <param name="curPower">传入能量数值更新后的数字</param>
    /// <param name="isAddPower">是否是加能量</param>
    public void UpdatePower(int curPower, bool isAddPower)
    {
        if (!isAddPower)
        {
            hp.transform.GetChild(curPower).gameObject.SetActive(false);
        }
        else
        {
            hp.transform.GetChild(curPower - 1).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 打开/关闭背包
    /// </summary>
    public void OpenCloseBag()
    {
        if (QuestMgr.Instance.isOpen || DialogueMgr.Instance.gameObject.activeInHierarchy)
            return;

        bagPanel.SetActive(!BagMgr.Instance.isOpen);            // 开闭背包面板
        bagIcon.sprite = BagMgr.Instance.isOpen ? bagClose : bagOpen;   // 改变背包开闭图标
        bagIcon.SetNativeSize();                                        // 设置原始大小

        Player.Instance.canMove = BagMgr.Instance.isOpen;       // 背包开启时 人物不能移动

        BagMgr.Instance.isOpen = !BagMgr.Instance.isOpen;       // 设置背包打开状态 bool

        BagMgr.Instance.tip.SetActive(false);                   // 将提示框关闭
    }


    /// <summary>
    /// 打开/关闭任务面板
    /// </summary>
    public void OpenCloseQuest()
    {
        if (BagMgr.Instance.isOpen || DialogueMgr.Instance.gameObject.activeInHierarchy)
            return;

        questPanel.SetActive(!QuestMgr.Instance.isOpen);            // 开闭任务面板
        questIcon.sprite = QuestMgr.Instance.isOpen ? questClose : questOpen;   // 改变任务面板开闭图标
        bagIcon.SetNativeSize();                                        // 设置原始大小

        Player.Instance.canMove = QuestMgr.Instance.isOpen;         // 任务面板开启时  人物不能移动

        QuestMgr.Instance.isOpen = !QuestMgr.Instance.isOpen;       // 设置任务面板打开状态 bool
    }


    /// <summary>
    /// 增加任务到UI显示
    /// </summary>
    public void AddQuest(Quest quest)
    {
        // 加载QuestLine并实例化    并设置父对象为questPanel
        GameObject questLine = Instantiate(this.questLine, questPanel.transform);

        // QuestLine脚本中添加Quest  方便删除任务时判断UI进行删除
        questLine.GetComponent<QuestLine>().quest = quest;
        // 修改QuestLine中的背景图
        questLine.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>(quest.NpcQuestBgResPath);
        // 在QuestLineInfos中添加任务所需物品信息
        Transform father = questLine.transform.Find("QuestLineInfos");
        for (int i = 0; i < quest.items.Count; i++)
        {
            GameObject info = Instantiate(questLineInfo, father);
            info.GetComponentInChildren<Text>().text = quest.itemNums[i] + " x";
            info.GetComponentInChildren<Image>().sprite = quest.items[i].GetSprite();
        }
    }


    /// <summary>
    /// 移除任务的UI显示
    /// </summary>
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < questPanel.transform.childCount; i++)
        {
            QuestLine questLine = questPanel.transform.GetChild(i).GetComponent<QuestLine>();

            if (questLine != null && questLine.quest.questName == quest.questName)
                Destroy(questPanel.transform.GetChild(i).gameObject);
        }
    }


    /// <summary>
    /// 切换玩家花洒状态
    /// </summary>
    public void ChangePottingState()
    {
        Player.Instance.isPotting = !Player.Instance.isPotting;
    }
}
