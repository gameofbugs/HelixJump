using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioSource BackGround;
    public AudioSource SoundEffects;

    public AudioClip bgClip;

    public AudioClip ball;
    public AudioClip gameOver;
    public AudioClip scoreAdd;
    public AudioClip highScore;
    public AudioClip stackDestroy;
    public AudioClip button;


    public void Start()
    {
        BGM(bgClip);
    }

    public void BGM(AudioClip clip)
    {
        BackGround.clip = clip;
        BackGround.loop = true;
        BackGround.Play();
    }
    public void SFX(AudioClip clip)
    {
        SoundEffects.PlayOneShot(clip);
    }
}