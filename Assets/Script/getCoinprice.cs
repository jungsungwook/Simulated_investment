using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
public class getCoinprice : MonoBehaviour
{

    public string url = "https://api.bithumb.com/public/ticker/";
    public string coin_value = null;
    public int last_price;
    public Text variation;
    public Image arrow;
    public int count = 0;
    public WWW GET(string url)
    {
        WWW www = new WWW(url + coin_value);
        StartCoroutine(WaitForRequest(www));
        return www;

    }
    // Use this for initialization
    void Start()
    {
        coin_value = this.gameObject.name;
        variation = GameObject.Find("Canvas").transform.Find(coin_value).transform.Find("Variation").GetComponent<Text>();
        arrow = GameObject.Find("Canvas").transform.Find(coin_value).transform.Find("arrow").GetComponent<Image>();
        StartCoroutine("Get_price");

    }
    IEnumerator Get_price()
    {
        while (true)
        {
            GET(url);
            count++;
            yield return new WaitForSecondsRealtime(2.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors 
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            ProcessPlayer(www.text);
        }
        else
        {
            this.GetComponent<Text>().text = "인터넷이 연결되지 않았습니다.";
            Debug.Log("WWW Error: " + www.error);
        }
    }

    private void ProcessPlayer(string jsonString)
    {

        JsonData jsonPlayer = JsonMapper.ToObject(jsonString);
        string isOk = jsonPlayer["status"].ToString();

        if (isOk.Equals("0000"))
        {
            int price = int.Parse(jsonPlayer["data"]["closing_price"].ToString());
            this.GetComponent<Text>().text = string.Format("{0:#,###}", price) + "원";
            print(string.Format("{0:#,###}", price));
            if (price > last_price)
            {
                last_price = price;
                arrow.sprite = Resources.Load<Sprite>("green_arrow");
                count = 0;
                int var = price - last_price;
                variation.text = "<color=#00B050FF>" + "+" + System.Convert.ToString(var) + "</color>";
            }
            else if (price == last_price && count >= 2)
            {
                arrow.sprite = Resources.Load<Sprite>("bar");
                variation.text = "<color=#323232FF>" + "-" + "</color>";
                
            }
            else if (price < last_price)
            {
                last_price = price;
                arrow.sprite = Resources.Load<Sprite>("red_arrow");
                count = 0;
                int var = last_price - price;
                variation.text = "<color=#ff0000>" + "-" + System.Convert.ToString(var) + "</color>";
            }
        }
        else
        {
            this.GetComponent<Text>().text = "-";
            return;
        }
    }
}
