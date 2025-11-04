using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Settings")]
    [SerializeField] private int startingCurrency = 100;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI currencyTextTMP;
    [SerializeField] private Text currencyText; 

    private int currentCurrency;

    public int CurrentCurrency
    {
        get => currentCurrency;
        private set
        {
            currentCurrency = value;
            UpdateUI();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentCurrency = startingCurrency;
    }

    private void UpdateUI()
    {
        if (currencyTextTMP != null)
            currencyTextTMP.text = CurrentCurrency.ToString();
        else if (currencyText != null)
            currencyText.text = CurrentCurrency.ToString();
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            return true;
        }
        return false;
    }

    public void SetCurrency(int amount)
    {
        CurrentCurrency = amount;
    }
}
