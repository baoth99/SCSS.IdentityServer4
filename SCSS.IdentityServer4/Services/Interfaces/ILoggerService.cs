using System;

namespace SCSS.IdentityServer4.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(Exception ex, string message);

        void LogError(string message);
    }
}
