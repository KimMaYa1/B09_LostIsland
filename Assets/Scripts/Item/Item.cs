using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject // 게임 오브젝트에 붙일 필요없는 스크립트
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

    public string itemName; // 아이템 이름
    [TextArea] // 인스펙터 창에서 여러줄로 적는것이 가능해짐-메모장처럼 됨
    public string itemDesc; // 아이템의 설명

    public int itemWeight; // 아이템 무게
    public ItemType itemType; // 아이템 유형
    public Sprite itemImage; // 아이템 이미지 = 인벤토리 이미지용 
    public GameObject itemPrefab; // 아이템 프리팹(월드의 아이템 드랍 실체용)


    public string weaponType; // "GUN" , PICKAXE" 등의 무기유형 


    public enum ItemType //열거형 아이템 타입
    {
        Equipment, // 장비
        Used, // 소모품
        Ingredient, // 재료
        ETC // 기타
    }

    
}