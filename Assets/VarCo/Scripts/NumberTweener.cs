using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VarCo
{
    public class NumberTweener : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp;
        [SerializeField] private float tweenDuartion;
        private int currentNumber = 0;

        public void SetNumber(int number)
        {
            DOTween.To(() => currentNumber, x => currentNumber = x, number, tweenDuartion).OnUpdate(() =>
            {
                tmp.text = currentNumber.ToString();
            }).OnComplete(() =>
            {
				tmp.text = number.ToString();
			}).SetUpdate(true);
        }
    }
}
