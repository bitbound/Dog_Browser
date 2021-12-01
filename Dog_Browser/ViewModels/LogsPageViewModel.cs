using Dog_Browser.Mvvm;
using Dog_Browser.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dog_Browser.ViewModels
{
    public class LogsPageViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<LogsPageViewModel> _logger;
        private readonly string _logsDirectory = Path.Combine(Path.GetTempPath(), "Dog_Browser");
        private string _currentLogContents = "";
        private string _selectedLogName = "";
        public LogsPageViewModel(IFileSystem fileSystem, IDialogService dialogService, ILogger<LogsPageViewModel> logger)
        {
            _fileSystem = fileSystem;
            _dialogService = dialogService;
            _logger = logger;

            LoadLogFileNames();
        }

        public string CurrentLogContents
        {
            get => _currentLogContents;
            set => SetProperty(ref _currentLogContents, value);
        }

        public ObservableCollection<string> LogNames { get; } = new();

        public string SelectedLogName
        {
            get => _selectedLogName;
            set
            {
                SetProperty(ref _selectedLogName, value);
                LoadLogFileContents();
            }
        }

        private void LoadLogFileContents()
        {
            try
            {
                var filePath = Path.Combine(_logsDirectory, SelectedLogName);
                CurrentLogContents = _fileSystem.ReadAllText(filePath);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading log file.");
                _dialogService.Show("There was an error loading the log file.", "Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLogFileNames()
        {
            try
            {
                if (!_fileSystem.DirectoryExists(_logsDirectory))
                {
                    return;
                }

                var files = _fileSystem.EnumerateFiles(_logsDirectory);

                if (!files.Any())
                {
                    return;
                }

                LogNames.Clear();
                foreach (var file in files)
                {
                    LogNames.Add(Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading log file names.");
                _dialogService.Show("There was an error loading the log file names.", "Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
