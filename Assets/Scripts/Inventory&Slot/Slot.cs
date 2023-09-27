using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector2 originPos; // 슬롯 원래위치

    public Item item; // 획득한 아이템
    public int itemCount; // 획득 아이템 개수
    public Image itemImage; // 아이템의 이미지

    //롱클릭용 변수
    private float clickTime; // 클릭 중인 시간
    [SerializeField]
    private float minClickTime = 1; // 최소 클릭시간
    private bool isClick; // 클릭 중인지 판단 

    /*
    //쿨타임 계산용 변수 (퀵슬롯 상속)
    [SerializeField]
    protected float coolTime;
    protected float currentCoolTime;
    protected bool isCooltTime;
    */

    //퀵슬롯용 변수
    [SerializeField]
    private Image[] img_CoolTime; //쿨타임 이미지

    [SerializeField]
    private Inventory inventory;



    //필요한 컴포넌트
    private ItemEffectDatabase theItemEffectDatabase;

    //슬롯 아이템 개수
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;



    //인벤토리와 퀵슬롯
    [SerializeField]
    private RectTransform baseRect; // 인벤토리 UI의 범위
    [SerializeField]
    private RectTransform quickSlotBaseRect; // 퀵슬롯 UI 의 범위

    //private PlayerController PlayerPos; // 드래그 종료 시 드랍 될 위치용
    [SerializeField]
    private InputNumber theInputNumber; // 아이템 판매 시 나타날 UI 연동
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        originPos = transform.position; // 슬롯 원래 위치 저장
        //PlayerPos = FindObjectOfType<PlayerController>();   
    }

    void Update()
    {
        //롱클릭 업데이트
        if (isClick) //클릭중
        {
            // 클릭시간 측정
            clickTime += Time.deltaTime;
        }
        // 클릭 중이 아니라면
        else
        {
            // 클릭시간 초기화
            clickTime = 0;
        }

        /*
        if (isCooltTime && !Inventory.inventoryActivated) // && !EventContoroller.randomEvent)
            CoolTimeCalc(); // 쿨타임 계산
        */
    }




    //슬롯이 빈 상태일 때 아이템 이미지 객체 투명화
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha; // Color의 알파값
        itemImage.color = color; //파라미터 1 보임 0 안보임
    }

    //아이템 획득
    public virtual void AddItem(Item _item, int _count = 1)
    {
        item = _item; // 정보
        itemCount = _count; // 개수
        itemImage.sprite = item.itemImage; //아이템 이미지

        if (item.itemType != Item.ItemType.Equipment) //아이템 타입이 "장비" 가 아닌경우만 개수표기
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else //장비 아이템
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);   
        }
        SetColor(1);            
    }

    //인벤토리 내 아이템 개수 조절
    public void SetSlotCount(int _count)
    {       
        itemCount += _count; //개수를 더하거나 깎을 수 있음
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0) // 아이템이 없어진경우
            ClearSlot();
    }

    //슬롯 비우기(초기화)
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    //슬롯 클릭 & 아이템 사용★
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
                if (item != null)
                {
                    if (item.itemType == Item.ItemType.Used) // && !isCooltTime)
                    {
                        //PlayerPos.DecreaseWT(item.itemWeight); //소모품 사용시 무게감소
                                                                   //쿨타임 적용
                        //currentCoolTime = coolTime;
                        //isCooltTime = true;

                        //아이템 획득 가능 여부
                        //PlayerController.canPickUp = true;

                        //theItemEffectDatabase.UseItem(item);
                        SetSlotCount(-1);
                    }
                }           
        }
    }

    //슬롯 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this; // 드래그 슬롯이 슬롯이 됨
            DragSlot.instance.DragSetImage(itemImage); // 드래그 중인 이미지도 넣어줌
            DragSlot.instance.slotRect.anchoredPosition = eventData.position;
        }
    }

    //슬롯 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
        //theItemEffectDatabase.HideToolTip();
    }

    //슬롯 드래그 종료
    public void OnEndDrag(PointerEventData eventData) //다른곳이나 자기 자신에서 드래그 끝난경우 오출
    {
        //theItemEffectDatabase.HideToolTip();
        //!(안쪽 내용true인 경우) == false 됨 
        // 드래그가 끝난곳이 인벤토리or퀵슬롯 영역이 아니라면

        //Debug.Log($"앵커 x : {DragSlot.instance.slotRect.anchoredPosition.x}");
        //Debug.Log($"앵커 y : {DragSlot.instance.slotRect.anchoredPosition.y}");
        //Debug.Log($"베이스 xMin : {baseRect.rect.xMin}");
        //Debug.Log($"베이스 xMax : {baseRect.rect.xMax}");
        //Debug.Log($"베이스 yMin : {baseRect.rect.yMin}");
        //Debug.Log($"베이스 yMax : {baseRect.rect.yMax}");

        //인벤토리 영역
        if (!((DragSlot.instance.slotRect.anchoredPosition.x > baseRect.rect.xMin
            && DragSlot.instance.slotRect.anchoredPosition.x < baseRect.rect.xMax
            && DragSlot.instance.slotRect.anchoredPosition.y > baseRect.rect.yMin + 130f
            && DragSlot.instance.slotRect.anchoredPosition.y < baseRect.rect.yMax + 130f)
            ||
            //퀵슬롯 영역
            (DragSlot.instance.slotRect.anchoredPosition.x > quickSlotBaseRect.rect.xMin
            && DragSlot.instance.slotRect.anchoredPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.slotRect.anchoredPosition.y > quickSlotBaseRect.rect.yMin - 400f
            && DragSlot.instance.slotRect.anchoredPosition.y < quickSlotBaseRect.rect.yMax - 400f)))
            
        {
            //버리기 함수 실행
            //아이템 판매 코드 혹은 함수 추가 //아래 코드 추후 수정######
            if (DragSlot.instance.dragSlot != null) // 드래그슬롯에 아이템이 있는경우만 실행
            {
                Debug.Log("영역 밖 드롭-드래그 슬롯 위치 : " + DragSlot.instance.slotRect.anchoredPosition);
                //theInputNumber.Call();
            }
        }
        else //인벤토리 영역 내부인 경우 == 드래그 슬롯값만 비워줌
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
      

    }

    public void OnDrop(PointerEventData eventData) // 다른 슬롯에서 드래그가 끝난경우만 호출
    {
        if (DragSlot.instance.dragSlot != null) //빈슬롯 간의 ChangeSlot 호출 방지
        {
            //theItemEffectDatabase.HideToolTip();
            ChangeSlot();
        }

    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount; //교체당하는 슬롯의 아이템정보 미리 복사

        //교체당하는 슬롯에 드래그 슬롯 복사체의 정보 입력(아이템/개수)
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //교체 당하는 슬롯에 아이템이 있다면
        if(_tempItem != null)
        {
            //드래그 슬롯에 교체당하는 아이템 복사 정보(_tempItem,_tempItemCount) 입력
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else //교체 당하는 슬롯이 비었다면
        {
            //교체하는 슬롯 초기화
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }



    //슬롯 롱 클릭 (툴팁 표기) == 컴포넌트)이벤트 시스템

    // 버튼 일반 클릭
    public void ButtonClick()
    {
        print("버튼 일반 클릭");
    }


    // 버튼 클릭이 시작했을 때
    public void ButtonDown()
    {
        isClick = true;
    }

    // 버튼 클릭이 끝났을 때
    public void ButtonUp()
    {
        isClick = false;

        // 클릭 중인 시간이 최소 클릭시간 이상이라면
        if (clickTime >= minClickTime && item != null)
        {
            //툴팁 호출
            //theItemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }

    /*
    //퀵슬롯 쿨타임용
    private void CoolTimeCalc()
    {
        Debug.Log("작동됨");
        currentCoolTime -= Time.deltaTime;
        for (int i = 0; i < img_CoolTime.Length; i++)
        {
            img_CoolTime[i].fillAmount = currentCoolTime / coolTime; // 쿨타임 수치 게이지 반영
        }
        if (currentCoolTime <= 0)
            isCooltTime = false;
    }
    */
}
