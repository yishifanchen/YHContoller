using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class UDPControl : MonoBehaviour
{
    
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }

    public static UDPControl instance;
    private int port=10001;
    public LGZN.Net.UDPClient uDPClient;

    
    private void Awake()
    {
        instance = this;
        uDPClient = new LGZN.Net.UDPClient(port, LGZN.Net.UDPMode.SendAndReceive);
    }
    private void Start()
    {
        uDPClient.Receive(OnReceive);
    }
    private void OnReceive(string str)
    {
        //print(str);
    }
    int mousex, mousey;
    bool isClick = false;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GetCursorPos(ref mousex, ref mousey);
        //    mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
        //    mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        //}
       
    }
    [DllImport("user32")]
    public static extern int SetCursorPos(int x, int y);
    [DllImport("user32")]
    private static extern bool GetCursorPos(ref int x, ref int y);
    [DllImport("user32")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    private void OnDestroy()
    {
        uDPClient.Close();
    }
    private void OnApplicationQuit()
    {
        uDPClient.Close();
    }
}
