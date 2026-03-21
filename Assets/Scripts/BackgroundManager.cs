using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BackgroundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform balance;
    [SerializeField] Transform background;
    private float smoothSpeed = 5f;


    private float default_rotation;
    private Vector3 default_translation;
    void Start()
    {
        default_rotation = balance.rotation.eulerAngles.z;
        default_translation=background.position;
    }   

    // Update is called once per frame
    void Update()
    {
        float offset = (balance.rotation.eulerAngles.z - default_rotation) * 0.1f;
        Vector3 targetPosition = new Vector3(default_translation.x - offset, default_translation.y, default_translation.z);

        background.position = Vector3.Lerp(
            background.position,
            targetPosition,
            smoothSpeed * Time.deltaTime);
    }
}
