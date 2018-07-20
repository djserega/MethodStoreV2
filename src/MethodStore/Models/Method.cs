using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Models
{
    public class Method : NotifyPropertyChangedClass
    {
        private int _iD;
        private string _group = "";
        private string _type = "";
        private string _objectName = "";
        private string _methodName = "";
        private string _methodInvokationString = "";
        private string _description = "";
        private bool _templateAddToText = false;
        private string _templateName = "";
        private string _templateTextCorrect = "";
        private string _templateAddToContextMenu = "";

        public int ID { get => _iD; set { _iD = value; NotifyPropertyChanged(); } }
        public string Group { get => _group; set { _group = value; NotifyPropertyChanged(); } }
        public string Type { get => _type; set { _type = value; NotifyPropertyChanged(); } }
        public string ObjectName { get => _objectName; set { _objectName = value; NotifyPropertyChanged(); } }
        public string MethodName { get => _methodName; set { _methodName = value; NotifyPropertyChanged(); } }

        public string MethodInvokationString { get => _methodInvokationString; set { _methodInvokationString = value; NotifyPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; NotifyPropertyChanged(); } }

        public bool TemplateAddToText { get => _templateAddToText; set { _templateAddToText = value; NotifyPropertyChanged(); } }
        public string TemplateName { get => _templateName; set { _templateName = value; NotifyPropertyChanged(); } }
        public string TemplateTextCorrect { get => _templateTextCorrect; set { _templateTextCorrect = value; NotifyPropertyChanged(); } }
        public string TemplateAddToContextMenu { get => _templateAddToContextMenu; set { _templateAddToContextMenu = value; NotifyPropertyChanged(); } }

        internal void Fill(Method method)
        {
            Type = method.Type;
            ObjectName = method.ObjectName;
            MethodName = method.MethodName;

            SetMethodInvokationString();
        }

        public override void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.NotifyPropertyChanged(propertyName);

            switch (propertyName)
            {
                case "Type":
                case "ObjectName":
                case "MethodName":
                    SetMethodInvokationString();
                    break;
            }
        }

        internal void SetMethodInvokationString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (NeedAddTypeInInvokationString())
            {
                stringBuilder.Append(_type);
                stringBuilder.Append(".");
            }
            stringBuilder.Append(_objectName);
            stringBuilder.Append(".");
            stringBuilder.Append(_methodName);
            stringBuilder.Append("(");
            stringBuilder.Append(")");
            stringBuilder.Append(";");

            MethodInvokationString = stringBuilder.ToString();
        }

        private bool NeedAddTypeInInvokationString()
        {
            return new EF.Context<Types>().FindByName(_type)?.AddToInvocationString ?? false;
        }

    }
}
