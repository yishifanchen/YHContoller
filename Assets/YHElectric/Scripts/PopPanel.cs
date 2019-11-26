using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopPanel : MonoBehaviour
{
    public static PopPanel instance;
    public Transform popPanelBtnParent;
    private void Awake()
    {
        instance = this;
        Close();
    }
    void Start()
    {
        transform.Find("closeBtn").GetComponent<Button>().onClick.AddListener(Close);
    }
    void Close()
    {
        for(int i=0;i< popPanelBtnParent.childCount; i++)
        {
            Destroy(popPanelBtnParent.GetChild(i).gameObject);
        }
        transform.gameObject.SetActive(false);
    }
}
