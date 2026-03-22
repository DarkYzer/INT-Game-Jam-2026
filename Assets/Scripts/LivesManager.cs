using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public static int lives = 5;
    private Camera _mainCamera;
    private Vector3 _cameraStartPosition;
    private Vector3 _shakeDirection;
    private bool _shouldShake;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private GameObject balance;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private AnimationCurve gameOverFallingCurve;

    private void Start()
    {
        _mainCamera = Camera.main;
        lives = 5;
        if (_mainCamera) _cameraStartPosition = _mainCamera.transform.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        StartCoroutine(LivesUpdate());
        if (lives == 0) StartCoroutine(GameOver());
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
            var strength = shakeCurve.Evaluate(time / shakeDuration) * shakeStrength;
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

    private bool _canTakeDamage = true;
    private IEnumerator LivesUpdate()
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            lives--;
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
            _canTakeDamage = true;
        };
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        balance.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
        pauseMenu.YouDied();
        gameOverScreen.TryGetComponent<RectTransform>(out var pos);
        pos.anchorMin = new Vector2(0,1);
        pos.anchorMax = new Vector2(1,2);
        float time = 0;
        while (time < 2)
        {
            time += Time.deltaTime;
            pos.anchorMin = new Vector2(0,1 - gameOverFallingCurve.Evaluate(time*.5f));
            pos.anchorMax = new Vector2(1,2 - gameOverFallingCurve.Evaluate(time*.5f));
            yield return new WaitForEndOfFrame();
        }
        pos.anchorMin = new Vector2(0,0);
        pos.anchorMax = new Vector2(1,1);
    }
}
