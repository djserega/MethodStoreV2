using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Models
{
    public class Group : NotifyPropertyChangedClass
    {
        private int _iD;
        private string _name;

        public int ID { get => _iD; set { _iD = value; NotifyPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; NotifyPropertyChanged(); } }

        public override string ToString()
        {
            return _name;
        }
    }
}
