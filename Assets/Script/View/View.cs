using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour {


    public Control cnt;

    public RectTransform logeName;
    public RectTransform menUI;
    public RectTransform GameUI;
    public RectTransform InputUI;
    public Transform gameOverUI;
    public GameObject reStartButtom;
    public Text Score;
    public Text HScore;
    public Text AIScore;
    public Text GameOverScore;
    public GameObject settingUI;
    public GameObject RankUI;
    public Transform noImage;
    public Transform onImage;
    public Text RScore;
    public Text RHScore;
    public Text RGameOverScore;
    public Button UpButton;

    public bool isLeft = false;
    public bool isRight = false;

    private void Awake()
    {
        cnt = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
        logeName = transform.Find("Canvas/LogeName") as RectTransform;
        menUI = transform.Find("Canvas/menUI") as RectTransform;
        GameUI = transform.Find("Canvas/GameUI") as RectTransform;
        InputUI = transform.Find("Canvas/InputUI") as RectTransform;
        reStartButtom = transform.Find("Canvas/menUI/Button_restar").gameObject;
        Score= transform.Find("Canvas/GameUI/ScoreLabel/Score").GetComponent<Text>();
        HScore = transform.Find("Canvas/GameUI/HightScoreLabel/Score").GetComponent<Text>();
       // AIScore = transform.Find("Canvas/GameUI/AIScoreLabel/Score").GetComponent<Text>();
        gameOverUI = transform.Find("Canvas/GameOver");
        GameOverScore = transform.Find("Canvas/GameOver/Image/Score").GetComponent<Text>();
        settingUI = transform.Find("Canvas/SettingUI").gameObject;
        RankUI = transform.Find("Canvas/RankUI").gameObject;

        noImage = transform.Find("Canvas/menUI/Button_Audio/mute");
        onImage = transform.Find("Canvas/menUI/Button_Audio/Nomute");
        RScore = transform.Find("Canvas/RankUI/Image/ScoreLab/Score").GetComponent<Text>();
        RHScore = transform.Find("Canvas/RankUI/Image/RHScoreLab/RHScore").GetComponent<Text>();
        RGameOverScore = transform.Find("Canvas/RankUI/Image/RGameOverScoreLab/RGameOverScore").GetComponent<Text>();


    }
    public void ShowMenu() {
       
        //logeName.gameObject.SetActive(true);
        //logeName.DOAnchorPosY(-99f,0.5f);
        menUI.gameObject.SetActive(true);
        menUI.DOAnchorPosX(0f, 0.5f);
    }
    public void HideMenu()
    {
        //logeName.DOAnchorPosY(99f, 0.5f).OnComplete(delegate { logeName.gameObject.SetActive(false); }); ;

        menUI.DOAnchorPosX(480f, 0.5f).OnComplete(delegate { menUI.gameObject.SetActive(false); }); 
       
       
    }

    public void UpdateGameUI(int score, int hScore,int AiScore)
    {
        this.Score.text = score.ToString();
        this.HScore.text = hScore.ToString();
        //this.AIScore.text = AiScore.ToString();
    }
    public void ShowGameUI(int score,int hScore,int aiScore)
    {
       // this.AIScore.text = aiScore.ToString();
        this.Score.text = score.ToString();
        this.HScore.text = hScore.ToString();
        GameUI.gameObject.SetActive(true);
        GameUI.DOAnchorPosY(-268f, 0.5f);
        ShowInputUI();
    }
    public void ShowInputUI()
    {
       
        InputUI.gameObject.SetActive(true);
        InputUI.DOAnchorPosX(239f, 0.5f);

    }
    public void HideInputUI()
    {

        
        InputUI.DOAnchorPosX(-239f, 0.5f).OnComplete(delegate { InputUI.gameObject.SetActive(false); });

    }
    public void ShowGamwOverUI(int score)
    {
        this.GameOverScore.text = score.ToString();
        gameOverUI.gameObject.SetActive(true);

    }
    public void HideGamwOverUI()
    {
       
        gameOverUI.gameObject.SetActive(false);

    }
    public void HideGameUI()
    {
        GameUI.DOAnchorPosY(-268f, 0.5f).OnComplete(delegate{ GameUI.gameObject.SetActive(false); });

        HideInputUI();


    }
    public void ShowReStartButton()
    {

        reStartButtom.SetActive(true);

    }

    public void OnButtonClickHome()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowSettingUI()
    {
        cnt.audioManager.PlayClip();
        settingUI.SetActive(true);
    }
    public void HideSettingUI()
    {
        cnt.audioManager.PlayClip();
        settingUI.SetActive(false);
    }

    public void OnAudioButton()
    {
        cnt.audioManager.OnAudioButton(noImage,onImage);
        
    }

    public void OnButtonClearData()
    {
        cnt.audioManager.PlayClip();

        cnt.model.ClearData();
        RScore.text = cnt.model.Score.ToString();
        RHScore.text = cnt.model.Hscore.ToString();
        RGameOverScore.text = cnt.model.GameNum.ToString();
    }

    public void ShowRankUI()
    {
        RScore.text = cnt.model.Score.ToString();
        RHScore.text = cnt.model.Hscore.ToString();
        RGameOverScore.text = cnt.model.GameNum.ToString();

        cnt.audioManager.PlayClip();
        RankUI.SetActive(true);
    }
    public void HideRankUI()
    {
        cnt.audioManager.PlayClip();
        RankUI.SetActive(false);
    }

    public void OnCilckUp()
    {
        cnt.GM.downShape.isPlayer = true;
        cnt.GM.isAI = false;

        cnt.GM.downShape.Rote();
    }
    public void OnCilckLeft()
    {
        // Debug.Log(1);
        cnt.GM.downShape.isPlayer = true;
        cnt.GM.isAI = false;

        isLeft = true;
    }
    public void OnCilckRight()
    {
        cnt.GM.downShape.isPlayer = true;
        cnt.GM.isAI = false;

        isRight = true;
       
    }
    public void OnCilckdown()
    {
        cnt.GM.downShape.isPlayer = true;
        cnt.GM.isAI = false;
        cnt.GM.downShape.DownFast();
    }

    public void OnClickAI()
    {
        if (cnt.GM.downShape.isPlayer)
        {
            cnt.GM.downShape.isPlayer = false;
            cnt.GM.isAI = true;
            cnt.GM.AI.ElsAI();
            
        }
       
    }

    public void OnButtonExit()
    {
        cnt.audioManager.PlayClip();
        Application.Quit();
    }

}
