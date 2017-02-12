using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace PWU2017.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            App.FileReceived += async (s, e) => File = await _fileService.LoadAsync(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Models.FileInfo _file;

        public Models.FileInfo File
        {
            get { return _file; }
            set
            {
                _file = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(File)));
            }
        }

        private readonly Services.FileService _fileService = new Services.FileService();

        public async void Save()
        {
            await _fileService.SaveAsync(File);
        }

        public async void Open()
        {
            // prompt a picker
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            picker.FileTypeFilter.Add(".txt");
            var file = await picker.PickSingleFileAsync();
            if (file == null)
                await new Windows.UI.Popups.MessageDialog("No file selected.").ShowAsync();
            else
                File = await _fileService.LoadAsync(file);
        }
    }
}