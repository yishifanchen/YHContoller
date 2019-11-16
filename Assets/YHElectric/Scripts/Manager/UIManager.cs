using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private float timer = 0;
    void Awake()
    {
        instance = this;
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
        //顶部展板
        transform.Find("bg/topBar/water").GetComponent<Button>().onClick.AddListener(() => BtnClick("water"));
        //实时监测
        transform.Find("bg/monitoring/transformer").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring"+"&"+ "transformer"));
        transform.Find("bg/monitoring/photovoltaic").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring" + "&" + "photovoltaic"));
        transform.Find("bg/monitoring/electricityRoom").GetComponent<Button>().onClick.AddListener(() => BtnClick("monitoring" + "&" + "electricityRoom"));
        //平台功能
        transform.Find("bg/function/energyCloud").GetComponent<Button>().onClick.AddListener(() => BtnClick("energyCloud"));
        //镜头操控
        transform.Find("bg/roam").GetComponent<Button>().onClick.AddListener(() => { isRoam = !isRoam; BtnClick("roam", isRoam); });
        transform.Find("bg/cameraMove/cameraMoveUp").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveUp"));
        transform.Find("bg/cameraMove/cameraMoveForward").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveForward"));
        transform.Find("bg/cameraMove/cameraMoveDown").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveDown"));
        transform.Find("bg/cameraMove/cameraMoveBack").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveBack"));
        //关闭
        transform.Find("bg/closeBtn").GetComponent<Button>().onClick.AddListener(() => BtnClick("closeBtn"));
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void BtnClick(string str)
    {
        print(str);
        UDPControl.instance.uDPClient.Send(str);
    }
    bool isRoam=false;
    private void BtnClick(string str,bool isBool)
    {
        print(str + "&" + isBool);
        UDPControl.instance.uDPClient.Send(str+"&"+ isBool);
    }
    private void BtnClick(float value)
    {
        print(value);
        UDPControl.instance.uDPClient.Send("timeSlider&"+value.ToString());
    }
    public void OnCamRot(Vector2 vector2)
    {
        if (timer < 0.2f) return;
        UDPControl.instance.uDPClient.Send("OnCamRotX&" + vector2.x);
        UDPControl.instance.uDPClient.Send("OnCamRotY&" + vector2.y);
        timer = 0;
    }
    public void OnCamRotEnd()
    {
        UDPControl.instance.uDPClient.Send("OnCamRotX&" + 0);
        UDPControl.instance.uDPClient.Send("OnCamRotY&" + 0);
    }
}
