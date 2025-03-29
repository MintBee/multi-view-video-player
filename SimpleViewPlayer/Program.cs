namespace SimpleViewMultiViewPlayer;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // The MainForm constructor now sets up MVP
        Application.Run(new MainForm());
    }
}