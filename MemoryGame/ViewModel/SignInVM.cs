using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class SignInVM : BaseVM
    {
        public SignInCommands ?SignInCommands { get; set; }
        public ObservableCollection<User>? Users{ get; set; }

        private string? _username;
        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        public SignInVM()
        {
            SignInCommands = new SignInCommands(this);
            Users = new ObservableCollection<User>();
            ReadUsersFromFile();
        }


        private void ReadUsersFromFile()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data","Users.txt"));
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 2)
                            {
                                var user = new User { Name = parts[0], ImagePath = parts[1] };
                                Users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la citirea fișierului: {ex.Message}");
            }
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged(nameof(SelectedUser));
            }
        }

    }
}
