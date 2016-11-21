using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Business;
using CompteWin10.Com;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Strings;
using CompteWin10.Utils;
using CompteWin10.View;

namespace CompteWin10
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// page principale
        /// </summary>
        public static Shell AppShell;

        /// <summary>
        /// Mode d'utilisation de l'appli
        /// </summary>
        public static AppareilEnum ModeApp { get; private set; }

        /// <summary>
        /// Indique si le romaing des catégorie est activé ou non
        /// </summary>
        public static bool IsRoamingCategorieActive { get; set; }

        /// <summary>
        /// Indique si le roaming des échéancier est activé ou non
        /// </summary>
        public static bool IsRoamingEcheancierActive { get; set; }

        /// <summary>
        /// Id de la couleur de fond
        /// </summary>
        public static int IdCouleurBackground { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            ApplicationData.Current.DataChanged += Current_DataChanged;
        }

        public async void Current_DataChanged(ApplicationData sender, object args)
        {
            try
            {
                if (AppShell != null)
                {
                    await ContexteAppli.ChargerMouvementRoaming();
                    await ContexteAppli.GenerateCategorieMouvement();

                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;
                Window.Current.Content = rootFrame;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

            if (rootFrame.Content == null)
            {
                //Controle de l'éxistence de la base de donnée pour choisir quel mode de démarrage
                var sqlite =await  ComSqlite.GetComSqlite();
                var dbExist = await sqlite.CheckDbExist();
                
                //Génération des données Utils
                DeviseUtils.GeneratePays();
                DeviseUtils.GenerateDevise();

                if (dbExist)
                {
                   OpenShell();
                }
                else
                {
                    OpenDemarrageView();
                }
            }
            Window.Current.Activate();
        }

        /// <summary>
        /// Charge les données de l'application et ouvre le shell
        /// </summary>
        public static async void OpenShell()
        {
            //Chargement des données
            var applicationBusiness = new ApplicationBusiness();
            await applicationBusiness.Initialization;

            //mise en place de la configuration
            await applicationBusiness.CheckVersion();
            ModeApp = await applicationBusiness.GetModeAppli();
            IsRoamingCategorieActive = await applicationBusiness.GetRoamingCategorieActive();
            IsRoamingEcheancierActive = await applicationBusiness.GetRoamingEcheancierActive();
            IdCouleurBackground = await applicationBusiness.GetIdCouleurBackGround();
            var idLangue = await applicationBusiness.GetLangueAppli();
            if (!string.IsNullOrWhiteSpace(idLangue))
            {
                ListeLangues.ChangeLangueAppli(idLangue);
            }

            //génère les catégories
            await ContexteAppli.GenerateCategorieMouvement();

            //a n'éxécuter que s'il s'agit de l'appareil principal
            if (ModeApp == AppareilEnum.ModeAppareilPrincipal)
            {
                //charge les mouvements donné par les appareils secondaires
                await ContexteAppli.ChargerMouvementRoaming();
                
                //controle les échéanciers
                await ContexteAppli.ControleEcheancier();
            }


            //sauvegarde den tant que dernier démarrage
            await applicationBusiness.SetDateDernierDemarrage();

            //Chargement de la page principale puis de sa frame
            var rootFrame = Window.Current.Content as Frame;
            rootFrame = new Frame();
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(Shell));
            AppShell = rootFrame.Content as Shell;
            AppShell.NavigateFrame(typeof(AcceuilView));
        }

        public static void OpenDemarrageView()
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame = new Frame();
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(DemarrageView));
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null && !rootFrame.CanGoBack) return;
            e.Handled = true;
            rootFrame?.GoBack();
        }
    }
}
