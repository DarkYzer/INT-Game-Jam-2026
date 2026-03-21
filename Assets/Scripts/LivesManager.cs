using System.Collections;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public int lives = 3;
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
        Debug.Log("hehehe");
        lives--;
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
}
