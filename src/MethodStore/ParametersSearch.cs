using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MethodStore
{
    public class ParametersSearch : INotifyPropertyChanged, IDisposable
    {
        private ParameterSearchEvents _parameterSearchEvents;
        private ApplicationDataContainer localSettings;

        private string _text;
        private bool _searchInGroup;
        private bool _searchInType;
        private bool _searchInObjectName;
        private bool _searchInMethodName;

        public ParametersSearch(ParameterSearchEvents parameterSearchEvents)
        {
            _parameterSearchEvents = parameterSearchEvents;
            LoadLocalSettings();
        }

        public string Text { get => _text; set { _text = value; NotifyPropertyChanged(); } }
        public bool SearchInGroup { get => _searchInGroup; set { _searchInGroup = value; NotifyPropertyChanged(); } }
        public bool SearchInType { get => _searchInType; set { _searchInType = value; NotifyPropertyChanged(); } }
        public bool SearchInObjectName { get => _searchInObjectName; set { _searchInObjectName = value; NotifyPropertyChanged(); } }
        public bool SearchInMethodName { get => _searchInMethodName; set { _searchInMethodName = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            _parameterSearchEvents.EvokeParameterSearchEvent();
        }

        private void LoadLocalSettings()
        {
            localSettings = ApplicationData.Current.LocalSettings;

            LoadSettingsByName("Text");
            LoadSettingsByName("SearchInGroup");
            LoadSettingsByName("SearchInType");
            LoadSettingsByName("SearchInObjectName");
            LoadSettingsByName("SearchInMethodName");

            localSettings = null;
        }

        private void LoadSettingsByName(string key)
        {
            object findedObject = localSettings.Values[key];
            if (findedObject != null)
            {
                switch (key)
                {
                    case "Text":
                        _text = (string)findedObject;
                        break;
                    case "SearchInGroup":
                        _searchInGroup = (bool)findedObject;
                        break;
                    case "SearchInType":
                        _searchInType = (bool)findedObject;
                        break;
                    case "SearchInObjectName":
                        _searchInObjectName = (bool)findedObject;
                        break;
                    case "SearchInMethodName":
                        _searchInMethodName = (bool)findedObject;
                        break;
                }
            }
        }

        public void SaveSettings()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["Text"] = _text;
            localSettings.Values["SearchInGroup"] = _searchInGroup;
            localSettings.Values["SearchInType"] = _searchInType;
            localSettings.Values["SearchInObjectName"] = _searchInObjectName;
            localSettings.Values["SearchInMethodName"] = _searchInMethodName;
        }

        public void Dispose()
        {
            SaveSettings();
        }
    }
}
