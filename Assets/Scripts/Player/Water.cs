using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public void CloseActive()
    {
        Player.Instance.canMove = true;
        gameObject.SetActive(false);
    }
}
