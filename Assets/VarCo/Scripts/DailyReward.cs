using System;

namespace VarCo
{
    public class DailyReward
    {
		public DateTime CurrentTime => useUtc ? DateTime.UtcNow : DateTime.Now;
		public DateTime LastClaimTime => lastClaimTime;
        public TimeSpan RemainingTime
        {
            get
            {
				DateTime cooldownEndTime = lastClaimTime.Add(cooldown);
				TimeSpan remainingTime = CurrentTime.Subtract(cooldownEndTime);
                return remainingTime;
			}
        }
        public bool CanClaim
        {
            get
            {
                DateTime lastClaimTime = this.lastClaimTime;
                DateTime cooldownEndTime = lastClaimTime.Add(cooldown);
				int result = DateTime.Compare(cooldownEndTime, CurrentTime);
                return result <= 0;
            }
        }
        public bool Expired
        {
            get
            {
				DateTime lastClaimTime = this.lastClaimTime;
				DateTime cooldownEndTime = lastClaimTime.Add(cooldown);
                DateTime expireTime = cooldownEndTime.Add(expiresIn);
                int result = DateTime.Compare(expireTime, CurrentTime);
				return result <= 0;
			}
        }
        public int Streak
        {
            get
            {
                if (Expired)
                {
                    streak = 0;
                }
                return streak;
            }
        }
		private DateTime lastClaimTime;
        private TimeSpan cooldown;
        private TimeSpan expiresIn;
        private int streak;
        private bool useUtc;

        public DailyReward(DateTime lastClaimTime, int lastStreak, TimeSpan cooldown, TimeSpan expiresIn, bool useUtc = true)
        {
            this.lastClaimTime = lastClaimTime;
            this.cooldown = cooldown;
            this.expiresIn = expiresIn;
            this.useUtc = useUtc;
            streak = Expired ? 0 : lastStreak;
        }

        public bool TryClaim()
        {
            if (CanClaim)
            {
				lastClaimTime = CurrentTime;
                streak = Expired ? 0 : streak + 1;
                return true;
			}
            return false;
		}
    }
}
