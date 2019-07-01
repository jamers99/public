using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeNames.Logic
{
    public class Game : ReadOnlyObservableCollection<Card>
    {
        private const int FirstTeamCardCount = 9;
        private const int SecondTeamCardCount = 8;

        public Game(int boardSize = 5) : base(new ObservableCollection<Card>())
        {
            BoardSize = boardSize;
            FirstTeam = GetFirstTeam();
            SecondTeam = GetSecondTeam();

            AddCards();
        }

        public bool IsPreviewing
        {
            set
            {
                foreach (var card in Items)
                {
                    card.IsPreviewing = value;
                }
            }
        }

        public int BoardSize { get; }

        CardType FirstTeam { get; }

        CardType SecondTeam { get; }

        CardType GetFirstTeam() => new Random().Next(1) == 0 ? CardType.TeamBlue : CardType.TeamRed;

        CardType GetSecondTeam() => FirstTeam == CardType.TeamBlue ? CardType.TeamRed : CardType.TeamBlue;

        void AddCards()
        {
            Items.Clear();

            var numberOfCards = BoardSize * BoardSize;
            var random = new Randomizer(numberOfCards - 1);

            var types = new Dictionary<int, CardType>
            {
                {
                    random.Next(), CardType.EndGame
                }
            };
            for (int i = 0; i < FirstTeamCardCount; i++)
            {
                types.Add(random.Next(), FirstTeam);
            }
            for (int i = 0; i < SecondTeamCardCount; i++)
            {
                types.Add(random.Next(), SecondTeam);
            }

            var words = Dictionary.GetAllWords()
                .Select(w => ((Guid guid, string word))(Guid.NewGuid(), w))
                .OrderBy(w => w.guid)
                .Take(numberOfCards)
                .Select(w => w.word);

            foreach (var word in words)
            {
                types.TryGetValue(Items.Count, out CardType type);
                Items.Add(new Card(word, type));
            }
        }
    }
}
