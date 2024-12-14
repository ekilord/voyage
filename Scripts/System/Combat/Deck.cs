using System;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using Newtonsoft.Json;
using System.Linq;

public class Deck
{
    [JsonProperty]
    private string Name;
    private List<Card> PermanentDeck;
    private List<Card> TemporaryDeck;

    public Deck()
    {
        Name = "";
        PermanentDeck = new List<Card>();
        TemporaryDeck = new List<Card>();
    }

    public Deck(string name, List<Card> cards)
    {
        Name = name;
        PermanentDeck = cards;

        CopyDeck();
    }

    private void CopyDeck()
    {
        List<Card> tempCards = new();
        tempCards.AddRange(PermanentDeck);

        TemporaryDeck = tempCards;
    }

    public void Shuffle()
    {
        System.Random rng = new();

        int n = TemporaryDeck.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (TemporaryDeck[j], TemporaryDeck[i]) = (TemporaryDeck[i], TemporaryDeck[j]);
        }
    }

    public void ShuffleCardInto(Card card)
    {
        TemporaryDeck.Add(card);
        Shuffle();
    }

    public Card DrawCard()
    {
        if (TemporaryDeck.Count == 0)
        {
            CopyDeck();
            Shuffle();
        }
        Card tempCard = TemporaryDeck[0];
        TemporaryDeck.RemoveAt(0);

        return tempCard;
    }

    public void ResetDeck()
    {
        TemporaryDeck.Clear();
        TemporaryDeck.AddRange(GetPermanentDeck());
    }

    public string GetName()
    {
        return Name;
    }

    public List<Card> GetPermanentDeck()
    {
        return PermanentDeck;
    }

    public List<Card> GetTemporaryDeck()
    {
        return TemporaryDeck;
    }

    public static class Decks
    {
        public static Deck MergeDecks(HashSet<Deck> decks)
        {
            List<Card> cards = new List<Card>();
            foreach (Deck deck in decks)
            {
                foreach (Card card in deck.GetPermanentDeck())
                {
                    if (!cards.Contains(card))
                    {
                        cards.Add(card);
                    }
                }
            }

            return new Deck("Player's Deck", cards);
        }

        public static readonly Deck WOODCUTTERS = new("Woodcutter's Deck", new List<Card>
        {
            Card.Cards.AXE_SWING,
            Card.Cards.CHOP,
            Card.Cards.CLEAVE,
            Card.Cards.HARDENED_SKIN,
            Card.Cards.BANDAGE,
            Card.Cards.REST,
            Card.Cards.INTIMIDATE,
            Card.Cards.BARK_ARMOR,
            Card.Cards.ROAR
        });

        public static readonly Deck STONEMASONS = new("Stonemason's Deck", new List<Card>
        {
            Card.Cards.ROCK_THROW,
            Card.Cards.HAMMER_STRIKE,
            Card.Cards.BOULDER_CRUSH,
            Card.Cards.STONE_SKIN,
            Card.Cards.MEND,
            Card.Cards.REINFORCE,
            Card.Cards.QUAKE,
            Card.Cards.FORTIFY,
            Card.Cards.RUMBLE
        });

        public static readonly Deck PLUNDERERS = new("Plunderer's Deck", new List<Card>
        {
            Card.Cards.SLASH,
            Card.Cards.AMBUSH,
            Card.Cards.RANSACK,
            Card.Cards.BATTLE_CRY,
            Card.Cards.SHIELD_UP,
            Card.Cards.RESTORATION,
            Card.Cards.DISORIENT,
            Card.Cards.BLINDING_POWDER,
            Card.Cards.BERSERK
        });

        public static readonly Deck MINERS = new("Miner's Deck", new List<Card>
        {
            Card.Cards.PICKAXE_STRIKE,
            Card.Cards.TUNNEL_COLLAPSE,
            Card.Cards.RUSTY_BLADE,
            Card.Cards.IRON_SKIN,
            Card.Cards.FIRST_AID,
            Card.Cards.DEFENSIVE_STANCE,
            Card.Cards.BATTLECRY,
            Card.Cards.HARDENED_WILL,
            Card.Cards.CHALLENGE
        });

        public static readonly Deck ARCANISTS = new("Arcanist's Deck", new List<Card>
        {
            Card.Cards.FIREBALL,
            Card.Cards.ICE_SPIKE,
            Card.Cards.ARCANE_BOLT,
            Card.Cards.MANA_SHIELD,
            Card.Cards.HEALING_AURA,
            Card.Cards.MAGIC_BARRIER,
            Card.Cards.WEAKEN,
            Card.Cards.ENFEEBLE,
            Card.Cards.BLESSING
        });

        public static readonly Deck BASE = new("Base Deck", new List<Card>
        {
            Card.Cards.QUICK_STRIKE,
            Card.Cards.CRUDE_SLASH,
            Card.Cards.HEAVY_SWING,
            Card.Cards.SELF_BANDAGE,
            Card.Cards.GUARD,
            Card.Cards.FOCUS,
            Card.Cards.DISTRACT,
            Card.Cards.TAUNT,
            Card.Cards.HINDER
        });

        public static readonly Deck GHOULS = new("Ghouls' Deck", new List<Card>
    {
        Card.Cards.ROTTEN_BITE,
        Card.Cards.DISEASED_STRIKE,
        Card.Cards.GRAVE_TOUCH,
        Card.Cards.REANIMATE,
        Card.Cards.DREAD_AURA,
        Card.Cards.SHADOW_SHROUD,
        Card.Cards.LIFE_DRAIN
    });

        public static readonly Deck GOBLINS = new("Goblins' Deck", new List<Card>
        {
        Card.Cards.SLASH,
        Card.Cards.AMBUSH,
        Card.Cards.DISORIENT,
        Card.Cards.RANSACK,
        Card.Cards.BATTLE_CRY,
        Card.Cards.SHIELD_UP,
        Card.Cards.BLINDING_POWDER
    });

        public static readonly Deck SKELETONS = new("Skeletons' Deck", new List<Card>
        {
        Card.Cards.BONE_CLUB,
        Card.Cards.ROTTEN_BITE,
        Card.Cards.DISEASED_STRIKE,
        Card.Cards.GRAVE_TOUCH,
        Card.Cards.FORTIFY,
        Card.Cards.QUAKE,
        Card.Cards.RUMBLE
    });

        public static readonly Deck GHOSTS = new("Ghosts' Deck", new List<Card>
        {
        Card.Cards.MIND_SPIKE,
        Card.Cards.CURSED_GAZE,
        Card.Cards.LIFE_DRAIN,
        Card.Cards.SHADOW_SHROUD,
        Card.Cards.VOID_GAZE,
        Card.Cards.PHASE_SHIFT,
        Card.Cards.STAR_INSIGHT
    });

        public static readonly Deck WOLF_PACK = new("Wolf Pack's Deck", new List<Card>
        {
        Card.Cards.FANGS,
        Card.Cards.CLAW_SWIPE,
        Card.Cards.PACK_HUNT,
        Card.Cards.HOWL,
        Card.Cards.LICK_WOUNDS,
        Card.Cards.PACK_FORMATION,
        Card.Cards.INTIMIDATE_HOWL
    });

        public static readonly Deck FEROCIOUS_BEAR = new("Ferocius Bear's Deck", new List<Card>
        {
        Card.Cards.FANGS,
        Card.Cards.CLAW_SWIPE,
        Card.Cards.HEAVY_SWING,
        Card.Cards.ROAR,
        Card.Cards.HARDENED_SKIN,
        Card.Cards.REST,
        Card.Cards.BERSERK
    });

        public static List<Deck> GetAllDecks()
        {
            return new List<Deck>()
            {
                PLUNDERERS,
                WOODCUTTERS,
                STONEMASONS,
                MINERS,
                ARCANISTS,
                GHOULS,
                GOBLINS,
                SKELETONS,
                GHOSTS,
                WOLF_PACK,
                FEROCIOUS_BEAR
            };
        }

        public static Deck GetDeckByName(string name)
        {
            foreach (Deck deck in GetAllDecks())
            {
                if (deck.GetName() == name) return deck;
            }

            return PLUNDERERS;
        }
    }
}

