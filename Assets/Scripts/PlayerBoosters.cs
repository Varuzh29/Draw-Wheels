using System;

public class PlayerBoosters
{
    public int SpeedPrice => storage.SpeedMultiplier * GameManager.Config.SpeedPriceStep;
    public int IncomePrice => storage.IncomeMultiplier * GameManager.Config.IncomePriceStep;
    public bool CanBuySpeed => storage.Money >= SpeedPrice;
    public bool CanBuyIncome => storage.Money >= IncomePrice;
    public event Action ValuesChanged;
	private PlayerWallet playerWallet;
    private Storage storage;

    public PlayerBoosters(PlayerWallet playerWallet, Storage storage)
    {
        this.playerWallet = playerWallet;
        this.storage = storage;
    }

    public void IncreaseSpeed()
    {
        if (playerWallet.TryPurchase(SpeedPrice))
        {
            storage.SpeedMultiplier++;
            ValuesChanged?.Invoke();
        }
        else
        {
            GameManager.GetReward(() =>
            {
                storage.SpeedMultiplier++;
				ValuesChanged?.Invoke();
			});
        }
    }

    public void IncreaseIncome()
    {
		if (playerWallet.TryPurchase(IncomePrice))
		{
			storage.IncomeMultiplier++;
		}
		else
		{
            GameManager.GetReward(() =>
            {
                storage.IncomeMultiplier++;
				ValuesChanged?.Invoke();
			});
		}
	}
}
