using TMPro;
using UnityEngine;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private GameObject claimedOverlay;
    [SerializeField] private TextMeshProUGUI dayNumber;
    [SerializeField] private TextMeshProUGUI rewardAmount;

    public RewardItemUI SetClaimed(bool claimed)
    {
        claimedOverlay.SetActive(claimed);
		return this;
    }

    public RewardItemUI SetDayNumber(int dayNumber)
    {
        this.dayNumber.text = dayNumber.ToString();
		return this;
    }

    public RewardItemUI SetRewardAmount(int rewardAmount)
    {
        this.rewardAmount.text = rewardAmount.ToString();
		return this;
    }
}
