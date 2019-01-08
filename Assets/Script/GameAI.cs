using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAI : MonoBehaviour {

    //尝试着对当前下落方块的4种旋转，与10种坐标摆放，共四十摆放方法打分
   
    public Model model;
    public Control control;
    public Shape modelShape;
    //虚拟的下落方块
    public const int MAX_ROW = 21;
    public const int MAX_COL = 10;
    public const int NONL_ROW = 18;
 
    public Transform[,] map = new Transform[MAX_COL, MAX_ROW];
    //虚拟map
    public int Count;
    public int Score = 0;
    public Vector3 tagPos = new Vector3(4,18,0);
    public Vector3 tagEuler = new Vector3(0,0,0);
    public Vector3 localEuler;
    public List<int> deleteRow;
    
    public void ElsAI()
    {
        //先将下落的方块与网格中摆好的位置信息赋给虚拟方块与虚拟网格
        //再是遍历4种角度，90,180,270,360，和遍历10种位置0-9的坐标，总共是40种
        //模拟下落，在对下落的后的网格做总体的评分，分高者，将位置与角度发送给正在下落的方块
        ToModelMap();
        ToModelShape();

        int CountNum = 0;//分数

        for (int i = 0; i < 4; i++)
        {

            modelShape.transform.RotateAround(modelShape.P.position, Vector3.forward, -90);
            localEuler = modelShape.transform.localEulerAngles;
            //Debug.Log("1e"+modelShape.transform.localEulerAngles);
            for (int j = 0; j < 10; j++)
            {
                ToModelMap();
                ToModelShape();
                modelShape.transform.localEulerAngles = localEuler;
                modelShape.transform.position = new Vector3(j, 18, 0);

                if (!control.model.isCanMove(modelShape.transform))
                {
                    // Debug.Log(modelShape.transform.position + "2e" + modelShape.transform.localEulerAngles);
                    //如果初始的位置不适合就跳过这一个
                    continue;
                }
                else
                {
                    
                    //Debug.Log(modelShape.transform.position + "3e" + modelShape.transform.localEulerAngles);

                    while (true)
                    {
                        Vector3 pos = modelShape.transform.position;
                        pos.y -= 1;
                        modelShape.transform.position = pos;
                        //模拟下落

                        if (!control.model.isCanMove(modelShape.transform))
                        {
                            pos.y += 1;
                            modelShape.transform.position = pos;

                            foreach (Transform I in modelShape.transform)
                            {
                                if (I.tag == "block")
                                {
                                    Vector2 pos1 = control.model.Round(I.position);
                                    map[(int)pos1.x, (int)pos1.y] = I;
                                }
                            }
                           //评分
                            CountNum++;
                            if (CountNum == 1)
                            {

                                Score = PierreDellacherie();
                                tagPos = modelShape.transform.position;
                                tagEuler = modelShape.transform.localEulerAngles;
                            }
                            else
                            {
                                if (PierreDellacherie() > Score)
                                {
                                    Score = PierreDellacherie();
                                    tagPos = modelShape.transform.position;
                                    tagEuler = modelShape.transform.localEulerAngles;

                                }

                            }
                            break;
                        }
                        if (pos.y == -3)
                        {
                            //以防万一死循环
                            Debug.Log(1);
                            break;
                        }
                    }

                   
                }


            }


        }

       // Debug.Log("pos" + tagPos + "e" + tagEuler + "Score" + Score);
        control.GM.downShape.GoTager(tagPos, tagEuler);
        // control.GM.downShape.GoTager(new Vector3(0,0,0), new Vector3(0, 0, 0));

    }



    // 对每一种摆法进行评价。评价包含如下7项指标
    //正常的就包含了6个指标。ISGameOver 是多加的

    public int PierreDellacherie()
    {
         int Lh = LandingHeight();
         int RL = Rowseliminated();
         int RT = RowTransitions();
         int CT = ColumnTransitions();
         int NO = NumberOfHoles();
         int WS = WellSums();
         int isOver = ISGameOver();
         int ss = -45 * Lh + 34 * RL - 32 * RT - 93 * CT - 79 * NO - 34 * WS+ isOver;

       

        //Debug.Log("     " + "pos" + modelShape.transform.position + "e" + modelShape.transform.localEulerAngles + "Score" + ss + "    Lh:   " +Lh +"   RL:  "+ RL+"   RT:  "+ RT+"    CT:  "+ CT+"    NO:  "+ NO+"   WS:  "+ WS);
        return ss;
    }

    public int LandingHeight()
    {
        //高度数，方块中心到网格最低端的高度
        //越高当然越不好所以要减分
        return (int)modelShape.P.transform.position.y;
    }

    public int Rowseliminated()
    {
        //方块贡献数
        //如果有消除了，就算算下落的方块，贡献了几个自己的方块
        //     1
        //     1
        //     1
        //     1
        //
        //11111011111
        //这个下落的长条如果直接下落就贡献了一个自己的方块
        deleteRow.Clear();
        int ChilCount = 0;
        if (CheckMap())
        {
            foreach (Transform t in modelShape.transform)
            {
                if (t.tag =="block")
                {
                    Vector2 pos1 = control.model.Round(t.position);

                    foreach (int r in deleteRow)
                    {
                        if ((int)pos1.y == r )
                        {
                         //   Debug.Log(pos1.x + "  " + pos1.y);
                            ChilCount++;
                        }
                    }

                    
                }
            }


          //  Debug.Log("ChilCount" + ChilCount);
            return Count * ChilCount;
        }
        else
        {
            return 0;
        }
       
    }
    public int WellSums()
    {

        // 井的总和数
        // 注意一列中可能有多个井，如图： 
        // ■■■ 
        // ■□□  0
        // ■□■  1
        // ■■■ 
        // ■■■
        // ■□■  1
        // ■□■  2
        // 中间一列为井，深度连加到一的和为   1+  (1+2)
        int sumNum = 0;
        for (int i = 0; i < MAX_COL; i++)
        {
            int colCount = 0;
            for (int j = NONL_ROW - 1; j >= 0; j--)
            {
                if (map[i, j] != null) { colCount = 0; continue; }

                if (i == 0)
                {
                    //最左边的第一列

                    if (map[1, j] != null && map[i, j] == null)
                    {
                        colCount++;
                        sumNum += colCount;
                        // Debug.Log(i +" |"+j + "colcount:" + colCount + "sumNum:" + sumNum);

                    }
                    else
                    {
                        colCount = 0;
                        //   Debug.Log(i + "| " + j + "colcount:" + colCount + "sumNum:" + sumNum);

                    }



                }
                else if (i == MAX_COL - 1)//如果是最右边的一列
                {

                    if (map[i - 1, j] != null && map[i, j] == null)
                    {
                        colCount++;
                        sumNum += colCount;
                        //  Debug.Log(i + " | " + j + "colcount:" + colCount + "sumNum:" + sumNum);
                    }
                    else
                    {

                        colCount = 0;
                        //     Debug.Log(i + " | " + j + "colcount:" + colCount + "sumNum:" + sumNum);
                    }

                }
                else//如果是中间的几列
                {

                    if (map[i - 1, j] != null && map[i, j] == null && map[i + 1, j] != null)
                    {
                        colCount++;
                        sumNum += colCount;
                        //   Debug.Log(i + " | " + j + "colcount:" + colCount + "sumNum:" + sumNum);
                    }
                    else
                    {
                        colCount = 0;
                        // Debug.Log(i + " | " + j + "colcount:" + colCount + "sumNum:" + sumNum);
                    }



                }


            }
        }
        return sumNum;
    }


    public int NumberOfHoles()
    {
        //空洞数
        //■□□
        //■□□■■■■■■■■■  
        //■■■■□□■■■■■■  有两个空洞 
        //■■■■■■■■■■■■  一列一列的看 有两个空洞 有裸露的不算

        int AllNullCount = 0;
        int maxR = 0;
        bool isMax = false;
        for (int i = NONL_ROW - 1; i >= 0; i--)
        {

            for (int j = 0; j < MAX_COL; j++)
            {
                if (map[j, i] != null)
                {
                    maxR = i;
                    isMax = true;
                    break;
                }
            }
            if (isMax) break;
            //break;
        }

        for (int j = 0; j < MAX_COL; j++)
        {
            int KongCount = 0;

            for (int i = maxR - 1; i >= 0; i--)
            {

                if (map[j, i] != null) { continue; }
                if (i == maxR - 1)
                {
                    if (map[j, i + 1] == null)
                    {
                        while (true)
                        {
                            i--;
                            if (i < 0)
                            {


                                break;
                            }
                            else
                            {
                                if (map[j, i] != null)
                                {


                                    break;
                                }
                            }
                        }
                        continue;
                    }

                }
                //Debug.Log(j + "  " + i);
                while (true)
                {

                    i--;
                    if (i < 0)
                    {
                        KongCount++;
                        AllNullCount += KongCount;

                        break;
                    }
                    else
                    {
                        if (map[j, i] != null)
                        {
                            KongCount++;
                            AllNullCount += KongCount;

                            break;
                        }
                    }

                }

            }

        }


        return AllNullCount;
    }

    public int RowTransitions()
    {
        //一行一行看
        //每一次改变就算作一次变换数加一
        //■□■□■■  变换数为4
        //■■□□□□  变换数为2  墙边算实心的
        //■□□□■■  变换数为2
        //■■■■■■  变换数为0
        // 变换数为  0+2+2+4

        int AllRowTranCount = 0;
        for (int i = 0; i < NONL_ROW; i++)
        {

            bool isNull = false;
            int RowTranCount = 0;
            for (int j = 0; j <= MAX_COL; j++)
            {
                bool ColisNull = false;

                if (j == 0)
                {
                    //第一列 如果是空 就是一次变换  isNUll 代表为空
                   // Debug.Log(j + "  " + i);
                    if (map[j, i] == null) { RowTranCount++; isNull = true; } //*
                }
                else if (j == MAX_COL)
                {
                    //第十列是最右边的边框 看之前的isnull 是否为空
                    if (isNull) { RowTranCount++; }
                    AllRowTranCount += RowTranCount;

                }
                else
                {
                    if (map[j, i] != null)//*
                    {
                        ColisNull = false;
                    }
                    else
                    {
                        ColisNull = true;
                    }

                    if (ColisNull != isNull)
                    {
                        RowTranCount++;
                        isNull = ColisNull;
                    }
                }

            }
           // Debug.Log("第：" + i + "行变化：" + RowTranCount);

        }

        return AllRowTranCount;
    }

    public int ColumnTransitions()
    {
        //列变换
        //与行变换一样只不过是一列一列算
        //■□■□■■  
        //■■□□□■     墙边四周算实心的
        //■□□□■■  
        //■■■■■□ 
        //042222
        //列变换 :0+4+2+2+2+2
        int AllColTranCount = 0;
        for (int i = 0; i < MAX_COL; i++)
        {
            bool isNull = false;
            int ColTranCount = 0;
            for (int j = 0; j <= NONL_ROW; j++)
            {
                bool ColisNull = false;

                if (j == 0)
                {
                    if (map[i, j] == null) { ColTranCount++; isNull = true; }//*
                }
                else if (j == NONL_ROW)
                {
                    if (isNull) { ColTranCount++; }
                    AllColTranCount += ColTranCount;

                }
                else
                {
                    if (map[i, j] != null)//*
                    {
                        ColisNull = false;
                    }
                    else
                    {
                        ColisNull = true;
                    }

                    if (ColisNull != isNull)
                    {
                        ColTranCount++;
                        isNull = ColisNull;
                    }
                }

            }
        }
        return AllColTranCount;
    }

    public int ISGameOver()
    {
        //判断是否输了
        int num = 0;
        for (int i = 18; i < MAX_ROW; i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {

                if (map[j, i] != null)
                {

                    num += -1000;
                }
            }
        }
        return num ;
    }


    public bool CheckMap()
    {
        //检测是否有消除的行
        Count = 0;
        for (int r = 0; r < MAX_ROW; r++)
        {
            if (CheckOneRow(r))
            {
                Count++;
                deleteRow.Add(r);
            }
        }
        if (Count > 0)
        {

            return true;
        }
        else
            return false;
    }
    public bool CheckOneRow(int r)
    {
        //把完成消除的的方块变为空
        for (int i = 0; i < MAX_COL; i++)
        {
            if (map[i, r] == null) return false;
        }
        return true;
    }
  
    public void ToModelShape()
    {
        //把现在下落的方块的数据赋给这个脚本下的虚拟方块
        int i = 0;
        foreach (Transform t in modelShape.transform)
        {
            t.localPosition = control.GM.downShape.transform.GetChild(i).localPosition;
            i++;
        }

    }
   

    public void ToModelMap()
    {
        //获取网格数据
        for (int i = 0; i < MAX_ROW; i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {
                map[j, i] = control.model.map[j, i];


            }
        }

    }
}
