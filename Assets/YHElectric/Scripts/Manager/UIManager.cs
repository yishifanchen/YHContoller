﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private float timer = 0;
    private string targetIP;
    void Awake()
    {
        instance = this;
        LoadSetting();
    }
    private void Start()
    {
        //天气
        for (int i = 0; i < transform.Find("bg/weather").childCount; i++)
        {
            int index = i;
            transform.Find("bg/weather").GetChild(i).GetComponent<Button>().onClick.AddListener(() => BtnClick("SetWeather&"+index.ToString()));
        }
        //时间
        transform.Find("bg/timeSlider").GetComponent<Slider>().onValueChanged.AddListener(BtnClick);
        //预测
        transform.Find("bg/forecasting").GetComponent<Button>().onClick.AddListener(() => BtnClick("tip" + "&" + "forecasting"));
        //公告
        transform.Find("bg/notice").GetComponent<Button>().onClick.AddListener(() => BtnClick("tip" + "&" + "notice"));
        //告警
        transform.Find("bg/alarm").GetComponent<Button>().onClick.AddListener(() => BtnClick("tip" + "&" + "alarm"));
        //顶部展板   displayBoard
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_water").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard"+"&"+"D_water"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_electric").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&"+"D_electric"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_gas").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&" + "D_gas"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_coal").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&" + "D_coal"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_energy").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&" + "D_energy"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_photovoltaic").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&" + "D_photovoltaic"));
        transform.Find("bg/topBar/bg/Scroll View/Viewport/Content/D_chargingPile").GetComponent<Button>().onClick.AddListener(() => BtnClick("displayBoard" + "&" + "D_chargingPile"));
        //实时监测  monitor
        transform.Find("bg/monitoring/bg/Scroll View/Viewport/Content/M_transformer").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring"+"&"+ "M_transformer"));
        transform.Find("bg/monitoring/bg/Scroll View/Viewport/Content/M_photovoltaic").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring" + "&" + "M_photovoltaic"));
        transform.Find("bg/monitoring/bg/Scroll View/Viewport/Content/M_workshop").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring" + "&" + "M_workshop"));
        //平台功能   function
        transform.Find("bg/function/bg/Scroll View/Viewport/Content/F_energyCloud").GetComponent<Button>().onClick.AddListener(() => BtnClick("function" + "&" + "F_energyCloud"));
        transform.Find("bg/function/bg/Scroll View/Viewport/Content/F_workshop").GetComponent<Button>().onClick.AddListener(() => BtnClick("function" + "&" + "F_workshop"));

        //策略    strategy
        transform.Find("bg/strategy/bg/Scroll View/Viewport/Content/S_transformer").GetComponent<Button>().onClick.AddListener(() => BtnClick("strategy" + "&" + "S_transformer"));
        //镜头操控
        transform.Find("bg/roam").GetComponent<Button>().onClick.AddListener(() => { isRoam = !isRoam; BtnClick("roam", isRoam); });
        
        //关闭
        transform.Find("bg/closeBtn").GetComponent<Button>().onClick.AddListener(() => BtnClick("closeBtn"));
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    void LoadSetting()
    {
        string text = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/UrlSetting.json");
        JsonData jd = JsonMapper.ToObject(text);
        targetIP = jd["URL"].ToString();
    }
    private void BtnClick(string str)
    {
        print(str);
        UDPControl.instance.uDPClient.Send(str, targetIP.ToString());
    }
    bool isRoam=false;
    private void BtnClick(string str,bool isBool)
    {
        print(str + "&" + isBool);
        UDPControl.instance.uDPClient.Send(str+"&"+ isBool, targetIP.ToString());
    }
    private void BtnClick(float value)
    {
        print(value);
        UDPControl.instance.uDPClient.Send("timeSlider&"+value.ToString(), targetIP.ToString());
    }
    public void OnCamRot(Vector2 vector2)
    {
        if (timer < 0.3f) return;
        UDPControl.instance.uDPClient.Send("OnCamRotX&" + vector2.x, targetIP.ToString());
        UDPControl.instance.uDPClient.Send("OnCamRotY&" + vector2.y, targetIP.ToString());
        timer = 0;
    }
    public void OnCamRotEnd()
    {
        UDPControl.instance.uDPClient.Send("OnCamRotX&" + 0, targetIP.ToString());
        UDPControl.instance.uDPClient.Send("OnCamRotY&" + 0, targetIP.ToString());
    }
    public void OnCamMove(Vector2 vector2)
    {
        if (timer < 0.3f) return;
        UDPControl.instance.uDPClient.Send("OnCamMoveX&" + vector2.x, targetIP.ToString());
        UDPControl.instance.uDPClient.Send("OnCamMoveY&" + vector2.y, targetIP.ToString());
        timer = 0;
    }
    public void OnCamMoveEnd()
    {
        UDPControl.instance.uDPClient.Send("OnCamMoveX&" + 0, targetIP.ToString());
        UDPControl.instance.uDPClient.Send("OnCamMoveY&" + 0, targetIP.ToString());
    }
}
