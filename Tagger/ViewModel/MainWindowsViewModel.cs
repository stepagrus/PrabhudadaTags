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
    public FileTemplate SelectedItem
    {
      get => _selectedItem;
      set
      {
        _selectedItem = value;
        RaisePropertyChanged(nameof(UpCommand));
        RaisePropertyChanged(nameof(DownCommand));
      }
    }

    public string Progress { get; private set; }

    public ICommand OpenFolderCommand => new RelayCommand(OpenFolderWithFiles);
    public ICommand DecrementNumberCommand => new RelayCommand(DecrementNumber);
    public ICommand IncrementNumberCommand => new RelayCommand(IncrementNumber);
    public ICommand RemoveNumberCommand => new RelayCommand(RemoveNumber);
    public ICommand UpCommand => new RelayCommand(IncNumber, HasSelectedItem);
    public ICommand DownCommand => new RelayCommand(DecNumber, HasSelectedItem);
    public ICommand ShowInExplorer => new RelayCommand(() => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", " /select, " + SelectedItem.SourcePath)));
    public ICommand PlayCommand => new RelayCommand(() => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(SelectedItem.SourcePath)));

    private void IncNumber()
    {

      if (String.IsNullOrEmpty(SelectedItem.Number))
        return;

      var groupByDate = FileItems.Where(o => o.Date == SelectedItem.Date).OrderBy(o => o.Number);
      if (SelectedItem.Number == "#1")
      {
        foreach (var fileItem in groupByDate)
        {
          if (fileItem == SelectedItem)
          {
            fileItem.RemoveNumber();
            continue;
          }
          else
          {
            fileItem.DecNumber();
          }
        }
      }
      else
      {
        int count = groupByDate.Count();
        for (int i = 0; i < count; i++)
        {
          bool hasPrevous = i > 0;
          if (groupByDate.ElementAt(i) == SelectedItem && hasPrevous)
          {
            var previous = groupByDate.ElementAt(i - 1);
            var current = SelectedItem;
            SwapNumbers(ref current, ref previous);
          }
        }
      }
    }

    private void DecNumber()
    {
      var groupByDate = FileItems.Where(o => o.Date == SelectedItem.Date).OrderBy(o => o.Number);
      int count = groupByDate.Count();

      for (int i = 0; i < count; i++)
      {
        bool hasNext = i < count - 1;
        var current = groupByDate.ElementAt(i);

        if (current == SelectedItem && hasNext)
        {
          var next = groupByDate.ElementAt(i + 1);
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

    private void OpenFolderWithFiles()
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

    private bool HasSelectedItem()
    {
      return SelectedItem != null;
    }

    private string _path;
    private FileTemplate _selectedItem;
  }
}

