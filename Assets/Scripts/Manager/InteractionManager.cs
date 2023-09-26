using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask interactLayerMask;
    public LayerMask itemLayerMask;

    private GameObject curInteractGameObject;
    private Item curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Collider[] colls = Physics.OverlapSphere(transform.position, maxCheckDistance, interactLayerMask);

            for(int i = 0; i < colls.Length; i++)
            {
                Debug.Log(transform.position - colls[i].gameObject.transform.position);
            }

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;

            RaycastHit hit;

            if (Physics.Raycast (rayOrigin, rayDirection, out hit, maxCheckDistance, itemLayerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<ItemPickUp>().item;
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.Interactable());
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && curInteractable != null)
        {
            //curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
