using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Models
{
    public class Method
    {
        public Guid ID { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string ObjectName { get; set; }
        public string MethodName { get; set; }

        public string MethodInvokationString { get; set; }
        public string Description { get; set; }

        public bool TemplateAddToText { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTextCorrect { get; set; }
        public string TemplateAddToContextMenu { get; set; }
    }
}
