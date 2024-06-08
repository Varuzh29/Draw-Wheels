using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReward
{
    public int CollectedMoneyAmount { get; private set; } = 0;
    private PlayerWallet playerWallet;

    public LevelReward(PlayerWallet playerWallet)
    {
        this.playerWallet = playerWallet;
    }

    public void Reset()
    {
        CollectedMoneyAmount = 0;
    }

    public void CollectMoney(int amount)
    {
        if (GameManager.GameState != GameState.Race)
        {
            GameManager.DebugMessage("Can't collect coins after or before race");
            return;
        }

        if (amount <= 0)
        {
            GameManager.DebugMessage("Can't collect zero or negative amount of money!");
            return;
        }

        CollectedMoneyAmount += amount;
    }

    public void GiveMoneyToPlayer(int multiplier)
    {
        int amount = CollectedMoneyAmount * multiplier;
        playerWallet.AddMoney(amount);
    }
}
