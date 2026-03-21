using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop: MonoBehaviour
{
    public InputActionReference trackingAction;
    public InputActionReference clickingAction;
    private Transform selectedObject;
    [SerializeField] Vector3 offset;
    Plane dragPlane;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] bool hasBeenPlaced = false;

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
        // --- On récupère l'object cliqué --- //
        Ray ray = Camera.main.ScreenPointToRay(currentTouchPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectedObject = hit.transform;
            dragPlane = new Plane(-Camera.main.transform.forward, hit.point);
            offset = selectedObject.position - hit.point;
        }

        if (selectedObject == transform && hasBeenPlaced) return; // si l'objet a déjà été posé il ne bouge plus avec les clics
        
        if (selectedObject == transform)
        {
            rb.useGravity = false; // si le bouton est pressé on désactive la gravité
            rb.isKinematic = true; // il n'est pas touché par la phisique
            col.enabled = false; // si le bouton est pressé on désactive le collider
        }
    }

    [SerializeField] bool isTrigger = false; // est ce que l'objet touche un autre ?
    public void OnTrigerEnter(Collider other)
    {
        isTrigger = true;        
    }

    public void OnTrigerExit(Collider other)
    {
        isTrigger = false;       
    }

    public void OnTouchRelease(InputAction.CallbackContext context)
    {
        if (selectedObject == transform)
        {
            // Si l'object est dans un autre, on le place plus haut et il tombe
            if (isTrigger)
                selectedObject.position += new Vector3(0, 3, 0);

            rb.useGravity = true; // si le bouton est laché on réactive la gravité
            rb.isKinematic = false; // il est touché par la phisique
            col.enabled = true; // si le bouton est laché on réactive le collider
        
            hasBeenPlaced = true; // On se rappele que l'objet à été placé => il ne bouge plus
        }
        selectedObject = null;
    }
}
