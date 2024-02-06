namespace OzonTech.Contest
{
    internal class T083Poker
    {
        internal static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int gamesCount))
                return;

            List<Game> games = new();

            for (int i = 0; i < gamesCount; i++)
            {
                Game game = new();

                int playersCount = int.Parse(Console.ReadLine()!);

                List<Player> players = new();

                for (int j = 0; j < playersCount; j++)
                {
                    var cards = Console.ReadLine()!.Split(' ').ToList();

                    game.Players.Add(new() { Cards = cards });

                    //game.CardDeck.RemoveAll(cd => cards.Any(c => c == cd));
                    cards.ForEach(c => game.CardDeck.Remove(c));
                }
                games.Add(game);
            }

            List<Card> resultDeck = new List<Card>();
            foreach (var game in games)
            {
                game
                    .Process()
                    .PrintResultCardDeck();
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

        internal static int Rank(string card)
        {
            return Ranks.First(r => r.Key == card[0]).Value;
        }

        internal static Dictionary<char, int> Suites = new()
        {
            ['S'] = 1,
            ['C'] = 2,
            ['D'] = 3,
            ['H'] = 4,
        };

        internal static List<string> GetDeck()
        {
            List<string> result = new List<string>();
            foreach (var rank in Ranks)
            {
                foreach (var suite in Suites)
                {
                    result.Add($"{rank.Key}{suite.Key}");
                }
            }
            return result;
        }
    }

    internal class Game
    {
        public Game()
        {
            CardDeck = Card.GetDeck();
            ResultCardDeck = new();
            Players = new();
        }
        public List<Player> Players { get; set; }
        public List<string> CardDeck { get; set; }
        public List<string> ResultCardDeck { get; set; }

        internal void PrintResultCardDeck()
        {
            Console.WriteLine(ResultCardDeck.Count);
            ResultCardDeck.ForEach(c => Console.WriteLine(c));
        }

        internal Game Process()
        {
            var me = Players.First();
            var others = Players.GetRange(1, Players.Count - 1);

            foreach (var card in CardDeck)
            {
                me.Cards.Add(card);

                others.ForEach(p => p.Cards.Add(card));
                others.Sort();
                var winner = others.Last();

                if (me.CompareTo(winner) >= 0)
                    ResultCardDeck.Add(card);

                others.ForEach(p => p.Cards.Remove(card));

                me.Cards.Remove(card);
            }
            return this;
        }
    }

    internal class Player : IComparable<Player>
    {
        public required List<string> Cards { get; set; }
        public Combination Combination
        {
            get
            {
                var result = Combination.HighCard;

                if (Card.Rank(Cards[0]) == Card.Rank(Cards[1]) ||
                    Card.Rank(Cards[1]) == Card.Rank(Cards[2]) ||
                    Card.Rank(Cards[2]) == Card.Rank(Cards[0]))
                    result = Combination.Pair;

                if (Card.Rank(Cards[0]) == Card.Rank(Cards[1]) &&
                    Card.Rank(Cards[1]) == Card.Rank(Cards[2]))
                    result = Combination.CardsSet;

                return result;
            }
        }

        public int CardsSetRank => Card.Rank(Cards[0]);

        public int PairRank
        {
            get
            {
                if (Card.Rank(Cards[0]) == Card.Rank(Cards[1]))
                    return Card.Rank(Cards[0]);

                if (Card.Rank(Cards[1]) == Card.Rank(Cards[2]))
                    return Card.Rank(Cards[1]);

                if (Card.Rank(Cards[2]) == Card.Rank(Cards[0]))
                    return Card.Rank(Cards[2]);

                return 0;
            }
        }

        public int HighCardRank
        {
            get
            {
                var result = Card.Rank(Cards[0]) > Card.Rank(Cards[1]) ? Card.Rank(Cards[0]) : Card.Rank(Cards[1]);

                if (Cards.Count == 2)
                    return result;

                if (Cards.Count == 3)
                    result = result > Card.Rank(Cards[2]) ? result : Card.Rank(Cards[2]);

                return result;
            }
        }

        public int CompareTo(Player? other)
        {
            var result = this.Combination - other!.Combination;
            if (result != 0)
                return result;

            if (Combination == Combination.CardsSet)
                return CardsSetRank - other.CardsSetRank;

            if (Combination == Combination.Pair)
                return PairRank - other.PairRank;

            if (Combination == Combination.HighCard)
                return HighCardRank - other.HighCardRank;
            return 0;
        }
    }

    internal enum Combination
    {
        HighCard, Pair, CardsSet
    }
}
