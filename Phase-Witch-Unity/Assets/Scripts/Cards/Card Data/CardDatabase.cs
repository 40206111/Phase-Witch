using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CardDataLists
{
    public static int NextID = 0;

    public List<CardBaseData> AllCards = new List<CardBaseData>();

    public List<AbilityBaseData> AllAbilities = new List<AbilityBaseData>();
}

public static class CardDatabase
{
    static CardDataLists DataLists;

    public static void PopulateLists(CardDataLists theLists)
    {
        DataLists = theLists;
    }

    public static List<CardBaseData> GetAllPlayerCards()
    {
        List<CardBaseData> output = new List<CardBaseData>();
        foreach (var card in DataLists.AllCards)
        {
            if (!card.EnemyCard)
            {
                output.Add(card);
            }
        }
        return output;
    }


    public static List<CardBaseData> GetAllEnemyCards()
    {
        return (List<CardBaseData>)from card in DataLists.AllCards where card.EnemyCard select card;
    }


    public static List<CardBaseData> GetAllSpellCards()
    {
        return (List<CardBaseData>)from card in DataLists.AllCards where !(card is UnitCardData) select card;
    }

    public static List<CardBaseData> GetAllUnitCards()
    {
        return (List<CardBaseData>)from card in DataLists.AllCards where (card is UnitCardData) select card;
    }

    public static AbilityBaseData GetAbilityFromType(eAbilityType type)
    {
        foreach (var ability in DataLists.AllAbilities)
        {
            if (ability.AbilityType == type)
            {
                return ability;
            }
        }
        return null;
    }


    public static List<CardBaseData> GetCards(int number)
    {
        var getAllCards = GetAllPlayerCards();

        Random rnd = new Random();

        List<CardBaseData> output = new List<CardBaseData>();

        for (int i = 0; i < number; ++i)
        {

            int randomNumber = rnd.Next(0, getAllCards.Count);
            output.Add(getAllCards[randomNumber]);
        }

        return output;

    }

}
