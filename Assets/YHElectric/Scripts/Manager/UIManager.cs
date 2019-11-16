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
        transform.Find("bg/topBar/water").GetComponent<Button>().onClick.AddListener(() => BtnClick("water"));
        transform.Find("bg/monitoring/transformer").GetComponent<Button>().onClick.AddListener(() => BtnClick("transformer"));
        transform.Find("bg/function/energyCloud").GetComponent<Button>().onClick.AddListener(() => BtnClick("energyCloud"));
        transform.Find("bg/cameraMove/cameraMoveUp").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveUp"));
        transform.Find("bg/cameraMove/cameraMoveForward").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveForward"));
        transform.Find("bg/cameraMove/cameraMoveDown").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveDown"));
        transform.Find("bg/cameraMove/cameraMoveBack").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraMoveBack"));
        //transform.Find("bg/cameraRot/cameraRotUp").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraRotUp"));
        //transform.Find("bg/cameraRot/cameraRotDown").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraRotDown"));
        //transform.Find("bg/cameraRot/cameraRotLeft").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraRotLeft"));
        //transform.Find("bg/cameraRot/cameraRotRight").GetComponent<Button>().onClick.AddListener(() => BtnClick("cameraRotRight&123.3"));
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
