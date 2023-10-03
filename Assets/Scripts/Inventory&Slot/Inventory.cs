using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{

    public static bool inventoryActivated = false; //인벤토리 활성화 상태
    public static bool shopActivated = false; //상점 활성화상태



     

    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent; //그리드 세팅 (모든 슬롯관리)

    public ItemPickUp[] Equipments; //장비 on & off용 정보 받아오기

    public TextMeshProUGUI itemNameTxt;
    public TextMeshProUGUI itemDescTxt;
    //[SerializeField]
    //private PlayerController player;

    //private Animator anim;
    //슬롯
    private Slot[] slots;

    //시작 시 테스트용 아이템 추가
    public Item[] startItems;

    void Start()
    {
        //슬롯 배열내에 모든 실제 슬롯 입력
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        //anim = GetComponent<Animator>();

        AcquireItem(startItems[0]);
        AcquireItem(startItems[1]);
        AcquireItem(startItems[2]);
        AcquireItem(startItems[3]);
    }




    //★★★인풋 액션 inventory 여기다 연결하세요★★★
    public void TryOpenIventory()
    {
        inventoryActivated = !inventoryActivated; //누르면 활성화 인벤 비활성화 스위칭
        //anim.SetBool("Appear", inventoryActivated); //인벤 활성/비활성 애니 


        //인벤토리가 열린 경우는 상태변수로 여러 조작/흐름 을 막아놓음(메모)
        if (inventoryActivated)
        {
            OpenInventory();
        }


        else
        {
            CloseInventory();
        }
    }

    //인벤토리 활성화 && 비활성화
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    //아이템 획득
    public void AcquireItem(Item _item, int _count = 1)
    {
        //if (!PlayerController.canPickUp)
            //return;
        
            if (Item.ItemType.Equipment != _item.itemType) // 장비가 아닌경우에만 (장비는 개수x)
            {
                //if (_item.itemType != Item.ItemType.Ingredient) //아이템 타입이 "재료" 가 아닌경우만 무게 계산
                //player.IncreaseWT(_item.itemWeight);

            //동일 아이템이 있는 경우부터 조사 반복문 
            for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
                {
                    if (slots[i].item != null) // 슬롯 기본값 null이므로 null참조 방지
                    {
                        if (slots[i].item.itemName == _item.itemName) //&& PlayerController.canPickUp) //넘어온 아이템이 이미 인벤에 있다면
                        {
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                    }

                }

            }

            //동일 아이템이 없는 경우 && 장비인 경우
            for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
            {
                if (slots[i].item == null) // && PlayerController.canPickUp) //인벤에 동일 아이템이 없다면
                {
                    slots[i].AddItem(_item, _count); // 슬롯의 additem 함수 호출(새 아이템 추가)
                    return;
                }
            }
    }

    //아이템 제거
    public void DeAcquireItem(Item _item, int _count = -1)
    {
        //player.DecreaseWT(_item.itemWeight);
        //동일 아이템이 있는 경우부터 조사 반복문 
        for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
        {
            if (slots[i].item != null) // 슬롯 기본값 null이므로 null참조 방지
            {
                if (slots[i].item.itemName == _item.itemName) //넘어온 아이템이 이미 인벤에 있다면
                {
                    slots[i].SetSlotCount(_count);                  
                }
            }
        }
    }

    public Slot[] GetSlots()
    {
        return slots;
    }
}


    

