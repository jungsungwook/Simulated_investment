using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class manager : MonoBehaviour {
    public Text time;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time.text = System.DateTime.Now.ToString("현재시간\nyyyy년 MM월 dd일 HH시 mm분 ss.fff초");
    }
}
