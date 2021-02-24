using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
public class coinone : MonoBehaviour
{
    public class Coin
    {
        public string name;
        public Text text;
        public Image arrow;
        public Text variation;
        public int count;
        public int lastprice;

    }
    public List<Coin> coinList = new List<Coin>();
    public string url3 = "https://api.coinone.co.kr/ticker/?currency=";
    public WWW GET(string url3)
    {
        WWW www = new WWW(url3);
        StartCoroutine(WaitForRequest(www));
        return www;

    }
    // Use this for initialization
    void Start()
    {

        coinList.Add(new Coin()
        {
            name = "btc",
            text = GameObject.Find("BTC_text").GetComponent<Text>(),
            arrow = GameObject.Find("BTC_arrow").GetComponent<Image>(),
            variation = GameObject.Find("BTC_variation").GetComponent<Text>(),
            count = 0,
            lastprice = 0


        });
        coinList.Add(new Coin()
        {
            name = "bch",
            text = GameObject.Find("BCH_text").GetComponent<Text>(),
            arrow = GameObject.Find("BCH_arrow").GetComponent<Image>(),
            variation = GameObject.Find("BCH_variation").GetComponent<Text>(),
            count = 0,
            lastprice = 0


        });
        coinList.Add(new Coin()
        {
            name = "eth",
            text = GameObject.Find("ETH_text").GetComponent<Text>(),
            arrow = GameObject.Find("ETH_arrow").GetComponent<Image>(),
            variation = GameObject.Find("ETH_variation").GetComponent<Text>(),
            count = 0,
            lastprice = 0


        });
        coinList.Add(new Coin()
        {
            name = "etc",
            text = GameObject.Find("ETC_text").GetComponent<Text>(),
            arrow = GameObject.Find("ETC_arrow").GetComponent<Image>(),
            variation = GameObject.Find("ETC_variation").GetComponent<Text>(),
            count = 0,
            lastprice = 0


        });
        coinList.Add(new Coin()
        {
            name = "iota",
            text = GameObject.Find("IOTA_text").GetComponent<Text>(),
            arrow = GameObject.Find("IOTA_arrow").GetComponent<Image>(),
            variation = GameObject.Find("IOTA_variation").GetComponent<Text>(),
            count = 0,
            lastprice = 0


        });

        

        StartCoroutine("Get_price");
    }
    IEnumerator Get_price()
    {
        while (true)
        {
            GET(url3);
            for(int i = 0; i < 5; i++)
            {
                coinList[i].count++;
            }
            yield return new WaitForSecondsRealtime(1.0f);
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
            ProcessPlayer(www.text);
        }
        else
        {
            for(int i = 0; i < 5; i++)
            {
                coinList[i].text.text = "인터넷이 연결되지 않았습니다.";
                coinList[i].arrow.sprite = Resources.Load<Sprite>("bar");
                coinList[i].variation.text = "<color=#323232FF>" + "-" + "</color>";
            }
            Debug.Log("WWW Error: " + www.error);

        }
    }

    private void ProcessPlayer(string jsonString)
    {
        JsonData jsonPlayer = JsonMapper.ToObject(jsonString);
        string isOk = jsonPlayer["result"].ToString();
        if (isOk.Equals("success"))
        {
            for (int i = 0; i < 5; i++)
            {
                int price = int.Parse(jsonPlayer[coinList[i].name]["last"].ToString());
                coinList[i].text.text = string.Format("{0:#,###}", price) + "원";
                if (price > coinList[i].lastprice)
                {
                    coinList[i].arrow.sprite = Resources.Load<Sprite>("green_arrow");
                    coinList[i].count = 0;
                    int var = price - coinList[i].lastprice;
                    coinList[i].variation.text = "<color=#00B050FF>" + "+" + System.Convert.ToString(var) + "</color>";
                    coinList[i].lastprice = price;
                }
                else if(price == coinList[i].lastprice && coinList[i].count >= 3)
                {
                    coinList[i].arrow.sprite = Resources.Load<Sprite>("bar");
                    coinList[i].variation.text = "<color=#323232FF>" + "-" + "</color>";
                }
                else if(price < coinList[i].lastprice)
                {
                    coinList[i].arrow.sprite = Resources.Load<Sprite>("red_arrow");
                    coinList[i].count = 0;
                    int var = coinList[i].lastprice - price;
                    coinList[i].variation.text = "<color=#ff0000>" + "-" + System.Convert.ToString(var) + "</color>";
                    coinList[i].lastprice = price;
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                coinList[i].text.text = "잘못된 접근입니다.";
                coinList[i].arrow.sprite = Resources.Load<Sprite>("bar");
                coinList[i].variation.text = "<color=#323232FF>" + "-" + "</color>";
            }
            return;
        }
    }
}
