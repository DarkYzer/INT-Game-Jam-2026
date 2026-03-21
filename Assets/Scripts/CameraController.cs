using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Transform cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            cam.position -= new Vector3(0, scroll * 0.2f, 0);
        }
    }
}
