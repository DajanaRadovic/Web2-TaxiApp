using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace PriceCalculationService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class PriceCalculationService : StatelessService, ICalculation
    {
        public PriceCalculationService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<Calculation> GetPrice(string toLocation, string fromLocation)
        {
            double rangeMin = 5.0;
            double rangeMax = 20.0;

            Random random = new Random();
            double price = rangeMin + (rangeMax - rangeMin) * random.NextDouble();

            TimeSpan estimatedTimeMin = new TimeSpan(0, 1, 0); // 1 min
            TimeSpan estimatedTimeMax = new TimeSpan(0, 2, 0); // 2 min

            return new Calculation(price, estimatedTimeMin, estimatedTimeMax);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        => this.CreateServiceRemotingInstanceListeners();

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
