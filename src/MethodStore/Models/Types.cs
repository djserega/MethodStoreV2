using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Models
{
    public class Types : INotifyPropertyChanged
    {
        private int _iD;
        private string _name;
        private bool _addToInvocationString;

        public int ID { get => _iD; set { _iD = value; NotifyPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; NotifyPropertyChanged(); } }
        public bool AddToInvocationString { get => _addToInvocationString; set { _addToInvocationString = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
