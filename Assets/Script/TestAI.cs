using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public const int MAX_ROW = 18;
    public const int MAX_COL = 10;
    public const int NONL_ROW = 18;
    public GameObject block;
    public GameObject[,] map =
      new GameObject[MAX_COL, MAX_ROW];
    public int[,] Fallmap =
        new int[MAX_COL, MAX_ROW]
        {   {1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{1,0,0,1},
            //{1,0,1,1},
            //{1,1,0,0},
            //{1,1,1,0},
         };

    //new int[3, 7] 
    //    {   {1,1,1,1,1,1,1},
    //        {0,0,0,1,0,0,0},
    //        {1,1,1,1,1,1,0}


    //     };
    // Use this for initialization
    void Start()
    {
        FallMap();
         Debug.Log(1);
         Debug.Log("空洞数： " + NumberOfHoles());
         Debug.Log("行变换： " + RowTransitions());
         Debug.Log("列变换： " + ColumnTransitions());
         Debug.Log("井的总和： "+WellSums());
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FallMap()
    {
        for (int i = 0; i < NONL_ROW; i++)
        {

            for (int j = 0; j < MAX_COL; j++)
            {
                if (Fallmap[j, i] == 1)
                {
                    GameObject a = Instantiate(block, new Vector3(j, i, 0), Quaternion.identity);
                    map[j, i] = a;
                }
            }
        }
    }

    public void CreateMap()
    {
        for (int i = 0; i < MAX_ROW; i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {
                GameObject a = Instantiate(block, new Vector3(j, i, 0), Quaternion.identity);
                if (map[j, i] = null)
                {
                    a.GetComponent<SpriteRenderer>().color = Color.black;
                }
                else
                {
                    a.GetComponent<SpriteRenderer>().color = Color.white;

                }
            }
        }
    }

    public int WellSums()
    {

        // 6.井的总和（Well Sums）： 
        // 井指两边皆有方块的空列。该指标为所有井的深度连加到1再求总和
        // 注意一列中可能有多个井，如图： 
        // ■□□ 
        // ■□■ 
        // ■□■ 
        // ■■■ 
        // ■□■ 
        // ■□■ 
        // ■□■ 
        // 中间一列为井，深度连加到一的和为(2 + 1) + (3 + 2 + 1) = 9
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
    {//空洞数（Number of Holes） 
        int AllNullCount = 0;
        int maxR = 0;
        bool isMax = false;
        for (int i = NONL_ROW-1; i >= 0; i--)
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
            
            for (int i = maxR-1; i >= 0; i--)
            {

                    if(map[j, i] != null) { continue; }
                    if (i == maxR - 1)
                    {
                        if (map[j, i + 1] == null) {
                        while (true)
                        {
                            i--;
                            if (i < 0)
                            {


                                break ;
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
                    Debug.Log(j+"  "+ i);
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
        //从左到右（或者反过来）检测一行，当该行中某个方格从有方块到无方块（或无方块到有方块）， 
        //视为一次变换。游戏池边界算作有方块。行变换从一定程度上反映出一行的平整程度，越平整值越小
        //该指标为所有行的变换数之和
        //如图：■表示有方块，□表示空格（游戏池边界未画出） 
        //■■□□■■□□■■□□ 变换数为6
        //□□□□□■□■□■□■ 变换数为9
        //■■■■□□□□□□■■ 变换数为2
        //■■■■■■■■■■■■  变换数为0


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
    {//列变换（Column Transitions）：大意同上 
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
}

   