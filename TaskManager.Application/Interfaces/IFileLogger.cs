

namespace TaskManager.Application.Interfaces;

public interface IFileLogger
{
    void LogException(Exception ex, string context = "General");
    void LogInfo(string message, string context = "Info");
}
