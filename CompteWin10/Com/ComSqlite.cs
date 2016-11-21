using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using CompteWin10.Context;
using CompteWin10.Model;
using SQLite;

namespace CompteWin10.Com
{
    /// <summary>
    /// Singleton pour la connexion à la base de donnée SQLite (nécéssite le paquet qlite-net)
    /// </summary>
    public class ComSqlite
    {
        /// <summary>
        /// objet du singleton
        /// </summary>
        private static ComSqlite _comSqlite;

        /// <summary>
        /// connexion à la base
        /// </summary>
        public SQLiteAsyncConnection Connection { get;}

        private static string _path;
        private static string _fileName;


        /// <summary>
        /// retourne une instance de l'objet de connexion à la base de donnée
        /// </summary>
        /// <returns>l'instance de connexion</returns>
        public static async Task<ComSqlite> GetComSqlite()
        {
            if (_comSqlite == null)
            {
                _fileName = ContexteStatic.Database;
                _path = Path.Combine(ApplicationData.Current.LocalFolder.Path, ContexteStatic.Database);
                _comSqlite = new ComSqlite(_path);
            }
            return _comSqlite;
        }

        /// <summary>
        /// Constructeur créant la base si elle n'existe et vérifie si il faut effectuer des mises à jour
        /// </summary>
        /// <param name="nomFichier">le nom du fichier de la base de donnée</param>
        private ComSqlite(string nomFichier)
        {
            Connection = new SQLiteAsyncConnection(nomFichier);
        }

        /// <summary>
        /// vérifi l'existence d'une base de donnée
        /// </summary>
        /// <returns>true si existant</returns>
        public async Task<bool> CheckDbExist()
        {
            try
            {
                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(_fileName);
                if (item != null)
                {
                    var t = await Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE  name='Application' ");
                    return t > 0;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        

        /// <summary>
        /// Créer la base de donnée
        /// </summary>
        public async Task CreateDb()
        {
            await Connection.CreateTableAsync<Application>();
            await Connection.CreateTableAsync<Banque>();
            await Connection.CreateTableAsync<Compte>();
            await Connection.CreateTableAsync<Echeancier>();
            await Connection.CreateTableAsync<Mouvement>();
            await Connection.CreateTableAsync<Categorie>();
            await Connection.CreateTableAsync<SousCategorie>();
            await Connection.CreateTableAsync<SoldeInitial>();

            var resultatAppli = await Connection.Table<Application>().Where(x => x.Id == 1).CountAsync();
            if (resultatAppli == 0)
            {
                var app = new Application
                {
                    Id = 1,
                    Version = ContexteStatic.Version
                };
                await Connection.InsertAsync(app);
            }

        }
        
        /// <summary>
        /// Efface le fichier de la base de donnée
        /// </summary>
        /// <returns>la task</returns>
        public async Task DeleteDatabase()
        {
            await Connection.DropTableAsync<Application>();
            await Connection.DropTableAsync<Banque>();
            await Connection.DropTableAsync<Compte>();
            await Connection.DropTableAsync<Echeancier>();
            await Connection.DropTableAsync<Mouvement>();
            await Connection.DropTableAsync<Categorie>();
            await Connection.DropTableAsync<SousCategorie>();
            await Connection.DropTableAsync<SoldeInitial>();
        }


        /// <summary>
        /// ajoute une donnée en base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la donnée</param>
        public async Task AjouterDonnee<T>(T data)
        {
            await Connection.InsertAsync(data);
        }

        /// <summary>
        /// Ajoute une liste de donnée à la base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task AjouterListeDonnee<T>(IEnumerable<T> data)
        {
            await Connection.InsertAllAsync(data);
        }

        /// <summary>
        /// met à jour une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> UpdateDonnee<T>(T data)
        {
            return await Connection.UpdateAsync(data);
        }

        /// <summary>
        /// Met à jour plusieurs données
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task<int> UpdateListeDonnee<T>(IEnumerable<T> data)
        {
            return await Connection.UpdateAllAsync(data);
        }

        /// <summary>
        /// efface une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> DeleteDonnee<T>(T data)
        {
            return await Connection.DeleteAsync(data);
        }

        /// <summary>
        /// Efface une liste de données
        /// </summary>
        /// <typeparam name="T">le type de données à effacer</typeparam>
        /// <param name="data">la liste de données à effacer</param>
        /// <returns>le nombre de ligne effacé</returns>
        public async Task<int> DeleteListeDonnee<T>(IEnumerable<T> data)
        {
            var i = 0;
            foreach (var dataToDelete in data)
            {
                await Connection.DeleteAsync(dataToDelete);
                i++;
            }
            return i;
        }


    }
}
