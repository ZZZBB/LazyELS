using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Menu;
        AddTransition(Transition.OnButtonStart,StateID.Play);
    }

    public override void DoBeforeEntering()
    {
        cnt.view.ShowMenu();
       
    }
    public override void DoBeforeLeaving()
    {
        cnt.view.HideMenu();
       
    }
    public void OnButtonStart()
    {
        cnt.audioManager.PlayClip();
       
       
        fsm.PerformTransition(Transition.OnButtonStart);
    }

    public void OnButtonReStart()
    {
        cnt.audioManager.PlayClip();
        cnt.model.ClearMap();
        cnt.model.score = 0;
        cnt.GM.isAI = false;
        if (cnt.GM.downShape.gameObject!=null)
        {
            Destroy(cnt.GM.downShape.gameObject);
        }
        
        fsm.PerformTransition(Transition.OnButtonStart);

    }


}
