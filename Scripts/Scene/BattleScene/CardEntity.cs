using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEntity : MonoBehaviour
{
    private int Index;
    private Card Card;

    public int GetIndex()
    { 
        return Index; 
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public Card GetCard()
    {
        return Card;
    }

    public void SetCard(Card card)
    {
        Card = card;
    }

}
