using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector2 originPos; // ���� ������ġ

    public Item item; // ȹ���� ������
    public int itemCount; // ȹ�� ������ ����
    public Image itemImage; // �������� �̹���

    //��Ŭ���� ����
    private float clickTime; // Ŭ�� ���� �ð�
    [SerializeField]
    private float minClickTime = 1; // �ּ� Ŭ���ð�
    private bool isClick; // Ŭ�� ������ �Ǵ� 

    /*
    //��Ÿ�� ���� ���� (������ ���)
    [SerializeField]
    protected float coolTime;
    protected float currentCoolTime;
    protected bool isCooltTime;
    */

    //�����Կ� ����
    [SerializeField]
    private Image[] img_CoolTime; //��Ÿ�� �̹���

    [SerializeField]
    private Inventory inventory;



    //�ʿ��� ������Ʈ
    private ItemEffectDatabase theItemEffectDatabase;

    //���� ������ ����
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;



    //�κ��丮�� ������
    [SerializeField]
    private RectTransform baseRect; // �κ��丮 UI�� ����
    [SerializeField]
    private RectTransform quickSlotBaseRect; // ������ UI �� ����

    //private PlayerController PlayerPos; // �巡�� ���� �� ��� �� ��ġ��
    [SerializeField]
    private InputNumber theInputNumber; // ������ �Ǹ� �� ��Ÿ�� UI ����
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        originPos = transform.position; // ���� ���� ��ġ ����
        //PlayerPos = FindObjectOfType<PlayerController>();   
    }

    void Update()
    {
        //��Ŭ�� ������Ʈ
        if (isClick) //Ŭ����
        {
            // Ŭ���ð� ����
            clickTime += Time.deltaTime;
        }
        // Ŭ�� ���� �ƴ϶��
        else
        {
            // Ŭ���ð� �ʱ�ȭ
            clickTime = 0;
        }

        /*
        if (isCooltTime && !Inventory.inventoryActivated) // && !EventContoroller.randomEvent)
            CoolTimeCalc(); // ��Ÿ�� ���
        */
    }




    //������ �� ������ �� ������ �̹��� ��ü ����ȭ
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha; // Color�� ���İ�
        itemImage.color = color; //�Ķ���� 1 ���� 0 �Ⱥ���
    }

    //������ ȹ��
    public virtual void AddItem(Item _item, int _count = 1)
    {
        item = _item; // ����
        itemCount = _count; // ����
        itemImage.sprite = item.itemImage; //������ �̹���

        if (item.itemType != Item.ItemType.Equipment) //������ Ÿ���� "���" �� �ƴѰ�츸 ����ǥ��
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else //��� ������
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);   
        }
        SetColor(1);            
    }

    //�κ��丮 �� ������ ���� ����
    public void SetSlotCount(int _count)
    {       
        itemCount += _count; //������ ���ϰų� ���� �� ����
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0) // �������� ���������
            ClearSlot();
    }

    //���� ����(�ʱ�ȭ)
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    //���� Ŭ�� & ������ ����
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
                if (item != null)
                {
                    if (item.itemType == Item.ItemType.Used) // && !isCooltTime)
                    {
                        //PlayerPos.DecreaseWT(item.itemWeight); //�Ҹ�ǰ ���� ���԰���
                                                                   //��Ÿ�� ����
                        //currentCoolTime = coolTime;
                        //isCooltTime = true;

                        //������ ȹ�� ���� ����
                        //PlayerController.canPickUp = true;

                        //theItemEffectDatabase.UseItem(item);
                        SetSlotCount(-1);
                    }
                }           
        }
    }

    //���� �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this; // �巡�� ������ ������ ��
            DragSlot.instance.DragSetImage(itemImage); // �巡�� ���� �̹����� �־���
            DragSlot.instance.slotRect.anchoredPosition = eventData.position;
        }
    }

    //���� �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
        //theItemEffectDatabase.HideToolTip();
    }

    //���� �巡�� ����
    public void OnEndDrag(PointerEventData eventData) //�ٸ����̳� �ڱ� �ڽſ��� �巡�� ������� ����
    {
        //theItemEffectDatabase.HideToolTip();
        //!(���� ����true�� ���) == false �� 
        // �巡�װ� �������� �κ��丮or������ ������ �ƴ϶��

        //Debug.Log($"��Ŀ x : {DragSlot.instance.slotRect.anchoredPosition.x}");
        //Debug.Log($"��Ŀ y : {DragSlot.instance.slotRect.anchoredPosition.y}");
        //Debug.Log($"���̽� xMin : {baseRect.rect.xMin}");
        //Debug.Log($"���̽� xMax : {baseRect.rect.xMax}");
        //Debug.Log($"���̽� yMin : {baseRect.rect.yMin}");
        //Debug.Log($"���̽� yMax : {baseRect.rect.yMax}");

        //�κ��丮 ����
        if (!((DragSlot.instance.slotRect.anchoredPosition.x > baseRect.rect.xMin
            && DragSlot.instance.slotRect.anchoredPosition.x < baseRect.rect.xMax
            && DragSlot.instance.slotRect.anchoredPosition.y > baseRect.rect.yMin + 130f
            && DragSlot.instance.slotRect.anchoredPosition.y < baseRect.rect.yMax + 130f)
            ||
            //������ ����
            (DragSlot.instance.slotRect.anchoredPosition.x > quickSlotBaseRect.rect.xMin
            && DragSlot.instance.slotRect.anchoredPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.slotRect.anchoredPosition.y > quickSlotBaseRect.rect.yMin - 400f
            && DragSlot.instance.slotRect.anchoredPosition.y < quickSlotBaseRect.rect.yMax - 400f)))
            
        {
            //������ �Լ� ����
            //������ �Ǹ� �ڵ� Ȥ�� �Լ� �߰� //�Ʒ� �ڵ� ���� ����######
            if (DragSlot.instance.dragSlot != null) // �巡�׽��Կ� �������� �ִ°�츸 ����
            {
                Debug.Log("���� �� ���-�巡�� ���� ��ġ : " + DragSlot.instance.slotRect.anchoredPosition);
                //theInputNumber.Call();
            }
        }
        else //�κ��丮 ���� ������ ��� == �巡�� ���԰��� �����
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
      

    }

    public void OnDrop(PointerEventData eventData) // �ٸ� ���Կ��� �巡�װ� ������츸 ȣ��
    {
        if (DragSlot.instance.dragSlot != null) //�󽽷� ���� ChangeSlot ȣ�� ����
        {
            //theItemEffectDatabase.HideToolTip();
            ChangeSlot();
        }

    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount; //��ü���ϴ� ������ ���������� �̸� ����

        //��ü���ϴ� ���Կ� �巡�� ���� ����ü�� ���� �Է�(������/����)
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //��ü ���ϴ� ���Կ� �������� �ִٸ�
        if(_tempItem != null)
        {
            //�巡�� ���Կ� ��ü���ϴ� ������ ���� ����(_tempItem,_tempItemCount) �Է�
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else //��ü ���ϴ� ������ ����ٸ�
        {
            //��ü�ϴ� ���� �ʱ�ȭ
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }



    //���� �� Ŭ�� (���� ǥ��) == ������Ʈ)�̺�Ʈ �ý���

    // ��ư �Ϲ� Ŭ��
    public void ButtonClick()
    {
        print("��ư �Ϲ� Ŭ��");
    }


    // ��ư Ŭ���� �������� ��
    public void ButtonDown()
    {
        isClick = true;
    }

    // ��ư Ŭ���� ������ ��
    public void ButtonUp()
    {
        isClick = false;

        // Ŭ�� ���� �ð��� �ּ� Ŭ���ð� �̻��̶��
        if (clickTime >= minClickTime && item != null)
        {
            //���� ȣ��
            //theItemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }

    /*
    //������ ��Ÿ�ӿ�
    private void CoolTimeCalc()
    {
        Debug.Log("�۵���");
        currentCoolTime -= Time.deltaTime;
        for (int i = 0; i < img_CoolTime.Length; i++)
        {
            img_CoolTime[i].fillAmount = currentCoolTime / coolTime; // ��Ÿ�� ��ġ ������ �ݿ�
        }
        if (currentCoolTime <= 0)
            isCooltTime = false;
    }
    */
}
