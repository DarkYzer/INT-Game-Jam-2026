using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            cam.fieldOfView -= scroll * 0.5f;
        }
    }
}
