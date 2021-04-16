using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private readonly IFileStoreService _fileStoreService;
        private readonly ISearchboxService _searchboxService;

        private String _text;
        public String Text
        {
            get => this._text;
            set => SetProperty(ref this._text, value);
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                if (this._clickCommand is null)
                {
                    this._clickCommand = new RelayCommand(Click);
                }
                return this._clickCommand;
            }
        }

        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand
        {
            get
            {
                if (this._saveClickCommand is null)
                {
                    this._saveClickCommand = new AsyncRelayCommand(SaveClickAsync);
                }
                return this._saveClickCommand;
            }
        }


        private ICommand _readClickCommand;
        public ICommand ReadClickCommand
        {
            get
            {
                if (this._readClickCommand is null)
                {
                    this._readClickCommand = new AsyncRelayCommand(ReadClickAsync);
                }
                return this._readClickCommand;
            }
        }

        private ICommand _deleteClickCommand;
        public ICommand DeleteClickCommand
        {
            get
            {
                if (this._deleteClickCommand is null)
                {
                    this._deleteClickCommand = new RelayCommand(DeleteClick);
                }
                return this._deleteClickCommand;
            }
        }

        public SettingViewModel(IFileStoreService fileStoreService, ISearchboxService searchboxService)
        {
            this._fileStoreService = fileStoreService;
            this._searchboxService = searchboxService;
        }

        private void Click()
        {
            var text = this._searchboxService.GetSearchboxText();
            Text = text;
            //Text = DateTime.Now.ToString();
        }

        private static String _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static String _path = Path.Combine(_localAppData, Program.AppName);
        private static String _filename = "test.txt";

        private async Task SaveClickAsync()
        {
            var time = DateTime.Now.ToString();
            var bytes = Encoding.UTF8.GetBytes(time);
            await this._fileStoreService.SaveAsync(_path, _filename, bytes);
        }


        private async Task ReadClickAsync()
        {
            var bytes = await this._fileStoreService.ReadAsync(_path, _filename);
            var str = Encoding.UTF8.GetString(bytes);
            MessageBox.Show(str);
        }


        private void DeleteClick()
        {
            this._fileStoreService.DeleteFile(_path, _filename);
            this._fileStoreService.DeleteFolder(_path);
        }
    }
}
