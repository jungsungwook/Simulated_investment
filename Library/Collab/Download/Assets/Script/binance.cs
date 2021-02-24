using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class binance : MonoBehaviour
{
    public string endpoint = "https://api.binance.com";
    public string APIKEY = "ZtKLZL0kgCSudwEpHGz4w8D911JbhjD5NeADQBCyUev1RXOkydOvrpWPq7MYydB6";
    private string SECRETKEY = "CMn7PMmYpzdoewZVfWWihyKLrSSg0YFFXyU2B1qVU2NA6riCQlnftpBawbWJOQiu";
    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;

    }
    public string getAPIKEY()
    {
        return APIKEY;
    }
    public string getSECRETKEY()
    {
        return SECRETKEY;
    }
    IEnumerator connectedWeb()
    {
        while (true)
        {
            GET(endpoint + "/api/v3/time");
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors 
        if (www.error == null)
        {
            ProcessPlayer(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);

        }
    }
    private void ProcessPlayer(string jsonString)
    {
        try
        {
            JsonData jsonPlayer = JsonMapper.ToObject(jsonString);
            string isOk = jsonPlayer["serverTime"].ToString();
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("connectedWeb");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
