using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tagger.Model;
using System;
using GalaSoft.MvvmLight.Threading;

namespace Tagger.ViewModel
{
    class MainWindowsViewModel : ViewModelBase
    {
        public MainWindowsViewModel()
        {
            FileItems = new ObservableCollection<FileTemplate>();
        }

        public ObservableCollection<FileTemplate> FileItems { get; private set; }
        public FileTemplate SelectedItem { get; set; }
        public string Progress { get; private set; }

        public ICommand OpenFolderCommand => new RelayCommand(ShowDialog);
        public ICommand DecrementNumberCommand => new RelayCommand(DecrementNumber);
        public ICommand IncrementNumberCommand => new RelayCommand(IncrementNumber);
        public ICommand RemoveNumberCommand => new RelayCommand(RemoveNumber);
        public ICommand UpCommand => new RelayCommand(UpNumber);
        public ICommand DownCommand => new RelayCommand(DownNumber);
        public ICommand ShowInExplorer => new RelayCommand(() => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", " /select, " + SelectedItem.GetSourcePath())));

        private void UpNumber()
        {
            //получить выбранный объект
            //получить дату у выбранного объекта
            //выбрать все объекты с такой же датой
            //у группы объектов получить номер
            //если выьранный объект имеет номер 1, то 
            //  убрать номер вообще
            //  у остальный объектов в группе понизить номер на 1
            // если выбранный номер больше 1, то
            //  если предыдущего объекта нет, то ничего не делать
            //  обменять номера у выбранного объекта и предыдущего
            //  

            var group = FileItems.Where(o => o.Date == SelectedItem.Date).OrderBy(o => o.Number);
            if (SelectedItem.Number == "#1")
            {
                foreach (var item in group)
                {
                    if (item == SelectedItem)
                    {
                        item.RemoveNumber();
                        continue;
                    }
                    else
                    {
                        item.DecNumber();
                    }

                }
            }
            else if (!String.IsNullOrEmpty(SelectedItem.Number))
            {
                int count = group.Count();
                for (int i = 0; i < count; i++)
                {
                    bool hasPrevous = i > 0;
                    if (group.ElementAt(i) == SelectedItem && hasPrevous)
                    {
                        //exchange
                        var previous = group.ElementAt(i - 1);
                        var current = SelectedItem;
                        SwapNumbers(ref current, ref previous);
                    }
                }
            }

        }

        private void DownNumber()
        {
            //получить выбранный объект
            //получить дату у выбранного объекта
            //выбрать все объекты с такой же датой
            //у группы объектов получить номер
            //если после выбранного объекта имеется следующий, то 
            //  обменять их номера местами
            var group = FileItems.Where(o => o.Date == SelectedItem.Date).OrderBy(o => o.Number);
            int count = group.Count();

            for (int i = 0; i < count; i++)
            {
                bool hasNext = i < count - 1;
                var current = group.ElementAt(i);

                if (current == SelectedItem && hasNext)
                {
                    var next = group.ElementAt(i + 1);
                    SwapNumbers(ref current, ref next);
                    break;
                }

            }
        }

        private void SwapNumbers(ref FileTemplate a, ref FileTemplate b)
        {
            var t = a.Number;
            a.Number = b.Number;
            b.Number = t;
        }

        private void ShowDialog()
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _path = folderBrowserDialog.SelectedPath;
                    GetMp3FilesDescriptions();
                }
            }
        }

        private void GetMp3FilesDescriptions()
        {
            FileItems.Clear();
            var fileService = new PrabhupadaFileService(_path);
            fileService.NewFileParsed += OnNewFileParsed;
            fileService.ProgressUpdated += OnProgressUpdated;
            Task.Run(() => fileService.StartGettingFilesDescriptors());

        }

        private void OnProgressUpdated(object sender, string e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Progress = e;
                RaisePropertyChanged(nameof(Progress));
            });
        }

        private void OnNewFileParsed(object sender, FileTemplate e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                FileItems.Add(e);
            });
        }

        private void DecrementNumber()
        {
            SelectedItem?.DecNumber();
        }

        private void IncrementNumber()
        {
            SelectedItem?.IncNumber();
        }

        private void RemoveNumber()
        {
            SelectedItem?.RemoveNumber();
        }

        private string _path;
    }



}

