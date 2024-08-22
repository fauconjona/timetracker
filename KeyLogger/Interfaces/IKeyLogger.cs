using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeyLogger.Services.KeyLoggerService;

namespace KeyLogger.Interfaces
{
    public interface IKeyLoggerService
    {
        /// <summary>
        /// Start the key logger
        /// </summary>
        void Start(LogHandler logHandler);

        /// <summary>
        /// Stop the key logger
        /// </summary>
        void Stop();
    }
}
