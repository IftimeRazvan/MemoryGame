using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    public class MenuVM : BaseVM
    {
        public MenuCommands? MenuCommands { get; set; }

        public ObservableCollection<Size> PredefinedSizes { get; set; }
        public MenuVM(User? _user) {
            user = _user;
            MenuCommands = new MenuCommands(this);

            PredefinedSizes = new ObservableCollection<Size>
            {
                new Size(2, 4),
                new Size(4, 2),
                new Size(3, 6),
                new Size(6, 3),
                new Size(5, 2),
                new Size(2, 6),
                new Size(6, 6),
                new Size(4, 5),
                new Size(5, 4)
            };
        }

        public MenuVM()
        {
            MenuCommands = new MenuCommands(this);

            PredefinedSizes = new ObservableCollection<Size>
            {
                new Size(2, 4),
                new Size(4, 2),
                new Size(3, 6),
                new Size(6, 3),
                new Size(5, 2),
                new Size(2, 6),
                new Size(6, 6),
                new Size(4, 5),
                new Size(5, 4)
            };
        }

        public User user { get; set; }

        private string? _selectedCategory;
        public string? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged(nameof(SelectedCategory));
            }
        }


        private int _boardWidth;
        public int BoardWidth
        {
            get => _boardWidth;
            set
            {
                _boardWidth = value;
                NotifyPropertyChanged(nameof(BoardWidth));
            }
        }

        private int _boardHeight;
        public int BoardHeight
        {
            get => _boardHeight;
            set
            {
                _boardHeight = value;
                NotifyPropertyChanged(nameof(BoardHeight));
            }
        }



    }
}
