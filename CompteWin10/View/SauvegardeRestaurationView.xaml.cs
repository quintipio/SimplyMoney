using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Context;
using CompteWin10.Interface;
using CompteWin10.Utils;
using CompteWin10.ViewModel;

namespace CompteWin10.View
{
    /// <summary>
    /// View pour l'import ou l'export (1 pour import, 2 pour export)
    /// </summary>
    public sealed partial class SauvegardeRestaurationView : IView<SauvegardeRestaurationViewModel>
    {
        private int _mode;

        public SauvegardeRestaurationViewModel ViewModel { get; set; }



        public SauvegardeRestaurationView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            ViewModel = new SauvegardeRestaurationViewModel();
            await ViewModel.Initialization;

            _mode =  (int) e.Parameter;

            GridMdp.Visibility = Visibility.Collapsed;
            InfoPageTextBlock.Text = (_mode == 1)
                ? ResourceLoader.GetForCurrentView().GetString("RestaureText")
                : ResourceLoader.GetForCurrentView().GetString("SauvegardeText");
            ImportExportButton.Content = (_mode == 1)
                ? ResourceLoader.GetForCurrentView().GetString("RestaureText")
                : ResourceLoader.GetForCurrentView().GetString("SauvegardeText");
        }


        private void MotDePasseCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            GridMdp.Visibility = MotDePasseCheckBox.IsChecked != null && MotDePasseCheckBox.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private async void ParcourirButton_Click(object sender, RoutedEventArgs e)
        {
            ParcourirButton.IsEnabled = false;
            try
            {
                switch (_mode)
                {
                    //en cas d'import
                    case 1:
                        
                        var fileOpenPicker = new FileOpenPicker
                        {
                            CommitButtonText = ResourceLoader.GetForCurrentView().GetString("Ok"),
                            ViewMode = PickerViewMode.List,
                            SuggestedStartLocation = PickerLocationId.Downloads,
                            FileTypeFilter = { ContexteStatic.ExtensionImportExport },
                        };


                        var fichier = await fileOpenPicker.PickSingleFileAsync();
                        if (fichier != null)
                        {
                            ViewModel.Fichier = fichier;
                            EmplacementFichierTextBlock.Text = ViewModel.Fichier.Path;
                        }
                        break;

                    //en cas d'export
                    case 2:
                            var listeExtension = new List<string> { ContexteStatic.ExtensionImportExport };
                        
                            var fileSavePicker = new FileSavePicker
                            {
                                CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                                SuggestedFileName = "simplyMoneyData",
                                SuggestedStartLocation = PickerLocationId.Downloads,
                                DefaultFileExtension = ContexteStatic.ExtensionImportExport,

                            };
                        
                            fileSavePicker.FileTypeChoices.Add("simplyMoneyData", listeExtension);
                            var fichierTmp = await fileSavePicker.PickSaveFileAsync();
                            if (fichierTmp != null)
                            {
                                ViewModel.Fichier = fichierTmp;
                                EmplacementFichierTextBlock.Text = ViewModel.Fichier.Path;
                            }
                        break;
                }
            }
            catch
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("SelectionFichier"));
            }
            ParcourirButton.IsEnabled = true;
        }

        private async void ImportExport_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            ImportExportButton.IsEnabled = false;
            ParcourirButton.IsEnabled = false;
            WaitProgressRing.IsActive = true;
            var retour = ViewModel.Validate();

            if (string.IsNullOrWhiteSpace(retour))
            {
                switch (_mode)
                {
                    case 1:
                        var button =
                            await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("AttentionRestaurer"),
                                ResourceLoader.GetForCurrentView().GetString("Attention"), MessageBoxButton.YesNo);

                        if (button == MessageBoxResult.Yes)
                        {
                            if (await ViewModel.Restauration())
                            {
                                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("RestaurerOK"));
                            }
                            else
                            {
                                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("ErreurRestaurer"));
                            }
                        }
                        break;

                    case 2:
                        if (await ViewModel.Sauvegarde())
                        {
                            await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("SauvegardeOK"));
                        }
                        else
                        {
                            await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("ErreurSauvegarde"));
                        }
                        break;
                }
            }
            else
            {
                await MessageBox.ShowAsync(retour);
            }
            ImportExportButton.IsEnabled = true;
            ParcourirButton.IsEnabled = true;
            WaitProgressRing.IsActive = false;
            ((Button)sender).IsEnabled = true;
        }

    }
}
