namespace QuikLauncher
{
    public interface IConfigurationService
    {
        int Delay { get; }
        string QuikAuth { get; }
        string QuikAuthTitle { get; }
        string QuikConnectButtonTitle { get; }
        string QuikLoginTitle { get; }
        string QuikNotAuthTitle { get; }
        string QuikNotAuthTitle2 { get; }
        string QuikPass { get; }
        string QuikPath { get; }
    }
}