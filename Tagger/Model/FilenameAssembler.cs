using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Model
{
    public class FilenameAssembler
    {
        public FilenameAssembler(string filename)
        {
            _fileName = filename;
            Disassemble(filename);
        }

        public void SetNumber(int number)
        {
            Number = $"#{number}";
        }

        public void IncNumber()
        {
            if (!_isValid)
                return;

            if (Int32.TryParse(Number.Remove(0, 1), out int num))
                SetNumber(++num);
        }

        public void DecNumber()
        {
            if (!_isValid)
                return;

            if (Int32.TryParse(Number.Remove(0, 1), out int num))
                SetNumber(--num);
        }

        public string FileName
        {
            get
            {
                if (_isValid)
                {
                    return Assemble();
                }
                else
                {
                    return _fileName;
                }
            }
            set
            {
                _fileName = value;
                Disassemble(value);
            }
        }
        private string _fileName;

        public string Date { get; set; } = String.Empty;
        public string Number { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Shastra { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;


        private void Disassemble(string filename)
        {
            var items = filename.Split(new string[] { Delimiter }, StringSplitOptions.None);

            var dateComponents = items[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            bool isIncorrentDate = dateComponents.Length < 1 || dateComponents.Length > 2;
            bool isIncorrectFileNameFormat = items.Length < 3;
            if (isIncorrentDate || isIncorrectFileNameFormat)
            {
                _isValid = false;
                return;
            }
            else
            {
                _isValid = true;
            }

            Date = dateComponents[0];
            if (dateComponents.Length == 2)
                Number = dateComponents[1];

            City = items[1];
            Shastra = items[2];

            if (items.Length > 3)
                Title = String.Join(Delimiter, items.GetSubArrayStartAt(3));
        }

        private string Assemble()
        {
            List<string> items = new List<string>();
            string _date = Date;
            if (!String.IsNullOrEmpty(Number))
                _date += " " + Number;
            items.Add(_date);
            items.Add(City);
            items.Add(Shastra);
            items.Add(Title);
            return String.Join(Delimiter, items);
        }

        private bool _isValid;

        private const string Delimiter = " — ";
    }
}
