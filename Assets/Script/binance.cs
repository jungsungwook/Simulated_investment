using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

public class Coinfo
{

}
public class binance : MonoBehaviour
{
    public Socket transferSock;
    public InputField ipf;
    public void GetResponse()
    {
        print("Response");
        string _buf = ipf.text;
        Byte[] _data = new Byte[1024];
        _data = Encoding.Default.GetBytes(_buf);
        transferSock.Send(_data);
    }
    public void DisconnectBtn()
    {
        print("disconnect");
        state = false;
    }
    public void connectBtn()
    {
        print("connect");
        state = true;
    }
    public Boolean state = false;
    public Coroutine runningCoroutine=null;
    IEnumerator requestServer()
    {
        state = true;
        try
        {
            transferSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            transferSock.Connect(new IPEndPoint(IPAddress.Parse("211.238.68.75"), 9988));
        }
        catch(Exception e)
        {
            Debug.Log(e);
            state = false;
        }
        while (true)
        {
            if (!state)
            {
                runningCoroutine = null;
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        transferSock.Close();
    }
    // Update is called once per frame
    void Update()
    {
        if (state&&runningCoroutine==null)
        {
            runningCoroutine = StartCoroutine("requestServer");
        }
    }
    private void Start()
    {

    }
}
