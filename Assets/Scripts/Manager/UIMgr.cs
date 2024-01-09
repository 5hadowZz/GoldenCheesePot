using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    private static UIMgr instance;
    public static UIMgr Instance => instance;

    [Header("״̬��")]
    public Image head;
    public Image state;
    public GameObject power;
    public GameObject hp;
    [Header("��ʹ��Sprite")]
    public Sprite stateMaxHP;
    public Sprite stateNormal;

    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// ����Ѫ��UI
    /// </summary>
    /// <param name="curHP">����Ѫ����ֵ���º��curHP</param>
    /// <param name="isAddHP">�Ƿ��Ǽ�Ѫ</param>
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
    /// ��������UI
    /// </summary>
    /// <param name="curPower">����������ֵ���º������</param>
    /// <param name="isAddPower">�Ƿ��Ǽ�����</param>
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
}
