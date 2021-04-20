using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private readonly IFileStoreService _fileStoreService;

        public LanguageInfo CurrentLanguage
        {
            get => I18NSource.Instance.CurrentLanguage;
            set => I18NSource.Instance.CurrentLanguage = value;
        }

        public ObservableCollection<LanguageInfo> SupportedLanguages
        {
            get => I18NSource.Instance.SupportedLanguages;
        }

        public SettingViewModel(IFileStoreService fileStoreService)
        {
            this._fileStoreService = fileStoreService;
        }
    }
}