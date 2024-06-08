using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPLocString : MonoBehaviour
{
    [SerializeField] private string key;
    private TMP_Text tMP_Text;
    private TMP_Text TMP_Text
    {
        get
        {
            if (tMP_Text == null)
            {
                tMP_Text = GetComponent<TMP_Text>();
            }
            return tMP_Text;
        }
    }

    [Button]
    public void SetText()
    {
        SetLanguage(GameManager.Language);
    }

    private void SetLanguage(string language)
    {
        if (!GameManager.LocalizedStrings.ContainsKey(key))
        {
            Debug.LogWarning($"Can't find {key} key in LocalizedStrings!");
            TMP_Text.text = "null";
            return;
        }
        else if (!GameManager.LocalizedStrings[key].ContainsKey(language))
        {
            Debug.LogWarning($"Can't find {key} key translation to {language} language in LocalizedStrings! Will use en language");
            TMP_Text.text = GameManager.LocalizedStrings[key]["en"];
            return;
        }
        else
        {
            TMP_Text.text = GameManager.LocalizedStrings[key][language];
        }
    }

    private void Start()
    {
        SetText();
    }
}
