using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MethodStoreService
{
    public partial class Service1 : ServiceBase
    {
        private HttpService _httpService;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            _httpService = new HttpService();
            _httpService.StartService();
        }

        protected override void OnStop()
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            _httpService = null;
        }
    }
}
