using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public GameObject rain;
    public float fullDayLength= 300;
    //별중요한건아닌데 풀데이렝스를 낮밤 시간으로 맞출것
    private float shortTime;
    private float middleTime;
    private float longTime;
    private float currentTime=100;
    private float nextCheckTime;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(RainCheck());
        shortTime = (fullDayLength/4)*1;
        middleTime = (fullDayLength / 4) * 2;
        longTime = (fullDayLength / 4) * 3;
    }
    IEnumerator RainCheck()
    {
        while (true)
        {
            int a = Random.Range(0, 100);
            if (a > 75)
            {
                currentTime = longTime;
            }
            else if (a > 50)
            {
                currentTime = middleTime;
            }
            else
            {
                currentTime = shortTime;
            }
            nextCheckTime = fullDayLength/2 + a/2;
            if (a<=10)
            {

                rain.SetActive(true);
                Invoke("RainOff", currentTime);

            }
            yield return new WaitForSeconds(nextCheckTime);
        }
    }
    private void RainOff()
    {
        rain.SetActive(false);
    }
}
