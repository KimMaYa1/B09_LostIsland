using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InteractionManager : MonoBehaviour
{
    public LayerMask interactLayerMask;
    public LayerMask itemLayerMask;
    public LayerMask monsterLayerMask;

    private GameObject curInteractGameObject;
    private Item curInteractable;
    private UIManager uiManager;

    //public TextMeshProUGUI promptText;
   /* [Header("ItemInfo")]
    public GameObject itemInfoObject;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemInfoText;*/

    [Header("CurSor")]
    public Texture2D defaultCurSor;
    public Texture2D interactionCurSor;
    public Texture2D attackCurSor;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(defaultCurSor, Vector2.left + Vector2.up, CursorMode.Auto);

        if (UIManager.instance.inventoryActivated)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (((1<<hit.collider.gameObject.layer)|itemLayerMask) == itemLayerMask)
            {
                curInteractGameObject = hit.collider.gameObject;
                if (hit.collider.TryGetComponent<ItemPickUp>(out ItemPickUp itemPickUp))
                {
                    curInteractable = itemPickUp.item;
                    SetPromptText();
                    Cursor.SetCursor(interactionCurSor, Vector2.left + Vector2.up, CursorMode.Auto);
                    return;
                }
            }
            if (((1 << hit.collider.gameObject.layer) | interactLayerMask) == interactLayerMask)
            {
                curInteractable = hit.collider.GetComponent<ItemPickUp>().item;
                SetPromptText();
                Cursor.SetCursor(interactionCurSor, Vector2.left + Vector2.up, CursorMode.Auto);
                return;
            }
            if (((1 << hit.collider.gameObject.layer)| monsterLayerMask) == monsterLayerMask)
            {
                //���� ���� ���

                Cursor.SetCursor(attackCurSor, Vector2.left + Vector2.up, CursorMode.Auto);
                return;
            }
        }
        curInteractGameObject = null;
        curInteractable = null;
        //promptText.gameObject.SetActive(false);
        uiManager.ClearInteractItem();
    }

    private void SetPromptText()
    {
        //promptText.gameObject.SetActive(true);
        //promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.Interactable());
        /*itemInfoObject.SetActive(true);
        itemNameText.text = string.Format(curInteractable.Interactable());
        itemInfoText.text = string.Format(curInteractable.itemDesc);*/
        uiManager.InteractItem(curInteractable);
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && curInteractable != null)
        {
            //curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            uiManager.ClearInteractItem();
            //promptText.gameObject.SetActive(false);
        }
    }
    
}
