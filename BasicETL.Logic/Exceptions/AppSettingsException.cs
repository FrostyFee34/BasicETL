namespace BasicETL.Logic.Exceptions;

public class AppSettingsException : Exception
{
    public AppSettingsException() : base("AppSettings are partially or fully empty")
    {
    }
}