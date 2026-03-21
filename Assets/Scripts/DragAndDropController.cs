using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDropController: MonoBehaviour
{
    [Header("input Actions")]
    public InputActionReference trackingAction;
    public InputActionReference clickingAction;
    
    [Tooltip("List of objects already placed")] 
    [SerializeField] List<Transform> hasBeenPlaced = new List<Transform>();
    [SerializeField] LayerMask placedObjectLayerMask;

    private Transform selectedObject;
    Plane dragPlane;
    Vector3 offset;
    private ObjectScript objScript;

    private Rigidbody rb;
    private Collider col; // le collider pas trigger

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
            objScript = selectedObject.GetComponent<ObjectScript>();
        }

        // si l'objet a déjà été posé il ne bouge plus avec les clics
        if (hasBeenPlaced.Contains(selectedObject)) 
        {
            selectedObject = null;
            return;
        }
        // --- On récupère le collider et le rigid body --- //
        rb = selectedObject.GetComponent<Rigidbody>();
        // on récupère le collider pas trigger
        col = selectedObject.GetComponents<Collider>()[0];
        if (col.isTrigger == true)
            col = selectedObject.GetComponents<Collider>()[1];

        rb.useGravity = false; // si le bouton est pressé on désactive la gravité
        rb.isKinematic = true; // il n'est pas touché par la phisique
        col.enabled = false; // si le bouton est pressé on désactive le collider
    }

    public void OnTouchRelease(InputAction.CallbackContext context)
    {

        rb.useGravity = true; // si le bouton est laché on réactive la gravité
        rb.isKinematic = false; // il est touché par la phisique
        col.enabled = true; // si le bouton est laché on réactive le collider
    
        if (selectedObject != null)
            hasBeenPlaced.Add(selectedObject); // On se rappele que l'objet à été placé => il ne bouge plus
        
        // Si l'object est dans un autre, on le place plus haut et il tombe
        // on fait un raycast très haut qui pointe vers le bas et on récupère le point de contact (le point le plus haut)
        Vector3 rayOrigin = selectedObject.position + new Vector3(0, 100, 0);
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity, placedObjectLayerMask))
        {
            if (hit.transform != selectedObject)
                selectedObject.position = selectedObject.position
                                        + new Vector3(0, hit.transform.position.y + 1, 0);
        } 

        selectedObject.gameObject.layer = LayerMask.NameToLayer("PlacedObject");
        // Object de selectionné
        selectedObject = null;  
    }
}
