using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class User : Base
    {

        private string? _name;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string? _imagePath;

        public string? ImagePath 
        { 
            get => _imagePath;
            set {
                _imagePath = value;
                NotifyPropertyChanged(nameof(ImagePath));
            } 
        }

        private int? _wins;

        public int? Wins
        {
            get => _wins;
            set
            {
                _wins = value;
                NotifyPropertyChanged(nameof(Wins));
            }
        }

        private int? _playedGames;

        public int? PlayedGames
        {
            get => _playedGames;
            set
            {
                _playedGames = value;
                NotifyPropertyChanged(nameof(PlayedGames));
            }
        }


    }
}
