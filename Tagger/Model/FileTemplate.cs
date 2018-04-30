using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace Tagger.Model
{
    public class FileTemplate : ObservableObject
    {
        public FileTemplate(string sourcePath)
        {
            SourcePath = sourcePath;
            ParentDir = System.IO.Path.GetDirectoryName(sourcePath);

            var fileName = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
            _assembler = new FilenameAssembler(fileName);
        }

        public string Filename
        {
            get => _assembler.FileName;
            set
            {
                if (_assembler.FileName != value)
                {
                    _assembler.FileName = value;
                    NameChanged();
                }
            }
        }

        internal string SourcePath { get; set; }

        public string ParentDir { get; }

        public TimeSpan Mp3Length { get; set; }

        public string Number
        {
            get => _assembler.Number;
            set
            {
                if (_assembler.Number != value)
                {
                    _assembler.Number = value;
                    NameChanged();
                }
            }
        }

        public string Date
        {
            get => _assembler.Date;
            private set
            {
                if (_assembler.Date != value)
                {
                    _assembler.Date = value;
                    NameChanged();
                }
            }
        }

        public void IncNumber()
        {
            _assembler?.IncNumber();
        }
        public void DecNumber()
        {
            _assembler?.DecNumber();
        }

        public void RemoveNumber()
        {
            if (_assembler != null)
                _assembler.Number = String.Empty;
        }

        private void NameChanged()
        {
            RaisePropertyChanged(nameof(Filename));
            RaisePropertyChanged(nameof(Number));
            RaisePropertyChanged(nameof(Date));

            PrabhupadaFileService.ReneameFile(this);
        }

        private FilenameAssembler _assembler;
    }
}

