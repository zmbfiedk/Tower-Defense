using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerAttackController))]
public class DoTweenPlayerAnimation : MonoBehaviour
{
    [Header("Movement Wobble")]
    [Tooltip("Minimum movement delta to trigger a wobble.")]
    [SerializeField] private float moveThreshold = 0.02f;
    [Tooltip("Cooldown between wobble triggers (to avoid spam).")]
    [SerializeField] private float wobbleCooldown = 0.08f;
    [Tooltip("How long the wobble animation lasts.")]
    [SerializeField] private float wobbleDuration = 0.35f;
    [Tooltip("Rotation punch (degrees) applied around Z axis for wobble.")]
    [SerializeField] private float wobbleRotationZ = 12f;
    [Tooltip("Scale punch amount (x,y).")]
    [SerializeField] private Vector3 wobbleScalePunch = new Vector3(0.06f, 0.04f, 0f);
    [Tooltip("Vibrato for punches (higher = more shakes)")]
    [SerializeField] private int wobbleVibrato = 10;
    [Range(0f, 1f)]
    [Tooltip("Elasticity for punch (0..1)")]
    [SerializeField] private float wobbleElasticity = 0.35f;

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

        if (delta >= moveThreshold && Time.time >= lastWobbleTime + wobbleCooldown)
        {
            TriggerWobble();
            lastWobbleTime = Time.time;
        }

        lastPosition = currentPosition;
    }

    private void TriggerWobble()
    {
        KillWobble();

        // Sequence: small scale punch + rotation punch (joined)
        wobbleSequence = DOTween.Sequence();

        // rotation punch around Z
        Vector3 rotPunch = new Vector3(0f, 0f, wobbleRotationZ);
        wobbleSequence.Append(transform.DOPunchRotation(rotPunch, wobbleDuration, wobbleVibrato, wobbleElasticity));

        // scale punch joined (slight scale in/out)
        wobbleSequence.Join(transform.DOPunchScale(wobbleScalePunch, wobbleDuration, wobbleVibrato, wobbleElasticity));

        wobbleSequence.Play();
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

    public void ForceWobble() => TriggerWobble();
    public void ForceCameraShake() => TriggerCameraShake();
}
