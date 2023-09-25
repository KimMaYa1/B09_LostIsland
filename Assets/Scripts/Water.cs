using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Water : MonoBehaviour
{
    public static bool isWater=false;
    [SerializeField] private float waterDrag = 15;//물속 중령
    private float originDrag;//플레이어 리지드 바디 값 = 0 (떨어지지않음)

    //[SerializeField] private Color waterColor; // 물속 색
    //[SerializeField] private float waterFogDensity = 0.17f; // 물의 탁한 정도

    //private Color originColor;
    //private float originFogDensity;
    //[SerializeField] private Color originNightColor;
    //[SerializeField] private float originNightFogDensity;

    [SerializeField] private GameObject waterFilter;

    private void Start()
    {
        //originColor = RenderSettings.fogColor;
        //originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player") 
        {
            GetWater(other);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }
    private void GetWater(Collider Player)
    {
        isWater = true;
        Player.transform.GetComponent<Rigidbody>().drag = waterDrag; // 저항이 높아짐 =천천히 가라앉음
        //RenderSettings.fogColor = waterColor;
        //RenderSettings.fogDensity = waterFogDensity;
        waterFilter.SetActive(true);
    }


    private void GetOutWater(Collider Player)
    {
        if (isWater) 
        {
            isWater = false;
            Player.transform.GetComponent<Rigidbody>().drag = originDrag;

            //RenderSettings.fogColor = originColor;
            //RenderSettings.fogDensity = originFogDensity;
            waterFilter.SetActive(false);
        }
    }
}
