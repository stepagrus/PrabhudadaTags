using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace Tagger.Model
{
    public class FileTemplate : ViewModelBase
    {

        public string GetSourcePath() => SourcePath;

        public string Filename
        {
            get => _assembler?.Assemble() ?? String.Empty;
            set
            {
                bool isNew = _assembler == null;
                bool isUpdate = !isNew && _assembler?.Assemble() != value;



                if (isNew)
                {
                    _assembler = FilenameAssembler.Disassemble(value);
                    _assembler.PropertyChanged += RaiseChanged;
                }
                else if (isUpdate)
                {
                    _assembler = FilenameAssembler.Disassemble(value);
                    _assembler.PropertyChanged += RaiseChanged;
                    RaisePropertyChanged(nameof(Filename));
                }
            }
        }

        private void RaiseChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Filename));
            RaisePropertyChanged(nameof(Number));

            PrabhupadaFileService.ReneameFile(this);

        }

        public string SourcePath { private get; set; }
        public TimeSpan Mp3Length { get; set; }

        public string Number
        {
            get => Assembler.Number;
            set => Assembler.Number = value;
        }

        public string Date
        {
            get => Assembler.Date;
            private set => Assembler.Date = value;
        }

        public FilenameAssembler Assembler
        {
            get => _assembler;
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

        FilenameAssembler _assembler;
    }



}
