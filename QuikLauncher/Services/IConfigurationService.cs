namespace QuikLauncher
{
    public interface IConfigurationService
    {
        int Delay { get; }
        string QuikAuthTitle { get; }
        string QuikConnectButtonTitle { get; }
        string QuikLoginTitle { get; }
        string QuikNotAuthTitle { get; }
        string QuikNotAuthTitle2 { get; }
        string QuikPath { get; }
        string QuikUserPass { get; }
    }
}