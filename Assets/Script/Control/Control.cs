using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {
    [HideInInspector]
    public Model model;
    [HideInInspector]
    public View view;
    public FSMSystem fSM;
    public CameraManager mainCamrea;
    public GameManager GM;
    public AudioManager audioManager;
    private void Awake()
    {

        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
        GM = this.GetComponent<GameManager>();
        mainCamrea = GetComponent<CameraManager>();
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        MakeFSM();
    }
    void MakeFSM()//构造状态机
    {
        fSM = new FSMSystem();
        FSMState[] states = GetComponentsInChildren<FSMState>();
        //获取四种状态加载到状态机中
        foreach (FSMState state in states )
        {
            fSM.AddState(state,this);
        }
        //设置默认状态
        MenuState m = GetComponentInChildren<MenuState>();
        fSM.SetCurrentState(m);
    }
}
