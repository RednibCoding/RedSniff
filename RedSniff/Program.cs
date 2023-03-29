namespace Redsniff
{
    internal static class Program
    {
        public static MainState MainState = new();
        public static MainController MainController = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var mainForm = new MainForm();
            MainState.MainForm = mainForm;

            MainController.OnLoad();

            Application.Run(mainForm);
        }
    }
}