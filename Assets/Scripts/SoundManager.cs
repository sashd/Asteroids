using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource thrustSound;
    [SerializeField] private AudioSource explosionSound;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayFireSound(float pitch)
    {
        fireSound.pitch = pitch;
        fireSound.Play();
    }

    public void PlayThrustSound()
    {
        if (!thrustSound.isPlaying)
        {
            thrustSound.Play();
        }
    }

    public void StopThrustSound()
    {
        thrustSound.Stop();
    }

    public void PlayExplosionSound()
    {
        explosionSound.Play();
    }
}
