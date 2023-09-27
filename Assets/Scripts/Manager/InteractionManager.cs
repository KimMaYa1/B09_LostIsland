using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask interactLayerMask;
    public LayerMask itemLayerMask;

    private GameObject curInteractGameObject;
    private Item curInteractable;

    //public TextMeshProUGUI promptText;
    [Header("ItemInfo")]
    public GameObject itemInfoObject;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemInfoText;

    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, itemLayerMask))
        {
            curInteractGameObject = hit.collider.gameObject;
            curInteractable = hit.collider.GetComponent<ItemPickUp>().item;
            SetPromptText();
            return;
        }

        curInteractGameObject = null;
        curInteractable = null;
        //promptText.gameObject.SetActive(false);
        UIManager.instance.ClearInteractItem();
    }

    private void SetPromptText()
    {
        //promptText.gameObject.SetActive(true);
        //promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.Interactable());
        /*itemInfoObject.SetActive(true);
        itemNameText.text = string.Format(curInteractable.Interactable());
        itemInfoText.text = string.Format(curInteractable.itemDesc);*/
        UIManager.instance.InteractItem(curInteractable);
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && curInteractable != null)
        {
            //curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            UIManager.instance.ClearInteractItem();
            //promptText.gameObject.SetActive(false);
        }
    }
    
}
