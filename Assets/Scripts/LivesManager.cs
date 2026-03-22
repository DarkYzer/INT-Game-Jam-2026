using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public static int lives = 3;
    private Camera _mainCamera;
    private Vector3 _cameraStartPosition;
    private Vector3 _shakeDirection;
    private bool _shouldShake;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera) _cameraStartPosition = _mainCamera.transform.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        StartCoroutine(livesUpdate());
        StartCoroutine(Remove(other.gameObject));
        DragAndDropController.Instance.hasBeenPlaced.Remove(other.transform);
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float time = 0;
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            var strength = curve.Evaluate(time / shakeDuration) * shakeStrength;
            if (_shakeDirection.magnitude == 0) _shakeDirection = Random.insideUnitSphere;
            _mainCamera.transform.position += _shakeDirection * strength;
            yield return null;
            _mainCamera.transform.position = _cameraStartPosition;
            yield return null;
            _shakeDirection = Vector3.zero;
        }
        _mainCamera.transform.position = _cameraStartPosition;
    }

    private IEnumerator Remove(GameObject go)
    {
        yield return new WaitForSeconds(2);
        Destroy(go);
    }

    // ===== Gestion des coeurs ===== //
    [SerializeField] List<Image> heartList;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite fullHeart;

    bool canTakeDamage = true;
    IEnumerator livesUpdate()
    {
        if (canTakeDamage) lives--;;
        canTakeDamage = false;
        for (int i = 0; i < heartList.Count; i++)
        {
            if (lives <= i)
            {
                heartList[i].sprite = emptyHeart;
                heartList[i].color = Color.black;
            }
            else
            {
                heartList[i].sprite = fullHeart;
                heartList[i].color = Color.white;
            }
        }
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }

}
