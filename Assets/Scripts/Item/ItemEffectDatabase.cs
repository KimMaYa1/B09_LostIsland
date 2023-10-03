using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//##�������ͽ��� ������ �ִ� ��ü���� ������ �����ͺ��̽�##

[System.Serializable]
public class ItemEffect
{
    public string itemName; //�������� �̸�(Ű��)

    //�������� ȿ���� �������� �� �� �����Ƿ� �迭�� ����
    [Tooltip("HP,MP,DP �� ���밡��")]
    public string[] part; // ������ �� �κ� (hp ..)
    public float[] num; // ���� ��ġ( hp -10 ...)
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects; //�Ҹ�ǰ �����۵�
    //[SerializeField]
    //private SlotToolTip toolTip;
    [SerializeField]
    private PlayerConditins player;
    [SerializeField]
    private PlayerController Stat;

    //��� ���� == �������� ȿ�� string
    private const string HEALTH = "HEALTH", HUNGER = "HUNGER", THIRST = "THIRST", STAMINA = "STAMINA", ATTACK = "ATTACK", DEFENSE = "DEFENSC";



    //���� �κ��丮 �� ��� ���� ��ü �Լ�
    //[serializeField] private WeaponManager theWeaponManager;

    //�Լ� ���� slot.cs���� _item �Ѱ��ְ�-> �ش� �Լ����� SlotToolTip.cs�� ShowToolTip ����
   //�ش� ����� ���� �����÷ο� �Ͼ �� ������ ����
   /*
    public void ShowToolTip(Item _item, Vector2 _pos)
    {
        toolTip.ShowToolTip(_item, _pos);

    }
    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }
   */
    //������ ���
    public void UseItem(Item _item)
    {
        /*
         if(item.itemType == Item.ItemType.Equipment)
        {
           StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        else if ..*/


        if (_item.itemType == Item.ItemType.Used || _item.itemType == Item.ItemType.Equipment)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName) //�޾ƿ� �������� �̸��� ���� �������� �迭���� Ž��
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++) // Ž���� �������� ȿ�� ������ŭ �ݺ�
                    {
                        switch (itemEffects[x].part[y]) // y��° ȿ���� ����
                        {
                            case HEALTH:
                                player.Heal(itemEffects[x].num[y]); //Ž���� �������� y��° ��ġ����
                                break;
                            case HUNGER:
                                player.Eat(itemEffects[x].num[y]);
                                break;
                            case THIRST:
                                player.Drink(itemEffects[x].num[y]);
                                break;
                            case STAMINA:
                                player.UseStamina(-itemEffects[x].num[y]);
                                break;
                            case ATTACK:
                                Stat.playerStat.Atk += (itemEffects[x].num[y]);
                                break;
                            case DEFENSE:
                                Stat.playerStat.Def += (itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("�߸��� status �ڿ� �����");
                                break;
                        }
                        Debug.Log(_item.itemName + "�� ����߽��ϴ�");

                    }
                    return;
                }

            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� ����");
        }
    }

    //��� ������ Ż��
    public void Unequipped(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case ATTACK:
                                Stat.playerStat.Atk -= (itemEffects[x].num[y]);
                                break;
                            case DEFENSE:
                                Stat.playerStat.Def -= (itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("�߸��� status �ڿ� �����");
                                break;
                        }
                        Debug.Log(_item.itemName + "�� ���� ������");

                    }
                    return;
                }

            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� ����");
        }
    }
}


