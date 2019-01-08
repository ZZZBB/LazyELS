using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour {


    
    public const int MAX_ROW = 21;
    public const int MAX_COL = 10;
    public Transform[,] map = new Transform[MAX_COL, MAX_ROW];

    public int score = 0;
    public int hscore = 0;
    public int gameNum = 0;
    public int aiscore = 0;
    public int Score { get { return score; } }
    public int AIScore { get { return aiscore; } }
    public int Hscore { get { return hscore; } }
    public int GameNum { get { return gameNum; } }

    public Control cnt;
    private void Awake()
    {
        LaodDate();
    }
    public bool isCanMove(Transform  t)
    {
        foreach (Transform  i in t)
        {
            if (i.tag != "block") continue;
            Vector2 pos = Round(i.position);
            if (!IsInMap(pos)) return false;
           // Debug.Log((int)pos.x + "  " + (int)pos.y);
          //  Debug.Log(map[(int)pos.x, (int)pos.y].tag);
            if (map[(int)pos.x,(int)pos.y]!=null)
            {
                return false;
            }
        }
        return true;
    }
    bool IsInMap(Vector2 pos)
    {
        
        return pos.x >= 0 && pos.y < MAX_ROW &&pos.y>=0&& pos.x < MAX_COL;
    }
    public int count;

    public bool ISGameOver()
    {
        for (int i = 18;i<MAX_ROW;i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {

                if (map[j, i] != null)
                {
                    gameNum++;
                    SaveDate();
                    return true;
                } 
            }
        }
        return false;
    }
   public  Vector2 Round(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        return new Vector2(x, y);

    }
    public bool FallMap(Transform t)
    {
        foreach (Transform I in  t)
        {
            if (I.tag=="block")
            {
                Vector2 pos = Round(I.position);
                map[(int)pos.x, (int)pos.y] = I;
              
            }
        }
      return   CheckMap();
    }
    public bool tFallMap(Transform t)
    {
        
            if (t.tag == "block")
            {
                Vector2 pos = Round(t.position);
                map[(int)pos.x, (int)pos.y] = t;

            }
        
        return CheckMap();
    }

    public bool CheckMap()
    {
        count = 0;
        for (int r=0;r <MAX_ROW;r++)
        {
            if (CheckOneRow(r))
            {
                count++;
                DeletRow(r);
                MoveMap(r+1);
                r--;
            }
        }
        if (count > 0)
        {
           
                score += count;
                if (score > hscore)
                {
                    hscore = score;
                }
            
           
            
            SaveDate();
            return true;
        } 
        else
            return false;
    }
    public bool CheckOneRow(int r)
    {
        for (int i =0;i<MAX_COL;i++)
        {
            if (map[i, r] == null) return false;
        }
        return true;
    }
    public void DeletRow(int r)
    {
        for (int i = 0; i < MAX_COL; i++)
        {
            Destroy(map[i, r].gameObject);
            map[i, r] = null;

        }
    }

    public void MoveMap(int r)
    {
        for (int i=r; i < MAX_ROW; i++)
        {
            MoveOneRow(i);
        }
    }

    public void MoveOneRow(int r)
    {
        for (int i = 0; i < MAX_COL; i++)
        {
            if (map[i, r] != null)
            {
                map[i, r - 1] = map[i, r];
                map[i, r] = null;
                map[i, r-1].position += new Vector3(0, -1, 0);

            }
        }
       
    }
    public void SaveDate()
    {
       
        PlayerPrefs.SetInt("Heightscore", hscore);
        PlayerPrefs.SetInt("GameNumcount", gameNum);

    }
    public void LaodDate()
    {
        hscore = PlayerPrefs.GetInt("Heightscore", hscore);
        gameNum = PlayerPrefs.GetInt("GameNumcount", gameNum);

    }

    public void ClearMap()
    {
        for (int i = 0; i < MAX_ROW; i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {
                if (map[j, i] != null)
                {
                    Destroy(map[j, i].gameObject);
                    map[j, i] = null;
                }
                
            }
        }
    }

    public void ClearData()
    {
        score = 0;
        hscore = 0;
        gameNum = 0;
        SaveDate();
    }

    public void ReStart()
    {
        ClearMap();
        
    }


   
}
