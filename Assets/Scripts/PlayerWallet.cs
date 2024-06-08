using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet
{
    public int MoneyCount => storage.Money;
    public event Action MoneyCountChanged;
    private Storage storage;

    public PlayerWallet(Storage storage)
    {
        this.storage = storage;
    }

    public bool TryPurchase(int price)
    {
        if(storage.Money < price)
        {
            return false;
        }
        else
        {
            storage.Money -= price;
            MoneyCountChanged?.Invoke();
            return true;
        }
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            GameManager.DebugMessage("Can't add zero or negative amount of money!");
            return;
        }

		storage.Money += amount;
		MoneyCountChanged?.Invoke();
	}
}
