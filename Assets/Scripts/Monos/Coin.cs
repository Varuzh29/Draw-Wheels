using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VarCo;

public class Coin : MonoBehaviour
{
    [SerializeField] private TextMeshPro collectedTMP;

    public void OnCollected()
    {
        int amount = GameManager.StorageReader.IncomeMultiplier;
        collectedTMP.text = $"+{amount}";
        GameManager.LevelReward.CollectMoney(amount);
    }
}
