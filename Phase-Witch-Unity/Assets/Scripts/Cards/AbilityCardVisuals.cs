using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardVisuals : MonoBehaviour
{
    [SerializeField]
    GameObject AbilityDesc;
    [SerializeField]
    GameObject Value;
    Image AbilityImage;
    bool SetUp;

    private void Awake()
    {
        AbilityImage = GetComponent<Image>();
    }

    public void InitialiseAbility(Ability ability, bool isLight)
    {
        var abilityType = CardDatabase.GetAbilityFromType(ability.AbilityType);
        var light = abilityType.LightSideData;
        var dark = abilityType.DarkSideData;

        string spritePath = isLight ? light.AbilitySpritePath : dark.AbilitySpritePath;
        AbilityImage = Resources.Load(spritePath) as Image;

        int modifier = isLight ? ability.LightModifier : ability.DarkModifier;

        AbilityDesc.GetComponentInChildren<TextMeshProUGUI>().text = AbilityDescription(abilityType, isLight, modifier);

        if (modifier == 0)
        {
            Value.SetActive(false);
        }
        else
        {
            Value.SetActive(true);
            var tmp = Value.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = modifier.ToString();
        }

        SetUp = true;
        OnLoseFocus();
    }

    string AbilityDescription(AbilityBaseData ability, bool isLight, int mod)
    {
        string output;

        if (isLight)
        {
            var light = ability.LightSideData;
            output = $"{light.AbilityName}:  {light.AbilityDesc}";

        }
        else
        {
            var dark = ability.DarkSideData;
            output = $"{dark.AbilityName}:  {dark.AbilityDesc}";
        }

        for (int i = 0; i < output.Length; ++i)
        {
            if (output[i] == '@')
            {
                string start = i > 0 ? output.Substring(0, i) : "";
                string replacement = mod.ToString();
                string end = i < output.Length - 1 ? output.Substring(i + 1) : "";
                output = start + replacement + end;
            }
        }
        //output.Replace('@', mod.ToString()[0]);


        return output;
    }

    public void OnFocus()
    {
        if (!SetUp) return;
        AbilityDesc.SetActive(true);
    }

    public void OnLoseFocus()
    {
        AbilityDesc.SetActive(false);
    }

    private void OnDisable()
    {
        AbilityDesc.SetActive(false);
    }

    private void OnEnable()
    {
        AbilityDesc.SetActive(true);
    }

}
