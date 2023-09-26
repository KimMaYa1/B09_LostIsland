using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Water : MonoBehaviour
{
    public static bool isWater=false;
    [SerializeField] private float waterDrag = 15;//���� �߷�
    private float originDrag;//�÷��̾� ������ �ٵ� �� = 0 (������������)

    //[SerializeField] private Color waterColor; // ���� ��
    //[SerializeField] private float waterFogDensity = 0.17f; // ���� Ź�� ����

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
        Player.transform.GetComponent<Rigidbody>().drag = waterDrag; // ������ ������ =õõ�� �������
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
