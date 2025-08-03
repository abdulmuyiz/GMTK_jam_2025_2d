using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSrc;

    [SerializeField] private AudioClip bgSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip dodgeSound;
    [SerializeField] private AudioClip expSound;

    [SerializeField] private float bgSoundVolume;
    [SerializeField] private float deathVolume;
    [SerializeField] private float enemyDeathVolume;
    [SerializeField] private float fireVolume;
    [SerializeField] private float dodgeVolume;
    [SerializeField] private float expVolume;

    public void DeathSound()
    {
        audioSrc.PlayOneShot(deathSound, deathVolume);
    }

    public void FireSound()
    {
        audioSrc.PlayOneShot(fireSound, fireVolume);
    }

    public void EnemyDeathSound()
    {
        audioSrc.PlayOneShot(enemyDeathSound, enemyDeathVolume);
    }

    public void DodgeSound()
    {
        audioSrc.PlayOneShot(dodgeSound, dodgeVolume);
    }

    public void FieldExp()
    {
        audioSrc.PlayOneShot(expSound, expVolume);
    }
}
