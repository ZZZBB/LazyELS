using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySate : FSMState
{

    private void Awake()
    {
        stateID = StateID.Play;
        AddTransition(Transition.OnButtonPause,StateID.Menu);
    }
    public override void DoBeforeEntering()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        cnt.view.ShowGameUI(cnt.model.Score,cnt.model.Hscore,cnt.model.AIScore);
        cnt.GM.StartGame();
    }
    public override void DoBeforeLeaving()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;  //关闭手机屏幕常亮(按照手机设置)

        cnt.view.HideGameUI();
        cnt.view.ShowReStartButton();
        cnt.GM.PauseGame();
    }
    public void OnButtonPause()
    {
        cnt.audioManager.PlayClip();
        
        fsm.PerformTransition(Transition.OnButtonPause);
    }

    public void OnButtonRestar()
    {
        cnt.model.score = 0;
        cnt.view.UpdateGameUI(0,cnt.model.Hscore,cnt.model.AIScore);
        cnt.view.HideGamwOverUI();
        cnt.view.ShowInputUI();
        cnt.model.ClearMap();
        cnt.GM.isAI = false;
        cnt.GM.StartGame();
        
    }
}
