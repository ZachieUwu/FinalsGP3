using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSourse;
    [SerializeField] AudioSource SFXSourse;

    public AudioClip bg;
    public AudioClip pick;
    public AudioClip landing;
    public AudioClip portal;
    public AudioClip jump;


    private void Start()
    {
        MusicSourse.clip = bg;
        MusicSourse.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSourse.PlayOneShot(clip);
    }

}
