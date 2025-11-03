using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerAttackController))]
public class DoTweenPlayerAnimation : MonoBehaviour
{
    

    [Header("Screen Shake (on shot)")]
    [Tooltip("Camera transform to shake. Leave null to use Camera.main.")]
    [SerializeField] private Transform cameraTransform;
    [Tooltip("Duration of camera shake in seconds")]
    [SerializeField] private float cameraShakeDuration = 0.18f;
    [Tooltip("Strength of camera shake (world units)")]
    [SerializeField] private float cameraShakeStrength = 0.25f;
    [Tooltip("How many shakes (vibrato)")]
    [SerializeField] private int cameraShakeVibrato = 14;
    [Tooltip("Whether the shake fades out")]
    [SerializeField] private bool cameraShakeFadeOut = true;

    // internals
    private Vector3 lastPosition;
    private float lastWobbleTime = -999f;
    private Sequence wobbleSequence;
    private Tween cameraShakeTween;
    private PlayerAttackController attackController;

    private void Awake()
    {
        attackController = GetComponent<PlayerAttackController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        lastPosition = transform.position;
    }

    private void OnEnable()
    {
        if (attackController != null)
            attackController.OnShot += HandleOnShot;
    }

    private void OnDisable()
    {
        if (attackController != null)
            attackController.OnShot -= HandleOnShot;

        KillWobble();
        KillCameraShake();
    }

    private void Update()
    {
        CheckMovementWobble();
    }

    private void CheckMovementWobble()
    {
        Vector3 currentPosition = transform.position;
        float delta = (currentPosition - lastPosition).magnitude;


        lastPosition = currentPosition;
    }



    private void HandleOnShot(TowerAttackController.TowerType type)
    {
        TriggerCameraShake();
    }

    private void TriggerCameraShake()
    {
        if (cameraTransform == null) return;

        KillCameraShake();

        cameraShakeTween = cameraTransform.DOShakePosition(cameraShakeDuration, cameraShakeStrength, cameraShakeVibrato, randomness: 90f, snapping: false, fadeOut: cameraShakeFadeOut);
    }

    private void KillWobble()
    {
        if (wobbleSequence != null && wobbleSequence.IsActive())
        {
            wobbleSequence.Kill();
            wobbleSequence = null;
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z);
    }

    private void KillCameraShake()
    {
        if (cameraShakeTween != null && cameraShakeTween.IsActive())
        {
            cameraShakeTween.Kill();
            cameraShakeTween = null;
        }
    }
    public void ForceCameraShake() => TriggerCameraShake();
}
