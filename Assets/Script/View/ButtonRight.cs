using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Control CNT;
    public bool isMove = true;
    public void OnPointerDown(PointerEventData eventData)
    {
        isMove = true;
        StartCoroutine("GoLeftOrRight");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        isMove = false;
    }

    private IEnumerator GoLeftOrRight()
    {

        while (true)
        {
            if (!isMove) break;
            if (CNT.GM.downShape == null) break;
            CNT.GM.downShape.isPlayer = true;
            CNT.GM.isAI = false;
            if (!CNT.GM.downShape.InputControl(1)) break;


            yield return new WaitForSeconds(0.15f);

        }



    }
}
