using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{

    public static bool inventoryActivated = false; //�κ��丮 Ȱ��ȭ ����
    public static bool shopActivated = false; //���� Ȱ��ȣ ������





    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent; //�׸��� ���� (��� ���԰���)
    //[SerializeField]
    //private PlayerController player;

    //����
    [SerializeField]
    private GameObject go_shopBase;
    [SerializeField]
    private GameObject go_shopSlotParent;
    private Animator anim;
    //����
    private Slot[] slots;
    void Start()
    {
        //���� �迭���� ��� ���� ���� �Է�
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        anim = GetComponent<Animator>();
    }




    public void TryOpenIventory()
    {
        inventoryActivated = !inventoryActivated; //������ Ȱ��ȭ �κ� ��Ȱ��ȭ ����Ī
        anim.SetBool("Appear", inventoryActivated); //�κ� Ȱ��/��Ȱ�� �ִ� 


        //�κ��丮�� ���� ���� ���º����� ���� ����/�帧 �� ���Ƴ���(�޸�)
        if (inventoryActivated)
        {
            OpenInventory();
        }


        else
        {
            CloseInventory();
        }
    }

    //�κ��丮 Ȱ��ȭ && ��Ȱ��ȭ
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    //������ ȹ��
    public void AcquireItem(Item _item, int _count = 1)
    {
        //if (!PlayerController.canPickUp)
            //return;
        
            if (Item.ItemType.Equipment != _item.itemType) // ��� �ƴѰ�쿡�� (���� ����x)
            {
                if (_item.itemType != Item.ItemType.Ingredient) //������ Ÿ���� "���" �� �ƴѰ�츸 ���� ���
                //player.IncreaseWT(_item.itemWeight);

            //���� �������� �ִ� ������ ���� �ݺ��� 
            for (int i = 0; i < slots.Length; i++) //���� ������ŭ �ݺ���
                {
                    if (slots[i].item != null) // ���� �⺻�� null�̹Ƿ� null���� ����
                    {
                        if (slots[i].item.itemName == _item.itemName) //&& PlayerController.canPickUp) //�Ѿ�� �������� �̹� �κ��� �ִٸ�
                        {
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                    }

                }

            }

            //���� �������� ���� ��� && ����� ���
            for (int i = 0; i < slots.Length; i++) //���� ������ŭ �ݺ���
            {
                if (slots[i].item == null) // && PlayerController.canPickUp) //�κ��� ���� �������� ���ٸ�
                {
                    slots[i].AddItem(_item, _count); // ������ additem �Լ� ȣ��(�� ������ �߰�)
                    return;
                }
            }          

    }

    public void DeAcquireItem(Item _item, int _count = -1)
    {
        //player.DecreaseWT(_item.itemWeight);
        //���� �������� �ִ� ������ ���� �ݺ��� 
        for (int i = 0; i < slots.Length; i++) //���� ������ŭ �ݺ���
        {
            if (slots[i].item != null) // ���� �⺻�� null�̹Ƿ� null���� ����
            {
                if (slots[i].item.itemName == _item.itemName) //�Ѿ�� �������� �̹� �κ��� �ִٸ�
                {
                    slots[i].SetSlotCount(_count);                  
                }
            }           
        }
    }

}


    
