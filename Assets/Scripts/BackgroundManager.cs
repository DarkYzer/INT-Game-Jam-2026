using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform balance;
    public float maxOffset = 2000f; // déplacement max en pixels
    public float maxAngle = 45f;   // angle max attendu
    public float smoothSpeed = 5f;

    [SerializeField] private RectTransform rectTransform;
    private float targetX;


    void Update()
    {
        if (balance == null || rectTransform == null)
            return;

        float angle = balance.eulerAngles.z;
        if (angle > 180) angle -= 360;

        float normalized = Mathf.Clamp(angle / maxAngle, -1f, 1f);

        // Calcul position cible
        targetX = normalized * maxOffset;

        // Smooth movement
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * smoothSpeed);
        rectTransform.anchoredPosition = pos;
    }
}