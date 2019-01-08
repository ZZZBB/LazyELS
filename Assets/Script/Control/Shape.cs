using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {

    public bool isStopDown = false;
    private float time = 0;
    private float stepTime = 0.6f;
    //左右计时器
    private float h_Time = 0;
    private float h_StepTime = 0.6f;
    //变换计时
    private float r_Time = 0;
    private float r_StepTime = 0.6f;
  
    public Control CNT;
    public GameManager GM;
    public Transform P;
    public int space = 45;
    public bool isInFast = false;
    public Vector3 tagPos;
    public Vector3 tagEuler;
    public float h;
    public bool isPlayer = true;
    public bool isButtonDown = false;
    public AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        P = this.transform.Find("p");
    }
    private void Update()
    {
        if (isStopDown) return;
        time += Time.deltaTime;
        h_Time += Time.deltaTime;
        r_Time += Time.deltaTime;
        if (time>=stepTime)
        {
            time = 0;
            Down();
        }

       
    }

    public void Inti(Color color,Control cnt,GameManager gm)
    {
        foreach (Transform  t in this.transform)
        {
            if (t.tag=="block")
            {
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }
        CNT = cnt;
        GM = gm;
    }
    private void Down()
    {
        Vector3 pos = this.transform.position;
        pos.y -= 1;
        this.transform.position = pos;
        
        if(!CNT.model.isCanMove(this.transform))
        {
            pos.y += 1;
            this.transform.position = pos;
            isStopDown = true;
            GM.downShape = null;
          
            bool isLan =   CNT.model.FallMap(this.transform);
            if (isInFast)
            {
               
                CNT.audioManager.PlayAudio(audioSource, 3);
            }
           
            if (isLan)
            {
                
                CNT.view.UpdateGameUI(CNT.model.Score,CNT.model.Hscore,CNT.model.AIScore);
                CNT.audioManager.PlayIsLan();
                
            }
            if (CNT.model.ISGameOver())
            {
               

                CNT.GM.isPause = true;
                CNT.audioManager.PlayOver();
                CNT.view.HideInputUI();
                CNT.view.ShowGamwOverUI(CNT.model.Score);
            }
           
            return;
        }
        CNT.audioManager.PlayAudio(audioSource, 1);
    }

    public bool InputControl(int i)
    {
        
        
        
        h = i;
       
            Vector3 pos = this.transform.position;
            pos.x += h;
            this.transform.position = pos;
            if (!CNT.model.isCanMove(this.transform))
            {
                pos.x -= h;
                this.transform.position = pos; return false;
            }
            else
            {
                 CNT.audioManager.PlayAudio(audioSource, 4);
                 return true;
              

            }
        
    }


    public void DownFast()
    {
        stepTime /= space;
        isInFast = true;
    }
    public void IsStop()
    {
        isStopDown = true;
    }
    public void ISGo()
    {
        isStopDown = false;
    }

    public bool Rote()
    {
        this.transform.RotateAround(P.position, Vector3.forward, -90);
        if (!CNT.model.isCanMove(this.transform))
        {
            this.transform.RotateAround(P.position, Vector3.forward, 90);
            return false;
        }
        else
        {
            CNT.audioManager.PlayAudio(audioSource, 5);
            return true;
            
        }
       
    }


    public void GoTager(Vector3 pos,Vector3 eul)
    {

       // Debug.Log("Go"+"tr.pos:"+ pos + "e"+ eul);
        tagEuler = eul;
        tagPos = pos;
       // Debug.Log(pos);
        StartCoroutine("GoRote");
      
    }
    private IEnumerator GoLeftOrRight()
    {
        int x = (int) (tagPos.x - transform.position.x);
        int h = 0;

        while (true)
        {
            if (isPlayer) break;

            if ((int)tagPos.x == (int)transform.position.x) { break; }
            
           
            if (x > 0)
            {
                h = 1;
            }
            else if (x < 0)
            {
                h = -1;
            }
            Vector3 pos = this.transform.position;
            pos.x += h;
            this.transform.position = pos;
            if (!CNT.model.isCanMove(this.transform))
            {
                pos.x -= h;
                this.transform.position = pos;
                break;
            }
            yield return new WaitForSeconds(0.1f);
           
        }

        if (!isPlayer)
        {
            stepTime /= 55;
            isInFast = true;

        }
       
    }

    private IEnumerator GoRote()
    {
        
        while (true)
        {
            if (isPlayer) break;
            if (this.transform.eulerAngles == tagEuler)
            {//旋转到目标的角度
                break;
            }
            else
            {
                Rote();

            }
            yield return new WaitForSeconds(0.2f);
           
            

        }
        if (!isPlayer)
        {
            StartCoroutine("GoLeftOrRight");
        }
       
    }

   
}
