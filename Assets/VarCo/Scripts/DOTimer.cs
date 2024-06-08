using DG.Tweening;
using System;

namespace VarCo
{
    public static class DOTimer
    {
        public static Sequence Set(float delay, Action action, bool timeScaleIndependent = true)
        {
            return DOTween.Sequence().AppendInterval(delay).AppendCallback(() => action?.Invoke()).SetUpdate(timeScaleIndependent);
        }
    }
}
