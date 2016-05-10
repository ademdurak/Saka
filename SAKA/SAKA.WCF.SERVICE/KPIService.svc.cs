using SAKA.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ClassLibrary1;

namespace SAKA.WCF.SERVICE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KPIService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select KPIService.svc or KPIService.svc.cs at the Solution Explorer and start debugging.
    public class KPIService : IKPIService
    {
        public int count()
        {
            return KPI.count();
        }
        public int Hesapla(int x,int y)
        {
            return KPI.Hesapla(x,y);
        }

       
    }
}
