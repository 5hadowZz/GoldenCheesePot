using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel_in_Wood : MonoBehaviour
{
    private bool canInteract = false;
    private bool isTouch = false;
    public Sprite wood;

    private void Update()
    {
        if (!isTouch && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            isTouch = true;
            GetComponent<SpriteRenderer>().sprite = wood;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }
}
