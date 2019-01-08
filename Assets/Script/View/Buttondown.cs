using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Buttondown : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{

    public Control CNT;
    public bool isMove = true;
    public void OnPointerDown(PointerEventData eventData)
    {

        if (CNT.GM.downShape != null)
        {
            CNT.GM.downShape.DownFast();
        }
       
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        isMove = false;
    }

  
}
