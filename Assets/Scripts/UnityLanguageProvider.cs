using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLanguageProvider : ILanguageProvider
{
    private string defaultLanguage;

    public UnityLanguageProvider(string defaultLanguage)
    {
        this.defaultLanguage = defaultLanguage;
    }

    public string GetLanguage()
    {
        string lang = defaultLanguage;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                lang = "ru";
                break;
            case SystemLanguage.Portuguese:
                lang = "pt";
                break;
            case SystemLanguage.Spanish:
                lang = "es";
                break;
            case SystemLanguage.German:
                lang = "de";
                break;
            case SystemLanguage.French:
                lang = "fr";
                break;
            case SystemLanguage.Italian:
                lang = "it";
                break;
        }

        return lang;
    }
}
