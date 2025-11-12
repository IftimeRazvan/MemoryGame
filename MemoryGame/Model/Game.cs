using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryGame.Model
{
    public class Game : Base
    {
        public ObservableCollection<Card> Cards { get; set; }
        private int _timeRemaining { get; set; }
        private string _category { get; set; }
        private int _boardWidth { get; set; }
        private int _boardHeight { get; set; }

        public int TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                NotifyPropertyChanged(nameof(TimeRemaining));
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                NotifyPropertyChanged(nameof(Category));
            }
        }

        public int BoardWidth
        {
            get => _boardWidth;
            set
            {
                _boardWidth = value;
                NotifyPropertyChanged(nameof(BoardWidth));
            }
        }

        public int BoardHeight
        {
            get => _boardHeight;
            set
            {
                _boardHeight = value;
                NotifyPropertyChanged(nameof(BoardHeight));
            }
        }


        public Game(string category,int boardWidth,int boardHeight,int time)
        {
            Category = category;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            TimeRemaining = time;
            Cards = new ObservableCollection<Card>();
        }

        public Game()
        {
            Cards = new ObservableCollection<Card>();
        }
    }



    public class Card : Base
    {
        private bool _isFlipped;
        private bool _isMatched;

        public string ImagePath { get; set; }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                NotifyPropertyChanged(nameof(IsFlipped));
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                NotifyPropertyChanged(nameof(IsMatched));
            }
        }
    }
}


