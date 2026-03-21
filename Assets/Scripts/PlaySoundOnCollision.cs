using System.Collections;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySound(clip);
    }
}
