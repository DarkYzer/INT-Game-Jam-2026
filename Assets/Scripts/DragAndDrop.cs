using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop: MonoBehaviour
{
    public InputActionReference trackingAction;
    public InputActionReference clickingAction;
    Transform selectedObject;
    [SerializeField] Vector3 offset;
    Plane dragPlane;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnEnable()
    {
        trackingAction.action.Enable();
        clickingAction.action.Enable();

        trackingAction.action.performed += OnTouchProsition;
        clickingAction.action.performed += OnTouchPress;
        clickingAction.action.canceled += OnTouchRelease;
    }

    private void OnDisable()
    {
        trackingAction.action.performed -= OnTouchProsition;
        clickingAction.action.performed -= OnTouchPress;
        clickingAction.action.canceled -= OnTouchRelease;  

        trackingAction.action.Disable();
        clickingAction.action.Disable();
    }

    Vector2 currentTouchPos;
    public void OnTouchProsition(InputAction.CallbackContext context)
    {
        currentTouchPos = context.ReadValue<Vector2>();
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            0
        );
    
        if (selectedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(currentTouchPos);
            if (dragPlane.Raycast(ray, out float distance))
            {
                selectedObject.position = ray.GetPoint(distance) + offset;
            }
        }
    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {
        rb.useGravity = false;
        Ray ray = Camera.main.ScreenPointToRay(currentTouchPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectedObject = hit.transform;
            dragPlane = new Plane(-Camera.main.transform.forward, hit.point);
            offset = selectedObject.position - hit.point;
        }
    }

    public void OnTouchRelease(InputAction.CallbackContext context)
    {
        rb.useGravity = true;
       selectedObject = null;
    }
}
