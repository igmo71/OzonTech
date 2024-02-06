namespace OzonTech.Contest
{
    internal class T083Poker2
    {
        internal static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int gamesCount))
                return;

            Game[] games = new Game[gamesCount];

            for (int i = 0; i < gamesCount; i++)
            {

                int playersCount = int.Parse(Console.ReadLine()!);

                Game game = new(playersCount);

                Player[] players = new Player[playersCount];

                for (int j = 0; j < playersCount; j++)
                {
                    string[] cards = Console.ReadLine()!.Split(' ');

                    Player player = new(cards);

                    game.Players[j] = player;
                }
                games[i] = game;
            }

            foreach (var game in games)
            {
                game.Process();
                game.PrintResultCardDeck();
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

            internal static int Rank(string card)
            {
                return Ranks[card[0]];
            }

            internal static string[] GetDeck(Player[] players)
            {
                string[] deck = new string[52 - players.Length * 2];
                int i = 0;
                foreach (var rank in Ranks)
                {
                    foreach (var suite in Suites)
                    {
                        string card = $"{rank.Key}{suite.Key}";

                        if (CheckCard(card, players))
                        {
                            deck[i] = card;
                            i++;
                        }
                    }
                }
                return deck;
            }

            private static bool CheckCard(string card, Player[] players)
            {
                foreach (var player in players)
                    if (Array.Exists(player.Cards, c => c == card))
                        return false;

                return true;
            }
        }

        internal class Game
        {
            public Game(int playersCount)
            {
                Players = new Player[playersCount];
                ResultCardDeck = new();
            }

            public Player[] Players { get; set; }

            public List<string> ResultCardDeck { get; set; }

            internal void PrintResultCardDeck()
            {
                Console.WriteLine(ResultCardDeck.Count);
                ResultCardDeck.ForEach(c => Console.WriteLine(c));
            }

            internal void Process()
            {
                Player me = Players[0];
                Player[] otherPlayers = new Player[Players.Length - 1];
                Array.Copy(Players, 1, otherPlayers, 0, Players.Length - 1);

                foreach (var card in Card.GetDeck(Players))
                {
                    me.Cards[2] = card;

                    Array.ForEach(otherPlayers, p => p.Cards[2] = card);

                    Array.Sort(otherPlayers);
                    var winner = otherPlayers.Last();

                    if (me.CompareTo(winner) >= 0)
                        ResultCardDeck.Add(card);
                }
            }
        }

        internal class Player : IComparable<Player>
        {
            public Player(string[] cards)
            {
                Cards = new string[3];
                Cards[0] = cards[0];
                Cards[1] = cards[1];
            }

            public string[] Cards { get; set; }

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

                    result = result > Card.Rank(Cards[2]) ? result : Card.Rank(Cards[2]);

                    return result;
                }
            }

            public int CompareTo(Player? other)
            {
                var result = this.Combination - other!.Combination;
                if (result != 0)
                    return result;

                if (Combination == Combination.HighCard)
                    return HighCardRank - other.HighCardRank;

                if (Combination == Combination.Pair)
                    return PairRank - other.PairRank;

                if (Combination == Combination.CardsSet)
                    return CardsSetRank - other.CardsSetRank;
                return 0;
            }
        }

        internal enum Combination
        {
            HighCard, Pair, CardsSet
        }
    }
}
