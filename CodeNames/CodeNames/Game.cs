using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace CodeNames
{
    public class Game : ReadOnlyObservableCollection<Card>
    {
        private const int FirstTeamCardCount = 9;
        private const int SecondTeamCardCount = 8;

        public Game(int number, int boardSize = 5) : base(new ObservableCollection<Card>())
        {
            Number = number;
            BoardSize = boardSize;
            FirstTeam = GetFirstTeam();
            SecondTeam = GetSecondTeam();

            Dictionary.CollectionChanged += Dictionary_CollectionChanged;
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

        public WordDictionary Dictionary { get; } = new WordDictionary();

        public int Number { get; }

        public int BoardSize { get; }

        CardType FirstTeam { get; }

        CardType SecondTeam { get; }

        CardType GetFirstTeam() => new Random().Next(1) == 0 ? CardType.TeamBlue : CardType.TeamRed;

        CardType GetSecondTeam() => FirstTeam == CardType.TeamBlue ? CardType.TeamRed : CardType.TeamBlue;

        #region Card count

        int GetNumberOfCards() => BoardSize * BoardSize;

        public bool HasEnoughWords => Dictionary.Count >= GetNumberOfCards();

        void Dictionary_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasEnoughWords)));
        }

        #endregion

        #region Card generation

        public bool AddCards()
        {
            if (!HasEnoughWords)
                return false;

            Items.Clear();
            var numberOfCards = GetNumberOfCards();
            var typeMap = GetCardTypeMap(numberOfCards);
            var words = Dictionary
                .Select(w => ((Guid guid, string word))(Guid.NewGuid(), w.Text))
                .OrderBy(w => w.guid)
                .Take(numberOfCards)
                .Select(w => w.word);

            foreach (var word in words)
            {
                typeMap.TryGetValue(Items.Count, out CardType type);
                Items.Add(new Card(word, type));
            }

            UpdateRows();

            return true;
        }

        Dictionary<int, CardType> GetCardTypeMap(int numberOfCards)
        {
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

            return types;
        }

        #endregion

        #region Rows

        public ObservableCollection<List<Card>> RowsOfCards { get; } = new ObservableCollection<List<Card>>();

        void UpdateRows()
        {
            RowsOfCards.Clear();
            for (int i = 0; i < BoardSize; i++)
            {
                var rowBegin = i * BoardSize;
                var row = Items.Skip(rowBegin)
                               .Take(BoardSize)
                               .ToList();
                RowsOfCards.Add(row);
            }
        }

        #endregion
    }
}
