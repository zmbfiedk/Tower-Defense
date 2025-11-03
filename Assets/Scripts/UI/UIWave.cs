using System.Collections;
using UnityEngine;
using TMPro;

public class UiWave : MonoBehaviour
{
    [SerializeField] private WaveChecker waveChecker;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private int lastShownWave = -1;
    private Coroutine waveRoutine;

    private void Start()
    {
        if (waveChecker == null)
        {
            GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
            if (waveManagerObject != null)
            {
                waveChecker = waveManagerObject.GetComponent<WaveChecker>();
            }
            else
            {
                Debug.LogError("[UiWave] No object with tag 'WaveManager' found!");
            }
        }

        if (textMeshPro == null)
        {
            Debug.LogError("[UiWave] TextMeshProUGUI not assigned!");
        }
    }

    private void Update()
    {
        if (waveChecker == null || textMeshPro == null) return;

        int currentWave = waveChecker.GetWaveNumber();

        if (currentWave != lastShownWave)
        {
            lastShownWave = currentWave;
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);

            waveRoutine = StartCoroutine(ShowWaveText(currentWave));
        }
    }

    private IEnumerator ShowWaveText(int wave)
    {

        textMeshPro.text = "Wave " + wave;
        yield return new WaitForSeconds(2f);
        textMeshPro.text = "";
    }
}
