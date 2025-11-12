using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;
using Newtonsoft.Json;
using System.Windows;
using MemoryGame.View;
using System.Diagnostics;

namespace MemoryGame.ViewModel
{
    public class GameVM : BaseVM
    {
        private  Game _game;
        private readonly DispatcherTimer _timer;
        public GameCommands? GameCommands { get; set;}

        public GameVM()
        {
            GameCommands = new GameCommands(this);
            _game = new Game();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, args) =>
            {
                _game.TimeRemaining--;
                if (_game.TimeRemaining <= 0)
                {
                    _timer.Stop();
                }
            };
            _timer.Start();
        }

        public GameVM(string category, int boardWidth, int boardHeight, int timeLimit,User _user)
        {
            
            _game = new Game(category, boardWidth, boardHeight, timeLimit);
            InitializeCards(category);
            user = _user;
            user.Wins = 0;
            user.PlayedGames = 0;
            ReadUserStatistics(user);


            var gameWindow = Application.Current.Windows.OfType<GameWindow>().FirstOrDefault();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, args) =>
            {
                _game.TimeRemaining--;
                NotifyPropertyChanged(nameof(TimeRemaining)); 
                if (_game.TimeRemaining <= 0)
                {
                    _timer.Stop();
                    if(Cards.All(c => c.IsFlipped))
                    {
                        user.Wins++;
                    }
                    user.PlayedGames++;
                    WriteUserToFile(user);
                    gameWindow.Close();

                }
            };
            _timer.Start();
            GameCommands = new GameCommands(this);

            gameWindow.Closing += (sender, e) => {
                if (_timer.IsEnabled)
                {
                    SaveGame();
                    if (Cards.All(c => c.IsFlipped))
                    {
                        user.Wins++;
                        user.PlayedGames--;
                        WriteUserToFile(user);
                    }
                }
            };
        }

        public GameVM(Game game, User _user)
        {

            _game = game;
            user = _user;
            user.Wins = 0;
            user.PlayedGames = 0;
            ReadUserStatistics(user);

            var gameWindow = Application.Current.Windows.OfType<GameWindow>().FirstOrDefault();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, args) =>
            {
                _game.TimeRemaining--;
                NotifyPropertyChanged(nameof(TimeRemaining)); 
                if (_game.TimeRemaining <= 0)
                {
                    _timer.Stop();
                    if (Cards.All(c => c.IsFlipped))
                    {
                        user.Wins++;
                    }
                    user.PlayedGames++;
                    WriteUserToFile(user);
                    gameWindow.Close();
                }
            };
            _timer.Start();
            GameCommands = new GameCommands(this);


            gameWindow.Closing += (sender, e) => {
                if (_timer.IsEnabled)
                {
                    SaveGame();
                    if (Cards.All(c => c.IsFlipped))
                    {
                        user.Wins++;
                        user.PlayedGames++;
                        WriteUserToFile(user);
                    }
                }
            };
        }

        private void WriteUserToFile(User user)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data", "Stats5.txt")); ;
            try
            {
                var lines = File.ReadAllLines(filePath).ToList();

                // Find the line that contains the user's name
                var userLine = lines.FirstOrDefault(line => line.StartsWith(user.Name));

                if (userLine != null)
                {
                    // Remove the old line
                    lines.Remove(userLine);

                    // Add the updated line
                    lines.Add($"{user.Name}-{user.PlayedGames}-{user.Wins}");
                }
                else
                {
                    // If the user is not found, add a new line
                    lines.Add($"{user.Name}{user.PlayedGames}-{user.Wins}");
                }

                // Write all lines back to the file
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Eroare la scrierea fișierului: {ex.Message}");
            }
        }

        public void ReadUserStatistics(User user)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data", "Stats5.txt"));

            try
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(filePath);

                // Find the line that contains the user's name
                var userLine = lines.FirstOrDefault(line => line.StartsWith(user.Name + "-"));

                if (userLine != null)
                {
                    // Split the line to extract the statistics
                    var parts = userLine.Split('-');
                    if (parts.Length == 3)
                    {
                        user.PlayedGames = int.Parse(parts[1]);
                        user.Wins = int.Parse(parts[2]);

                    }
                    else
                    {
                        MessageBox.Show("Formatul liniei este incorect.");
                    }
                }
                else
                {
                    MessageBox.Show($"Nu există statistici pentru utilizatorul {user.Name}.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la citirea fișierului: {ex.Message}");
            }
        }


        User user { get; set; }


        public ObservableCollection<Card> Cards => _game.Cards;

        public int TimeRemaining
        {
            get => _game.TimeRemaining;
            set => _game.TimeRemaining = value;
        }

        public string Category
        {
            get => _game.Category;
            set => _game.Category = value;
        }

        public int BoardWidth
        {
            get => _game.BoardWidth;
            set => _game.BoardWidth = value;
        }

        public int BoardHeight
        {
            get => _game.BoardHeight;
            set => _game.BoardHeight = value;
        }

        private void InitializeCards(string Category)
        {
            var imagePaths = LoadImagePathsFromDirectory(Category);

            var cardPairs = imagePaths
       .Select(path => new Card[]
       {
            new Card { ImagePath = path, IsFlipped = false },
            new Card { ImagePath = path, IsFlipped = false }
       })
       .ToList();

            var allCards = cardPairs.SelectMany(pair => pair).ToList();

            int maxCards = _game.BoardWidth * _game.BoardHeight;
            if (allCards.Count > maxCards)
            {
                allCards = allCards.Take(maxCards).ToList();
            }

            var random = new Random();
            for (int i = allCards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = allCards[i];
                allCards[i] = allCards[j];
                allCards[j] = temp;
            }

            foreach (var card in allCards)
            {
                _game.Cards.Add(card);
            }
        }

        private List<string> LoadImagePathsFromDirectory(string category)
        {
            var imagePaths = new List<string>();
            var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Resources",category));

            try
            {
                var files = Directory.GetFiles(filePath);
                foreach (var file in files)
                {
                    if (supportedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    {
                        imagePaths.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }

            return imagePaths;
        }

        public void SaveGame()
        {
            string fileName = $"{user.Name}.json";
            try
            {
                var json = JsonConvert.SerializeObject(_game);
                File.WriteAllText(fileName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}");
            }
        }

        public void LoadGame()
        {
            string fileName = $"{user.Name}.json";
            try
            {
                if (File.Exists(fileName))
                {
                    var json = File.ReadAllText(fileName);
                    _game = JsonConvert.DeserializeObject<Game>(json);
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
    }
}

