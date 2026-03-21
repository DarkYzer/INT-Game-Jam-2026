using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using System.Drawing;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using System.Data.SqlTypes;

public class DragAndDropController: MonoBehaviour
{
    public static DragAndDropController Instance {get; private set;}

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    } 

    [Header("Score")]
    public int score;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("input Actions")]
    public InputActionReference trackingAction;
    public InputActionReference clickingAction;
    public InputActionReference turningAction;
    
    [Tooltip("List of objects already placed")] 
    public List<Transform> hasBeenPlaced = new List<Transform>();
    [SerializeField] LayerMask placedObjectLayerMask;
    [Tooltip("upOffset: offset de decalage vers le haut quand un objet est dans un autre")]
    [SerializeField] float upOffset;
    [Tooltip("rotateSpeed: vitesse de rotation des objets")]
    [SerializeField] float rotateSpeed;

    public Transform selectedObject;
    Plane dragPlane;
    Vector3 offset;
    private ObjectScript objScript;

    private Rigidbody rb;
    public List<Collider> cols = new List<Collider>(); // le collider pas trigger

    public UnityEvent<GameObject> OnPick; // Event qui renvoie l'objet lorsqu'on clique dessus

    private void OnEnable()
    {
        trackingAction.action.Enable();
        clickingAction.action.Enable();
        turningAction.action.Enable();

        trackingAction.action.performed += OnTouchProsition;
        clickingAction.action.performed += OnTouchPress;
        clickingAction.action.canceled += OnTouchRelease;
        turningAction.action.performed += OnTurn;
        turningAction.action.canceled += OnTurnEnd;
    
        scoreText.text = $"Score: {score}";
    }

    private void OnDisable()
    {
        trackingAction.action.performed -= OnTouchProsition;
        clickingAction.action.performed -= OnTouchPress;
        clickingAction.action.canceled -= OnTouchRelease;
        turningAction.action.performed -= OnTurn;
        turningAction.action.canceled -= OnTurnEnd;

        trackingAction.action.Disable();
        clickingAction.action.Disable();
        turningAction.action.Disable();
    }

    /* 
     * =========================================================
     *                        ROTATION
     * =========================================================
     */

    Vector2 input;
    float rotation;
    bool isTurning;
    private void OnTurn(InputAction.CallbackContext context)
    {
        if (selectedObject == null) return;
        isTurning = true;
        input = context.ReadValue<Vector2>();
        if (input.x < 0.1f)
            OnTurnLeft(context);
        if (input.x > 0.1f)
            OnTurnRight(context);
    }

    private void OnTurnEnd(InputAction.CallbackContext context)
    {
        rotation = 0;
        isTurning = false;
    }
    private void OnTurnRight(InputAction.CallbackContext context)
    {
        rotation = 1 * rotateSpeed;
    } 
    private void OnTurnLeft(InputAction.CallbackContext context)
    {
        rotation = -1 * rotateSpeed;
    }



    void Update()
    {
        if (selectedObject == null) return;

        if (isTurning)
            selectedObject.Rotate(0.0f, 0f, rotation, Space.Self);
    }

    /* 
     * =========================================================
     *                       DRAG & DROP
     * =========================================================
     */
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
            if (selectedObject.CompareTag("FixedObject"))
                selectedObject = null;
        }
        if (selectedObject == null) return;

        // si l'objet a déjà été posé il ne bouge plus avec les clics
        if (hasBeenPlaced.Contains(selectedObject)) 
        {
            selectedObject = null;
            return;
        }
        // --- On récupère le collider et le rigid body --- //
        rb = selectedObject.GetComponent<Rigidbody>();
        // on récupère le collider pas trigger
        foreach (Collider col in selectedObject.GetComponents<Collider>())
        {
            col.enabled = true;
            if (col.isTrigger)
                cols.Add(col);
        }

        rb.useGravity = false; // si le bouton est pressé on désactive la gravité
        rb.isKinematic = true; // il n'est pas touché par la phisique
        foreach (Collider col in cols)
            col.enabled = false; // si le bouton est pressé on désactive le collider

        OnPick.Invoke(selectedObject.gameObject);
    }

    public void OnTouchRelease(InputAction.CallbackContext context)
    {
        if (rb == null || selectedObject == null) return;
        rb.useGravity = true; // si le bouton est laché on réactive la gravité
        rb.isKinematic = false; // il est touché par la phisique
        foreach (Collider col in cols)
            col.enabled = false; // si le bouton est laché on réactive le collider

        hasBeenPlaced.Add(selectedObject); // On se rappele que l'objet à été placé => il ne bouge plus
        
        // Si l'object est dans un autre, on le place plus haut et il tombe
        // on fait un raycast très haut qui pointe vers le bas et on récupère le point de contact (le point le plus haut)
        Vector3 rayOrigin = selectedObject.position + new Vector3(0, 100, 0);
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity, placedObjectLayerMask))
        {
            // si le ray cast touche quelque chose d'autre que nous et de l'objet est plus haut
            if (hit.transform != selectedObject && hit.transform.position.y > selectedObject.position.y)
                selectedObject.position = selectedObject.position
                                        + new Vector3(0, hit.transform.position.y + upOffset, 0);
        } 

        selectedObject.gameObject.layer = LayerMask.NameToLayer("PlacedObject");

        // fake Object Update
        int size = hasBeenPlaced.Count;
        for (int i = 0; i < size; i++)
        {
            hasBeenPlaced[i]?.GetComponent<ObjectScript>()?.fakeUpdate();
            size = hasBeenPlaced.Count;
        }

        // Object de selectionné
        selectedObject = null;
        // gestion du score
        score += 1;
        scoreText.text = $"Score: {score}";

        cols.Clear();
    }
}
