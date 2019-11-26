using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public class H5Info
    {
        public float x;
        public float y;
        public int width;
        public int height;
        public float pivotX;
        public float pivotY;
        public string url;
    }
    public static Dictionary<string, H5Info> h5InfoDict = new Dictionary<string, H5Info>();
    public Transform[] panelType;
    private Transform[] panelTypeBtnParent;
    public GameObject btnPrefab;
    void Start()
    {
        panelTypeBtnParent = new Transform[4];
        for (int i=0;i< panelType.Length;i++)
        {
            panelTypeBtnParent[i] = panelType[i].Find("bg/Scroll View/Viewport/Content");
        }
        LoadSetting();
    }
    /// <summary>
    /// 加载h5配置文件
    /// </summary>
    void LoadSetting()
    {
        try
        {
            string text = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/Setting.json");
            JsonData jd = JsonMapper.ToObject(text);
            var type = jd["Setting"][0];
            IDictionary typeDict = type as IDictionary;
            List<string> typeDictKeys = new List<string>();
            foreach (string key in typeDict.Keys)
            {
                typeDictKeys.Add(key);
            }
            for (int i = 0; i < jd["Setting"][0].Count; i++)
            {
                var data = jd["Setting"][0][i][0];
                IDictionary dictionary = data as IDictionary;
                List<string> keys = new List<string>();
                foreach (string key in dictionary.Keys)
                {
                    keys.Add(key);
                }
                H5Info[] h5Info = new H5Info[data.Count];
                for (int j = 0; j < data.Count; j++)
                {
                    if(i<4)
                    {
                        GameObject go = Instantiate(btnPrefab);
                        go.transform.SetParent(panelTypeBtnParent[i]);
                        go.name = keys[j];
                        go.transform.GetChild(0).GetComponent<Text>().text = data[j]["name"].ToString();
                        if (data[j]["method"].Count>0)
                        {
                            FunctionInfo fi = go.AddComponent<FunctionInfo>();
                            for (int m=0;m< data[j]["method"].Count;m++)
                            {
                                fi.funcInfo.Add(data[j]["method"][m]["name"].ToString(), data[j]["method"][m]["value"].ToString());
                            }
                        }
                        string btnType = typeDictKeys[i];
                        go.GetComponent<Button>().onClick.AddListener(() => { UIManager.instance.BtnClick(btnType + "&" + go.name); ShowPanel(go); });
                    }
                    h5Info[j] = new H5Info
                    {
                        x = float.Parse(data[j]["x"].ToString()),
                        y = float.Parse(data[j]["y"].ToString()),
                        width = int.Parse(data[j]["width"].ToString()),
                        height = int.Parse(data[j]["height"].ToString()),
                        pivotX = float.Parse(data[j]["pivotX"].ToString()),
                        pivotY = float.Parse(data[j]["pivotY"].ToString()),
                        url = data[j]["url"].ToString()
                    };
                }
                for (int k = 0; k < keys.Count; k++)
                {
                    h5InfoDict.Add(keys[k], h5Info[k]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    void ShowPanel(GameObject go)
    {
        if (go.GetComponent<FunctionInfo>() != null)
        {
            FunctionInfo functionInfo = go.GetComponent<FunctionInfo>();
            PopPanel.instance.transform.gameObject.SetActive(true);
            foreach (string key in functionInfo.funcInfo.Keys)
            {
                GameObject g = Instantiate(btnPrefab);
                g.transform.GetChild(0).GetComponent<Text>().text = key;
                g.transform.SetParent(PopPanel.instance.popPanelBtnParent);
                g.GetComponent<Button>().onClick.AddListener(() => {
                    UIManager.instance.BtnClick("command" + "&" + functionInfo.funcInfo[key]);
                });
            }
        }
    }

}
