using Common.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    [ServiceContract]
    public interface ICalculation:IService
    {
        [OperationContract]
        Task<Calculation> GetPrice(string toLocation, string fromLocation);
    }
}
