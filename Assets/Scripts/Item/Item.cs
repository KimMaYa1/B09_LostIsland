using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject // ���� ������Ʈ�� ���� �ʿ���� ��ũ��Ʈ
{

    IInteractable item;


    public string Interactable()
    {
        if(item == null) 
        {
            item = new ItemInteraction();
        }
        return item.GetInteractPrompt(itemName);
    }

    public string itemName; // ������ �̸�
    [TextArea] // �ν����� â���� �����ٷ� ���°��� ��������-�޸���ó�� ��
    public string itemDesc; // �������� ����

    public int itemWeight; // ������ ����
    public ItemType itemType; // ������ ����
    public Sprite itemImage; // ������ �̹��� = �κ��丮 �̹����� 
    public GameObject itemPrefab; // ������ ������(������ ������ ��� ��ü��)


    public string weaponType; // "GUN" , PICKAXE" ���� �������� 


    public enum ItemType //������ ������ Ÿ��
    {
        Equipment, // ���
        Used, // �Ҹ�ǰ
        Ingredient, // ���
        ETC // ��Ÿ
    }

    
}