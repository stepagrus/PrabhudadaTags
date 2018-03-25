using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Model
{
    public class FilenameAssembler : ViewModelBase
    {
        private FilenameAssembler()
        {

        }

        public string Date
        {
            get;
            set;
        }
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                RaisePropertyChanged(nameof(Number));
            }
        }
        string _number;

        public string City { get; set; }
        public string Shastra { get; set; }
        public string Title { get; set; }

        public static FilenameAssembler Disassemble(string filename)
        {
            FilenameAssembler instance = new FilenameAssembler();

            var items = filename.Split(new string[] { Delimiter }, StringSplitOptions.None);
            if (items.Length < 3)
            {
                return new FilenameAssembler() { Date = filename };

            }

            var dateItems = items[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (dateItems.Length < 1 && dateItems.Length > 2)
                throw new FormatException();

            instance.Date = dateItems[0];
            if (dateItems.Length == 2)
                instance.Number = dateItems[1];

            instance.City = items[1];
            instance.Shastra = items[2];

            if (items.Length > 3)
                instance.Title = String.Join(Delimiter, items.SliceToEnd(3));

            return instance;
        }



        public string Assemble()
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

        public void SetNumber(int number)
        {
            Number = "#" + number.ToString();
        }

        public void IncNumber()
        {
            try
            {
                if (Int32.TryParse(Number.Remove(0, 1), out int num))
                    SetNumber(++num);
            }
            catch
            {
                SetNumber(1);
            }
        }

        public void DecNumber()
        {
            try
            {
                if (Int32.TryParse(Number.Remove(0, 1), out int num))
                    SetNumber(--num);
            }
            catch { }
        }

        private const string Delimiter = " — ";
    }
}
