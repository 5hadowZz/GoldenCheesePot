using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    private static UIMgr instance;
    public static UIMgr Instance => instance;

    [Header("状态栏")]
    public Image head;
    public Image state;
    public GameObject power;
    public GameObject hp;
    public Image bagIcon;
    [Header("待使用Sprite")]
    public Sprite stateMaxHP;
    public Sprite stateNormal;
    public Sprite bagOpen;
    public Sprite bagClose;
    public GameObject bagPanel;
    public Image fadePanel;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  // TAB键打开背包
        {
            OpenCloseBag();
        }
    }


    /// <summary>
    /// 更新血量UI
    /// </summary>
    /// <param name="curHP">传入血量数值更新后的curHP</param>
    /// <param name="isAddHP">是否是加血</param>
    public void UpdateHP(int curHP, bool isAddHP)
    {
        if (!isAddHP)
        {
            hp.transform.GetChild(curHP).gameObject.SetActive(false);

            if (state.sprite == stateMaxHP)
            {
                state.sprite = stateNormal;
                state.SetNativeSize();
            }
        }
        else
        {
            hp.transform.GetChild(curHP - 1).gameObject.SetActive(true);

            if (curHP == Player.Instance.maxHP)
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
        bagPanel.SetActive(!BagMgr.Instance.isOpen);            // 开闭背包面板
        bagIcon.sprite = BagMgr.Instance.isOpen ? bagClose : bagOpen;   // 改变背包开闭图标
        bagIcon.SetNativeSize();                                        // 设置原始大小

        Player.Instance.canMove = BagMgr.Instance.isOpen;       // 背包开启时 人物不能移动

        BagMgr.Instance.isOpen = !BagMgr.Instance.isOpen;       // 设置背包打开状态 bool
    }
}
