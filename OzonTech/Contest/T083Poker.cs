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
                var game = new Game();
                int playersCount = int.Parse(Console.ReadLine()!);

                List<Player> players = new List<Player>();
                for (int j = 0; j < playersCount; j++)
                {
                    var cards = Console.ReadLine()!.Split(' ').ToList();
                    game.Players.Add(new() { Cards = cards });
                    cards.ForEach(c => game.CardDeck.Remove(c));
                }
                games.Add(game);
            }

            foreach (var games in games)
            {
                var deck = Card.GetDeck();
                for (int i = 1; i < games.Count; i++)
                {
                    //if (players[0].CompareTo(players[i]) >= 0)
                    //    Console.WriteLine("Ok");
                    //else
                    //    Console.WriteLine("MMM");
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
            Players = new();
        }
        public List<Player> Players { get; set; }
        public List<string> CardDeck { get; set; }
    }


    internal class Player : IComparable<Player>
    {
        public required List<string> Cards { get; set; }
        public Combination Combination
        {
            get
            {
                var result = Combination.HighCard;
                if (Cards.Count == 2 && Cards[0][0] == Cards[1][0])
                    result = Combination.Pair;
                if (Cards.Count == 3 && Cards[0][0] == Cards[1][0] && Cards[1][0] == Cards[2][0])
                    result = Combination.Set;
                return result;
            }
        }

        public int SetRank => Card.Ranks[Cards[0][0]];
        public int PairRank => Card.Ranks[Cards[0][0]];
        public int HighCardRank
        {
            get
            {
                var result = Card.Ranks[Cards[0][0]] > Card.Ranks[Cards[1][0]] ? Card.Ranks[Cards[0][0]] : Card.Ranks[Cards[1][0]];
                if (Cards.Count == 2)
                    return result;
                if (Cards.Count == 3)
                    result = result > Card.Ranks[Cards[2][0]] ? result : Card.Ranks[Cards[2][0]];
                return result;
            }
        }

        public int CompareTo(Player? other)
        {
            var result = this.Combination - other.Combination;
            if (result != 0)
                return result;

            if (Combination == Combination.Set)
                return SetRank - other.SetRank;

            if (Combination == Combination.Pair)
                return PairRank - other.PairRank;

            if (Combination == Combination.HighCard)
                return HighCardRank - other.HighCardRank;
            return 0;
        }
    }

    internal enum Combination
    {
        HighCard, Pair, Set
    }
}
