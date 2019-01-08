using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour {
    //负责生成方块，与ai和玩家的装璜
    public bool isPause = true;
    public Shape[] shapes;

    public GameObject AINextObj;
    public GameObject PlayNextObj;

    public Color[] colors;
    public Shape downShape = null;
    public Control CNT;
    public Transform shapeHome;
    public GameAI AI;

    public int  AINextShape;
    public int  PlayNextShape;


  
    public bool isAI = false;
    
    private void Awake()
    {
       
        CNT = GetComponent<Control>();
        shapeHome = transform.Find("ShapeHome");

    }
  
    
    void Update () {
        if (isPause) return;
        if (downShape == null)
        {
          //如果没有正在下落的方块就生成方块

             CreateShape();
         
            if (isAI)
            {
                //如果是AI模式就让AI来玩
                downShape.isPlayer = false;
                AI.ElsAI();
               
            }
           
           
           
            DestroyShape();
        }

	}
    public void StartGame()
    {
        isPause = false;
        if (downShape!=null)
        {
            downShape.ISGo();
        }

    }
    public void PauseGame()
    {
        //暂停游戏
        isPause = true;
        if (downShape != null)
        {
            downShape.IsStop();



        }
    }
    void CreateShape()
    {
        CreateShapeOrAI();
    }
    

    public void CreateShapeOrAI()
    {
        //创建方块
        int index = Random.Range(0, shapes.Length);
        int cindex = Random.Range(0, colors.Length);

        downShape = GameObject.Instantiate(shapes[index]);
        downShape.Inti(colors[cindex], CNT, this.GetComponent<GameManager>());
        downShape.transform.SetParent(shapeHome);
      
    }

  
   
  

    public void DestroyShape()
    {
        //删除没有实体的方块
        foreach (Transform ch in shapeHome)
        {
            if (ch.childCount<=1)
            {
                Destroy(ch.gameObject);
            }
        }
    }

   
}
