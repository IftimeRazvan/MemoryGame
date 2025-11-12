using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MemoryGame.Model;
using System.Resources;
using System.Diagnostics;
using MemoryGame.View;

namespace MemoryGame.ViewModel.Commands
{
    public class SignInCommands : BaseVM
    {
        private readonly SignInVM _signInVM;

        public SignInCommands(SignInVM signInVM)
        {
            _signInVM = signInVM;
        }



        private ObservableCollection<User> ?Users
        {
            get => _signInVM.Users;
            set => _signInVM.Users = value;
        }


        private ICommand? _addUserCommand;
        public ICommand? AddCommand
        {
            get
            {
                if (_addUserCommand == null)
                    _addUserCommand = new RelayCommand(AddUser);
                return _addUserCommand;
            }
        }

        private void AddUser()
        {
            if (!string.IsNullOrWhiteSpace(_signInVM.Username))
            {
                
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string resourcesDirectory = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Resources", "Animals"));



                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory =resourcesDirectory,
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string absolutePath = openFileDialog.FileName;

                    var newUser = new User
                    {
                        Name = _signInVM.Username,
                        ImagePath = absolutePath
                    };
                    Users?.Add(newUser);
                    _signInVM.Username = string.Empty;
                    WriteUserToFile(newUser,appDirectory);
                }
                else
                {
                    MessageBox.Show("Selectarea imaginii a fost anulată.");
                }
            }
            else
            {
                MessageBox.Show("Vă rugăm introduceți un username.");
            }
        }

        private void WriteUserToFile(User user,string appDirectory)
        {
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data","Users.txt")); ;
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine($"{user.Name},{user.ImagePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la scrierea fișierului: {ex.Message}");
            }
        }

        private ICommand? _deleteUserCommand;
        public ICommand? DeleteUserCommand
        {
            get
            {
                if (_deleteUserCommand == null)
                    _deleteUserCommand = new RelayCommand(DeleteUser,CanDeleteUser);
                return _deleteUserCommand;
            }
        }

        private void DeleteUserFromFile(string userName,string filePath)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data", filePath));
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                bool userExists = lines.Any(line => line.StartsWith(userName));
                if (!userExists)
                {
                    return;
                }

                string[] newLines = lines.Where(line => !line.StartsWith(userName)).ToArray();

                File.WriteAllLines(filePath, newLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea utilizatorului din fișier: {ex.Message}", "Eroare");
            }
        }

        private void DeleteUserJsonFile(string userName)
        {
            try
            {
                string jsonFilePath = $"{userName}.json";

                // Verifică dacă fișierul există înainte de a-l șterge
                if (File.Exists(jsonFilePath))
                {
                    File.Delete(jsonFilePath);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea fișierului JSON: {ex.Message}", "Eroare");
            }
        }


        private void DeleteUser()
        {
            DeleteUserFromFile(_signInVM.SelectedUser.Name, "Users.txt");
            DeleteUserFromFile(_signInVM.SelectedUser.Name, "Stats5.txt");
            DeleteUserJsonFile(_signInVM.SelectedUser.Name);
            _signInVM.Users.Remove(_signInVM.SelectedUser);
        }

        private bool CanDeleteUser()
        {
            return _signInVM.SelectedUser != null;
        }

        private ICommand? _playCommand;
        public ICommand? PlayCommand
        {
            get
            {
                if (_playCommand == null)
                    _playCommand = new RelayCommand(Play,CanPlay);
                return _playCommand;
            }
        }

        private void Play()
        {
            MenuWindow menuWindow = new MenuWindow();
            MenuVM _menuVM = new MenuVM(_signInVM.SelectedUser);
            menuWindow.DataContext = _menuVM;
            menuWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private bool CanPlay()
        {
            return _signInVM.SelectedUser != null;
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(CloseGame);
                return _cancelCommand;
            }
        }

        private void CloseGame()
        {
            Application.Current.Shutdown();
        }


    }
}
