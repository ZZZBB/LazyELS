using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : FSMState
{

    private void Awake()
    {
        stateID = StateID.Pause;
    }
}
