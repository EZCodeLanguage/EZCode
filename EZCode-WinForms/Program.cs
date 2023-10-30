using EZCode;

namespace EZPlayer_WinForms
{
    internal static class Program
    {
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            EzCode ez = new EzCode();
            string code = "Code...";
            ez.Code = code;
            EZProj proj = new EZProj(ez);
            Application.Run(new EZCode.EZPlayer.Player(proj));
        }
    }
}