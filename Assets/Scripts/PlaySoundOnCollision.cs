using System.Collections;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private bool yes = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    private void Update()
    {
        if (yes)
        {
            audioSource.Play();
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yes = false;
        yield return new WaitForSeconds(1);
        yes = true;
    }
}
