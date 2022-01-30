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

    public void flip()
    {
        Animator.SetTrigger("Flip");
    }

    public void ShowCardData(CardBaseData card)
    {

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
