using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField]
    List<AbilityCardVisuals> DarkAbilities;
    [SerializeField]
    List<AbilityCardVisuals> LightAbilities;
    [SerializeField]
    Image CardImage;
    [SerializeField]
    GameObject Health;
    [SerializeField]
    GameObject Damage;

    [SerializeField]
    Animator Animator;

    private CardBaseData _card;
    public CardBaseData Card { get { return _card; } }
    public bool IsLight = true;

    bool Focused = false;

    public void flip()
    {
        IsLight = !IsLight;
        Focused = false;
        Animator.SetTrigger("Flip");
    }

    void ShowAbilityDescs(bool show)
    {
        foreach (var ability in DarkAbilities)
        {
            if (show && !IsLight)
            {
                ability.OnFocus();
            }
            else
            {
                ability.OnLoseFocus();
            }
        }
        foreach (var ability in LightAbilities)
        {
            if (show && IsLight)
            {
                ability.OnFocus();
            }
            else
            {
                ability.OnLoseFocus();
            }
        }
    }

    public void OnFocus(bool focus)
    {
        if (Focused == focus) return;

        ShowAbilityDescs(focus);

        Animator.SetBool("Hover", focus);

        Focused = focus;
    }

    public void ShowCardData(CardBaseData card)
    {
        _card = card;
        for (int i = 0; i < LightAbilities.Count; ++i)
        {
            if (card.Abilities.Count > i)
            {
                DarkAbilities[i].InitialiseAbility(card.Abilities[i], false);
                LightAbilities[i].InitialiseAbility(card.Abilities[i], true);
            }
            else
            {
                DarkAbilities[i].gameObject.SetActive(false);
                LightAbilities[i].gameObject.SetActive(false);
            }
        }

        CardImage = Resources.Load(card.CardSpritePath) as Image;

        if (card is UnitCardData)
        {
            var unit = card as UnitCardData;

            Health.SetActive(true);
            Damage.SetActive(true);
            Health.GetComponentInChildren<TextMeshProUGUI>().text = unit.Health.ToString();
            Damage.GetComponentInChildren<TextMeshProUGUI>().text = unit.Damage.ToString();
        }
        else
        {
            Health.SetActive(false);
            Damage.SetActive(false);
        }
    }



}
