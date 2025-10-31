using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAttackController))]
public class PlayerSound : MonoBehaviour
{
    [Header("Audio Source (optional)")]
    [SerializeField] private AudioSource audioSource;

    [Header("Weapon Shot Clips")]
    [SerializeField] private AudioClip singleShotClip;
    [SerializeField] private AudioClip tripleShotClip;
    [SerializeField] private AudioClip burstClip;
    [SerializeField] private AudioClip laserBurstClip;
    [SerializeField] private AudioClip rifleClip;
    [SerializeField] private AudioClip freezeThreeClip;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    private PlayerAttackController attackController;

    private void Awake()
    {
        attackController = GetComponent<PlayerAttackController>();

        if (audioSource == null)
        {
            // create one if not assigned
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnEnable()
    {
        if (attackController != null)
            attackController.OnShot += HandleShot;
    }

    private void OnDisable()
    {
        if (attackController != null)
            attackController.OnShot -= HandleShot;
    }

    private void HandleShot(TowerAttackController.TowerType type)
    {
        AudioClip clip = null;
        switch (type)
        {
            case TowerAttackController.TowerType.SingleShot: clip = singleShotClip; break;
            case TowerAttackController.TowerType.TripleShot: clip = tripleShotClip; break;
            case TowerAttackController.TowerType.Burst: clip = burstClip; break;
            case TowerAttackController.TowerType.LaserBurst: clip = laserBurstClip; break;
            case TowerAttackController.TowerType.Rifle: clip = rifleClip; break;
            case TowerAttackController.TowerType.FreezeThree: clip = freezeThreeClip; break;
        }

        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
