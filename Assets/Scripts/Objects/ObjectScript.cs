using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectScript : MonoBehaviour
{
    [SerializeField] private float shakeAngle = 20f;
    [SerializeField] private float shakeFastTime = .1f;
    [SerializeField] private float shakeSlowTime = .3f;
    public virtual void fakeUpdate()
    {
        return;
    }
    public bool isTrigger = false; // est ce que l'objet touche un autre ?bool isTrigger = false; // est ce que l'objet touche un autre ?
    public void OnTriggerStay(Collider other)
    {
        isTrigger = true;
        outlineScript.enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        isTrigger = false;
        outlineScript.enabled = false;   
    }

    public void shakeSlow(float shakeTotalTime)
    {
        StartCoroutine(rotate(shakeAngle, shakeFastTime, shakeTotalTime));
    }

    public void shakeFast(float shakeTotalTime)
    {
        StartCoroutine(rotate(shakeAngle, shakeSlowTime, shakeTotalTime));
    }

    private IEnumerator rotate(float angle, float time, float totalTime)
    {
        float grandTimer = 0f;
        while (grandTimer < totalTime)
        {
            float timer = 0f;
            while (timer < time)
            {
                Vector3 eulerAngle = new Vector3(0, 0, angle / Time.deltaTime);
                transform.Rotate(eulerAngle);
                yield return null;
                timer += Time.deltaTime;
                grandTimer += Time.deltaTime;
            }
        }
    }

    // outline
    Outline outlineScript;
    void Start()
    {
        outlineScript = GetComponent<Outline>();
        outlineScript.enabled = false;
    }
}
