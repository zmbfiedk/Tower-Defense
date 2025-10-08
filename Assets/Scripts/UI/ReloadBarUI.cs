using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReloadBarUI : MonoBehaviour
{
    [SerializeField] private Slider reloadSlider;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 50, 0);

    void Start()
    {
        if (player != null)
        {
            PlayerAttackController attackController = player.GetComponent<PlayerAttackController>();
            if (attackController != null)
                attackController.OnReload += StartReload; // instance access now works
        }
    }


    private void LateUpdate()
    {
        if (reloadSlider != null && player != null && Camera.main != null)
        {
            Vector3 screenPos = player.position+ offset;//Camera.main.WorldToScreenPoint(player.position);
            reloadSlider.transform.position = screenPos;
        }
    }

    public void StartReload(float reloadTime)
    {
        if (reloadSlider != null)
            StartCoroutine(FillReloadBar(reloadTime));
    }

    private IEnumerator FillReloadBar(float reloadTime)
    {
        float elapsed = 0f;
        reloadSlider.value = 0f;
        reloadSlider.gameObject.SetActive(true);

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            reloadSlider.value = elapsed / reloadTime;
            yield return null;
        }

        reloadSlider.value = 1f;
        yield return new WaitForSeconds(0.1f);
        reloadSlider.gameObject.SetActive(false);
    }
}
