using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Unit
{
    public string Name;
    public int Health;
    public int Resources;
    private int currentResources;
    public string ASCII;
    public int HandSize;

    public List<Card> Cards = new List<Card>();
    public List<Card> Deck = new List<Card>();
    public List<Card> Hand = new List<Card>();
    public List<Card> Discard = new List<Card>();
    private bool _isDead;
    public bool IsDead => _isDead;

    public event Action<Unit> OnUnitChanged;
    public event Action<Unit> OnUnitDead;
    public event Action<Card> OnCardUsed;
    
    public event Action<Unit> OnHandChanged;

    public int CurrentResources
    {
        get => currentResources;
        set
        {
            currentResources = value;
            OnUnitChanged?.Invoke(this);
        }
    }


    public void Attack(Unit other, AttackCard card)
    {
        other.ApplyCard(this, card);
    }

    public void ApplyCard(Unit other, AttackCard card)
    {
        Health -= card.damage;
        if (Health <= 0)
        {
            _isDead = true;
            OnUnitDead?.Invoke(this);
        }
        OnUnitChanged?.Invoke(this);
    }

    public void ShuffleAll()
    {
        var cardsToSuffle = new List<Card>(Cards);
        Discard.Clear();
        Deck.Clear();
        Hand.Clear();

        int count = cardsToSuffle.Count;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0,count - i);
            Deck.Add(cardsToSuffle[randomIndex]);
            cardsToSuffle.RemoveAt(randomIndex);
        }
        OnHandChanged?.Invoke(this);
    }
    
    public void Shuffle()
    {
        var cardsToShuffle = new List<Card>(Deck);
        cardsToShuffle.AddRange(Discard);
        Discard.Clear();
        Deck.Clear();

        int count = cardsToShuffle.Count;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0,count - i);
            Deck.Add(cardsToShuffle[randomIndex]);
            cardsToShuffle.RemoveAt(randomIndex);
        }
        
        OnHandChanged?.Invoke(this);
    }

    public void Draw(int cantCards)
    {
        while (cantCards > 0)
        {
            int deckCount = Deck.Count;
            if (deckCount > 0)
            {
                int randomIndex = Random.Range(0, deckCount);
                Card card = Deck[randomIndex];
                Hand.Add(card);
                Deck.RemoveAt(randomIndex);
                cantCards--;
                OnHandChanged?.Invoke(this);
            }
            else if(Discard.Count>0)
            {
                Shuffle();   
            }
            else
            {
                return;
            }

        }
        

    }

    public void UseCard(Unit unit, Card card)
    {
        if (!Hand.Contains(card) || !card.TryUse(this, unit)) return;
        Hand.Remove(card);
        Discard.Add(card);
        
        OnCardUsed?.Invoke(card);
        OnHandChanged?.Invoke(this);
    }

    public void DiscardHand()
    {
        Discard.AddRange(Hand);
        Hand.Clear();
        OnHandChanged?.Invoke(this);
    }
}