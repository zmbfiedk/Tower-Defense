using System.Collections;
using UnityEngine;
using TMPro;

public class UiWave : MonoBehaviour
{
    [SerializeField] private WaveChecker waveChecker;       // Can assign in Inspector, or it will find
    [SerializeField] private TextMeshProUGUI textMeshPro;    // Assign in Inspector!

    private int waveNumber;

    void Start()
    {
        if (waveChecker == null)
        {
            GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
            if (waveManagerObject != null)
            {
                waveChecker = waveManagerObject.GetComponent<WaveChecker>();
            }
        }


        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI is NOT assigned in Inspector!");
        }
    }

    void Update()
    {
        if (textMeshPro == null) return;

        int currentWave = 0;
        if (waveChecker != null)
        {
            currentWave = waveChecker.GetWaveNumber();
        }

        if (currentWave != waveNumber)
        {
            StartCoroutine(WaveCoroutine(currentWave));
            waveNumber = currentWave;
        }

    }

    IEnumerator WaveCoroutine(int wave)
    {
        textMeshPro.text = "Wave " + wave;
        yield return new WaitForSeconds(2f);
        textMeshPro.text = "";
    }
}