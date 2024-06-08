using UnityEngine;

public class PlayerData
{
	public bool audio = true;
	public int level = 1;
	public int speedMultiplier = 1;
	public int incomeMultiplier = 1;
	public int money = GameManager.Config.InitialMoneyAmount;
	public int currentSkin = 0;
	public int[] availableSkins = GameManager.Config.InitialSkins;
	public long lastClaimTimeTicks = 0;
	public int lastStreak = 0;
}

public class Storage
{
	private bool isDirty;
	private PlayerData playerData = new PlayerData();
	public int LastStreak
	{
		get
		{
			return playerData.lastStreak;
		}
		set 
		{ 
			playerData.lastStreak = value;
			SetDirty();
		}
	}
	public long LastClaimTimeTicks
	{
		get
		{
			return playerData.lastClaimTimeTicks;
		}
		set
		{
			playerData.lastClaimTimeTicks = value;
			SetDirty();
		}
	}
	public bool Audio
	{
		get
		{
			return playerData.audio;
		}
		set
		{
			playerData.audio = value;
			SetDirty();
		}
	}
	public int Level
	{
		get
		{
			return playerData.level;
		}
		set
		{
			playerData.level = value;
			GameManager.SDK.SetLeaderboradScore("LB" , playerData.level);
			SetDirty();
		}
	}
	public int SpeedMultiplier
	{
		get
		{
			return playerData.speedMultiplier;
		}
		set
		{
			playerData.speedMultiplier = value;
			SetDirty();
		}
	}
	public int IncomeMultiplier
	{
		get
		{
			return playerData.incomeMultiplier;
		}
		set
		{
			playerData.incomeMultiplier = value;
			SetDirty();
		}
	}
	public int Money
	{
		get
		{
			return playerData.money;
		}
		set
		{
			playerData.money = value;
			SetDirty();
		}
	}

	public int CurrentSkin
	{
		get
		{
			return playerData.currentSkin;
		}
		set
		{
			playerData.currentSkin = value;
			SetDirty();
		}
	}

	public int[] AvailableSkins
	{
		get
		{
			return playerData.availableSkins;
		}
		set
		{
			playerData.availableSkins = value;
			SetDirty();
		}
	}

	public Storage(string data)
	{
		if (data != null && data != string.Empty)
		{
			playerData = JsonUtility.FromJson<PlayerData>(data);
			GameManager.DebugMessage($"Data loaded successfully {data}");
		}
		else
		{
			playerData = new PlayerData();
			GameManager.DebugMessage("Data was null. Created new PlayerData");
		}
	}

	private void SaveData()
	{
		if (isDirty == false)
		{
			GameManager.DebugMessage("Nothing new to save");
			return;
		}

		string data = JsonUtility.ToJson(playerData);
		GameManager.SDK.SaveData(GameManager.Config.StorageKey, data);
		isDirty = false;
	}

	private void SetDirty()
    {
		if (isDirty) return;

        isDirty = true;
		SaveData();
    }
}

public class StorageReader
{
	public bool Audio => storage.Audio;
	public int Level => storage.Level;
	public int SpeedMultiplier => storage.SpeedMultiplier;
	public int IncomeMultiplier => storage.IncomeMultiplier;
	public int Money => storage.Money;
	public int CurrentSkin => storage.CurrentSkin;
	public int[] AvailableSkins => storage.AvailableSkins;
	private Storage storage;

	public StorageReader(Storage storage)
	{
		this.storage = storage;
	}
}
