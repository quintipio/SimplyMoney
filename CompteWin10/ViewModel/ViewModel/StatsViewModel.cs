using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// juste une petite classe pour l'affichage des stats
    /// </summary>
    public class StructGraph
    {
        public double Value { get; set; }
        public string Categ { get; set; }

        public StructGraph(double value, string categ)
        {
            Value = value;
            Categ = categ;
        }
    }


    /// <summary>
    /// ViewModel des stats
    /// </summary>
    public partial class StatsViewModel : AbstractViewModel
    {
        private CompteBusiness _compteBusiness;

        private MouvementBusiness _mouvementBusiness;

        private CategorieBusiness _categorieBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public StatsViewModel()
        {
            Initialization = InitializeAsync();
        }

        public async sealed override Task InitializeAsync()
        {
             _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _mouvementBusiness = new MouvementBusiness();
            await _mouvementBusiness.Initialization;

            _categorieBusiness = new CategorieBusiness();
            await _categorieBusiness.Initialization;


            GenererListePeriode();
            await GenererListeCompte();
            GenererListeCategorie();
        }

        /// <summary>
        /// génère les périodes
        /// </summary>
        private void GenererListePeriode()
        {
            PeriodeListe = new ObservableCollection<EnumModel>();
            PeriodeListe.Add(new EnumModel(0,ResourceLoader.GetForCurrentView().GetString("AucunText")));
            PeriodeListe.Add(new EnumModel(1, ResourceLoader.GetForCurrentView().GetString("1anText")));
            PeriodeListe.Add(new EnumModel(2, ResourceLoader.GetForCurrentView().GetString("6moisText")));
            PeriodeListe.Add(new EnumModel(3, ResourceLoader.GetForCurrentView().GetString("3moisText")));
            PeriodeListe.Add(new EnumModel(4, ResourceLoader.GetForCurrentView().GetString("1moisText")));
            PeriodeListe.Add(new EnumModel(5, ResourceLoader.GetForCurrentView().GetString("7joursText")));
        }

        /// <summary>
        /// Genere la liste de comtpes
        /// </summary>
        private async Task GenererListeCompte()
        {
            ListeCompte = new ObservableCollection<Compte> {new Compte {Id = 0, Nom = ""}};

            var data = await _compteBusiness.GetResumeCompte();
            foreach (var banque in data)
            {
                foreach (var compte in banque.ListeCompte)
                {
                    compte.Nom = banque.Nom + " : " + compte.Nom;
                    ListeCompte.Add(compte);
                }
            }
        }

        /// <summary>
        /// Génère une liste des catégories
        /// </summary>
        private void GenererListeCategorie()
        {
            CategorieListe = new ObservableCollection<Categorie>();
            CategorieListe.Add(new Categorie(0, ResourceLoader.GetForCurrentView().GetString("AucunText"),true));

            foreach (var category in ContexteAppli.ListeCategoriesMouvement)
            {
                CategorieListe.Add(category);
            }
        }

        /// <summary>
        /// Génère une date datemin pour les stats
        /// </summary>
        /// <param name="dateMax">la dateMax</param>
        /// <returns>la date min</returns>
        private DateTime GenererDateMin(DateTime dateMax)
        {
            switch (SelectedPeriode.Id)
            {
                case 0:
                    return new DateTime();

                case 1:
                    return DateUtils.SoustraireJours(dateMax, 365);

                case 2:
                    return DateUtils.SoustraireJours(dateMax, 275);

                case 3:
                    return DateUtils.SoustraireJours(dateMax, 91);

                case 4:
                    return DateUtils.SoustraireJours(dateMax, 31);

                case 5:
                    return DateUtils.SoustraireJours(dateMax, 7);

                default:
                    return new DateTime();
            }
        }

        /// <summary>
        /// retourne une liste de mouvement pour un compte
        /// </summary>
        /// <param name="idCompte">l'id du compte des mouvements attendu(0 si aucun)</param>
        /// <returns>la liste des mouvements</returns>
        public async Task<List<Mouvement>> GetStatsDepenseCourbeCompte(int idCompte)
        {
            var dateMax = DateUtils.GetMaintenant();
            var dateMin = GenererDateMin(dateMax);
            var liste = await _mouvementBusiness.GetListeMouvement(idCompte,dateMin,dateMax);

            if (liste.Count > 0)
            {
                foreach (var mouvement in liste)
                {
                    mouvement.Chiffre = (mouvement.Debit > 0) ? mouvement.Debit*-1 : mouvement.Credit;
                }
            }

            return liste;
        }

        /// <summary>
        /// retourne une structure de donnée pour le graphique par camembert des catégories
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<StructGraph>> GetStatsDepenseCategorie()
        {
            var dateMax = DateUtils.GetMaintenant();
            var dateMin = GenererDateMin(dateMax);
            var retour = new List<StructGraph>();
            if (SelectedCategorie != null && SelectedCategorie.Id > 0)
            {
                var liste = await _categorieBusiness.GetMouvementByCategorie(SelectedCategorie.Id, SelectedCategorie.IsCategPerso, dateMin, dateMax, SelectedCompte.Id);
                retour.AddRange(from item in liste let somme = item.Value.Sum(x => x.Debit) select new StructGraph(somme, item.Key.Libelle));
            }
            else
            {
                var liste = await _categorieBusiness.GetMouvementByCategorie(dateMin, dateMax, SelectedCompte.Id);
                retour.AddRange(from item in liste let somme = item.Value.Sum(x => x.Debit) select new StructGraph(somme, item.Key.Libelle));
            }
            return retour;
        }
    }
}
