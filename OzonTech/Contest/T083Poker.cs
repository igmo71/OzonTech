using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzonTech.Contest
{
    internal class T083Poker
    {
        internal static void Run(string[] args)
        {

            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            var cardsDeck = Card.GetDeck();

            List<int[]> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                int playersCount = int.Parse(Console.ReadLine()!);
                List<Player> playerList = new List<Player>();
                for (int j = 0; j < playersCount; j++)
                {
                    string[] playerCards = Console.ReadLine()!.Split(' ');
                    
                    var player = new Player()
                    {
                        Card1 = new()
                        {
                            Rank = new() { Key = playerCards[0][0], Value = Card.Ranks[playerCards[0][0]] },
                            Suite = new() { Key = playerCards[0][1], Value = Card.Suites[playerCards[0][1]] }
                        },
                        Card2 = new()
                        {
                            Rank = new() { Key = playerCards[1][0], Value = Card.Ranks[playerCards[1][0]] },
                            Suite = new() { Key = playerCards[1][1], Value = Card.Suites[playerCards[1][1]] }
                        }
                    };

                    cardsDeck.RemoveAt(player.Card1.)
                }
            }

        }

    }

    internal class Card
    {

        internal static Dictionary<char, int> Ranks = new()
        {
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['T'] = 10,
            ['J'] = 11,
            ['Q'] = 12,
            ['K'] = 13,
            ['A'] = 14,
        };

        internal static Dictionary<char, int> Suites = new()
        {
            ['S'] = 1,
            ['C'] = 2,
            ['D'] = 3,
            ['H'] = 4,
        };

        public required Rank Rank { get; set; }
        public required Suite Suite { get; set; }

        public static List<Card> GetDeck()
        {
            List<Card> deck = new List<Card>();
            foreach (var suite in Suites)
            {
                foreach (var rank in Ranks)
                {
                    deck.Add(new Card()
                    {
                        Rank = new() { Key = rank.Key, Value = rank.Value },
                        Suite = new() { Key = suite.Key, Value = suite.Value }
                    });
                }
            }
            return deck;
        }
    }

    

    internal class Suite
    {
        public required char Key { get; set; }
        public int Value { get; set; }
    }

    internal class Rank
    {
        public required char Key { get; set; }
        public int Value { get; set; }
    }

    internal class Player
    {
        internal required Card Card1 { get; set; }
        internal required Card Card2 { get; set; }
        public Combination Combination => Card1.Rank.Value == Card2.Rank.Value ? Combination.Pair : Combination.HighCard;
        public int PairRank => Card1.Rank.Value;
        public int HighCardRank => Card1.Rank.Value > Card2.Rank.Value ? Card1.Rank.Value : Card2.Rank.Value;
    }

    enum Combination
    {
        HighCard, Pair, Set
    }
}
