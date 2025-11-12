using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using MemoryGame.View;
using Newtonsoft.Json;
using MemoryGame.Model;
using System.IO;

namespace MemoryGame.ViewModel.Commands
{
    public class MenuCommands : BaseVM
    {
        private readonly MenuVM _menuVM;

        public MenuCommands(MenuVM menuVM)
        {
            _menuVM = menuVM;
        }


        private ICommand? _updateCategoryCommand;
        public ICommand? UpdateCategoryCommand
        {
            get
            {
                if (_updateCategoryCommand == null)
                    _updateCategoryCommand = new RelayCommand(parameter => UpdateCategory(parameter as string));
                return _updateCategoryCommand;
            }
        }

        void UpdateCategory(string? category)
        {
            _menuVM.SelectedCategory = category;
        }


        private ICommand? _aboutCommand;
        public ICommand? AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                    _aboutCommand = new RelayCommand(ShowAbout);
                return _aboutCommand;
            }
        }

        void ShowAbout(object? parameter)
        {
            string aboutInfo = $"Nume: {"Iftime Razvan"}\n" +
                               $"Email: {"razvan.iftime@student.unitbv.ro"}\n" +
                               $"Group: {"10LF232"}\n" +
                               $"Specialization: {"Informatica"}";

            MessageBox.Show(aboutInfo, "About");
        }



        private ICommand? _setStandardBoardCommand;
        public ICommand? SetStandardBoardCommand
        {
            get
            {
                if (_setStandardBoardCommand == null)
                    _setStandardBoardCommand = new RelayCommand(SetStandardBoard);
                return _setStandardBoardCommand;
            }
        }

        void SetStandardBoard(object? parameter)
        {
            _menuVM.BoardWidth = 4;
            _menuVM.BoardHeight = 4;
        }

        private ICommand? _setBoardSizeCommand;
        public ICommand? SetBoardSizeCommand
        {
            get
            {
                if (_setBoardSizeCommand == null)
                    _setBoardSizeCommand = new RelayCommand(SetBoardSize);
                return _setBoardSizeCommand;
            }
        }

        private void SetBoardSize(object parameter)
        {
            if (parameter is string)
            {
                 string ind = parameter as string;
                 int index = int.Parse(ind);
                _menuVM.BoardWidth = _menuVM.PredefinedSizes[index].Width;
                _menuVM.BoardHeight = _menuVM.PredefinedSizes[index].Height;
            }
        }

        private ICommand? _exitCommand;
        public ICommand? ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    _exitCommand = new RelayCommand(Exit);
                return _exitCommand;
            }
        }

        private void Exit()
        {
            MainWindow loginWindow = new MainWindow();

            var currentWindow = Application.Current.Windows
                                     .OfType<Window>()
                                     .SingleOrDefault(w => w.IsActive);

            loginWindow.Owner = null;

            loginWindow.Show();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }), DispatcherPriority.Background);
        }

        private ICommand? _newGameCommand;
        public ICommand? NewGameCommand
        {
            get
            {
                if (_newGameCommand == null)
                    _newGameCommand = new RelayCommand(NewGame);
                return _newGameCommand;
            }
        }

        private void NewGame()
        {
            GameWindow gamewindow = new GameWindow();
            GameVM _gameVM = new GameVM(_menuVM.SelectedCategory,_menuVM.BoardWidth,_menuVM.BoardHeight,60,_menuVM.user);
            gamewindow.DataContext = _gameVM;
            gamewindow.Show();

        }

        private ICommand? _openGameCommand;

        public ICommand? OpenGameCommand
        {
            get
            {
                if (_openGameCommand == null)
                    _openGameCommand = new RelayCommand(OpenGame);
                return _openGameCommand;
            }
        }

        private void OpenGame() 
        {
            GameWindow gamewindow = new GameWindow();

            string fileName = $"{_menuVM.user.Name}.json";
            try
            {
                if (File.Exists(fileName))
                {
                    var json = File.ReadAllText(fileName);
                    Game _game = JsonConvert.DeserializeObject<Game>(json);
                    GameVM _gameVM = new GameVM(_game,_menuVM.user);
                    gamewindow.DataContext = _gameVM;
                    gamewindow.Show();
                }
                else
                {
                    MessageBox.Show("No saved game found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}");
            }
        }

        private ICommand? _statsCommand;

        public ICommand? StatsCommand
        {
            get
            {
                if (_statsCommand == null)
                    _statsCommand = new RelayCommand(ShowStats);
                return _statsCommand;
            }
        }


        private void ShowStats()
        {
            StatsWindow statsWindow = new StatsWindow();
            StatsVM _statsVM = new StatsVM();
            statsWindow.DataContext = _statsVM;
            statsWindow.Show();
        }
    }

}

