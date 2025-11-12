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
    public class StatsVM : BaseVM
    {
        public ObservableCollection<string>? Stats { get; set; }

        public StatsVM()
        {
            Stats = new ObservableCollection<string>();
            ReadStatsFromFile();
        }


        private void ReadStatsFromFile()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\..", "Data", "Stats5.txt"));
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Stats.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la citirea fișierului: {ex.Message}");
            }
        }
    }
}
