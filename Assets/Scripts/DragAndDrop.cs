using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop: MonoBehaviour
{
    Transform selectedObject;
    [SerializeField] Vector3 offset;
    Plane dragPlane;
    Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    Vector2 currentTouchPos;
    public void OnTouchProsition(InputAction.CallbackContext context)
    {
        currentTouchPos = context.ReadValue<Vector2>();
    
        if (selectedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(currentTouchPos);
            if (dragPlane.Raycast(ray, out float distance))
            {
                selectedObject.position = ray.GetPoint(distance) + offset;
            }
        }
        if (context.canceled)
        {
            selectedObject = null;
        }
    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(currentTouchPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectedObject = hit.transform;
            dragPlane = new Plane(-Camera.main.transform.forward, hit.point);
            offset = selectedObject.position - hit.point;
        }
    }
}
