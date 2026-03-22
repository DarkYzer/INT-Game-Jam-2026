using UnityEngine;

public class PlaySoundNTimesOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private int n;
    
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySound(clip);
        n--;
        if (n <= 0) Destroy(this);
    }
}
