using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using CompteWin10.Model;

namespace CompteWin10.Utils
{
    /// <summary>
    /// Classe utilitaire contenant les devises et les pays
    /// </summary>
    public static class DeviseUtils
    {
        public static List<Pays> ListePays { get; } = new List<Pays>();

        public static List<Devise> ListeDevises { get; } = new List<Devise>();

        public static readonly int IdEuro = 2;

        /// <summary>
        /// Genere une liste de pays
        /// </summary>
        public static void GeneratePays()
        {
            ListePays.Clear();
            ListePays.Add(new Pays("AD", ResourceLoader.GetForCurrentView("Pays").GetString("AD")));
            ListePays.Add(new Pays("AE", ResourceLoader.GetForCurrentView("Pays").GetString("AE")));
            ListePays.Add(new Pays("AF", ResourceLoader.GetForCurrentView("Pays").GetString("AF")));
            ListePays.Add(new Pays("AG", ResourceLoader.GetForCurrentView("Pays").GetString("AG")));
            ListePays.Add(new Pays("AI", ResourceLoader.GetForCurrentView("Pays").GetString("AI")));
            ListePays.Add(new Pays("AL", ResourceLoader.GetForCurrentView("Pays").GetString("AL")));
            ListePays.Add(new Pays("AM", ResourceLoader.GetForCurrentView("Pays").GetString("AM")));
            ListePays.Add(new Pays("AO", ResourceLoader.GetForCurrentView("Pays").GetString("AO")));
            ListePays.Add(new Pays("AR", ResourceLoader.GetForCurrentView("Pays").GetString("AR")));
            ListePays.Add(new Pays("AT", ResourceLoader.GetForCurrentView("Pays").GetString("AT")));
            ListePays.Add(new Pays("AU", ResourceLoader.GetForCurrentView("Pays").GetString("AU")));
            ListePays.Add(new Pays("AW", ResourceLoader.GetForCurrentView("Pays").GetString("AW")));
            ListePays.Add(new Pays("AZ", ResourceLoader.GetForCurrentView("Pays").GetString("AZ")));
            ListePays.Add(new Pays("BA", ResourceLoader.GetForCurrentView("Pays").GetString("BA")));
            ListePays.Add(new Pays("BB", ResourceLoader.GetForCurrentView("Pays").GetString("BB")));
            ListePays.Add(new Pays("BD", ResourceLoader.GetForCurrentView("Pays").GetString("BD")));
            ListePays.Add(new Pays("BE", ResourceLoader.GetForCurrentView("Pays").GetString("BE")));
            ListePays.Add(new Pays("BF", ResourceLoader.GetForCurrentView("Pays").GetString("BF")));
            ListePays.Add(new Pays("BG", ResourceLoader.GetForCurrentView("Pays").GetString("BG")));
            ListePays.Add(new Pays("BH", ResourceLoader.GetForCurrentView("Pays").GetString("BH")));
            ListePays.Add(new Pays("BI", ResourceLoader.GetForCurrentView("Pays").GetString("BI")));
            ListePays.Add(new Pays("BJ", ResourceLoader.GetForCurrentView("Pays").GetString("BJ")));
            ListePays.Add(new Pays("BM", ResourceLoader.GetForCurrentView("Pays").GetString("BM")));
            ListePays.Add(new Pays("BN", ResourceLoader.GetForCurrentView("Pays").GetString("BN")));
            ListePays.Add(new Pays("BO", ResourceLoader.GetForCurrentView("Pays").GetString("BO")));
            ListePays.Add(new Pays("BR", ResourceLoader.GetForCurrentView("Pays").GetString("BR")));
            ListePays.Add(new Pays("BS", ResourceLoader.GetForCurrentView("Pays").GetString("BS")));
            ListePays.Add(new Pays("BW", ResourceLoader.GetForCurrentView("Pays").GetString("BW")));
            ListePays.Add(new Pays("BZ", ResourceLoader.GetForCurrentView("Pays").GetString("BZ")));
            ListePays.Add(new Pays("CA", ResourceLoader.GetForCurrentView("Pays").GetString("CA")));
            ListePays.Add(new Pays("CD", ResourceLoader.GetForCurrentView("Pays").GetString("CD")));
            ListePays.Add(new Pays("CF", ResourceLoader.GetForCurrentView("Pays").GetString("CF")));
            ListePays.Add(new Pays("CH", ResourceLoader.GetForCurrentView("Pays").GetString("CH")));
            ListePays.Add(new Pays("CI", ResourceLoader.GetForCurrentView("Pays").GetString("CI")));
            ListePays.Add(new Pays("CL", ResourceLoader.GetForCurrentView("Pays").GetString("CL")));
            ListePays.Add(new Pays("CM", ResourceLoader.GetForCurrentView("Pays").GetString("CM")));
            ListePays.Add(new Pays("CN", ResourceLoader.GetForCurrentView("Pays").GetString("CN")));
            ListePays.Add(new Pays("CO", ResourceLoader.GetForCurrentView("Pays").GetString("CO")));
            ListePays.Add(new Pays("CR", ResourceLoader.GetForCurrentView("Pays").GetString("CR")));
            ListePays.Add(new Pays("CS", ResourceLoader.GetForCurrentView("Pays").GetString("CS")));
            ListePays.Add(new Pays("CU", ResourceLoader.GetForCurrentView("Pays").GetString("CU")));
            ListePays.Add(new Pays("CV", ResourceLoader.GetForCurrentView("Pays").GetString("CV")));
            ListePays.Add(new Pays("CY", ResourceLoader.GetForCurrentView("Pays").GetString("CY")));
            ListePays.Add(new Pays("CZ", ResourceLoader.GetForCurrentView("Pays").GetString("CZ")));
            ListePays.Add(new Pays("DE", ResourceLoader.GetForCurrentView("Pays").GetString("DE")));
            ListePays.Add(new Pays("DJ", ResourceLoader.GetForCurrentView("Pays").GetString("DJ")));
            ListePays.Add(new Pays("DK", ResourceLoader.GetForCurrentView("Pays").GetString("DK")));
            ListePays.Add(new Pays("DM", ResourceLoader.GetForCurrentView("Pays").GetString("DM")));
            ListePays.Add(new Pays("DO", ResourceLoader.GetForCurrentView("Pays").GetString("DO")));
            ListePays.Add(new Pays("DZ", ResourceLoader.GetForCurrentView("Pays").GetString("DZ")));
            ListePays.Add(new Pays("EC", ResourceLoader.GetForCurrentView("Pays").GetString("EC")));
            ListePays.Add(new Pays("EE", ResourceLoader.GetForCurrentView("Pays").GetString("EE")));
            ListePays.Add(new Pays("EG", ResourceLoader.GetForCurrentView("Pays").GetString("EG")));
            ListePays.Add(new Pays("ER", ResourceLoader.GetForCurrentView("Pays").GetString("ER")));
            ListePays.Add(new Pays("ES", ResourceLoader.GetForCurrentView("Pays").GetString("ES")));
            ListePays.Add(new Pays("ET", ResourceLoader.GetForCurrentView("Pays").GetString("ET")));
            ListePays.Add(new Pays("FI", ResourceLoader.GetForCurrentView("Pays").GetString("FI")));
            ListePays.Add(new Pays("FJ", ResourceLoader.GetForCurrentView("Pays").GetString("FJ")));
            ListePays.Add(new Pays("FK", ResourceLoader.GetForCurrentView("Pays").GetString("FK")));
            ListePays.Add(new Pays("FM", ResourceLoader.GetForCurrentView("Pays").GetString("FM")));
            ListePays.Add(new Pays("FO", ResourceLoader.GetForCurrentView("Pays").GetString("FO")));
            ListePays.Add(new Pays("FR", ResourceLoader.GetForCurrentView("Pays").GetString("FR")));
            ListePays.Add(new Pays("GA", ResourceLoader.GetForCurrentView("Pays").GetString("GA")));
            ListePays.Add(new Pays("GB", ResourceLoader.GetForCurrentView("Pays").GetString("GB")));
            ListePays.Add(new Pays("GD", ResourceLoader.GetForCurrentView("Pays").GetString("GD")));
            ListePays.Add(new Pays("GE", ResourceLoader.GetForCurrentView("Pays").GetString("GE")));
            ListePays.Add(new Pays("GH", ResourceLoader.GetForCurrentView("Pays").GetString("GH")));
            ListePays.Add(new Pays("GL", ResourceLoader.GetForCurrentView("Pays").GetString("GL")));
            ListePays.Add(new Pays("GM", ResourceLoader.GetForCurrentView("Pays").GetString("GM")));
            ListePays.Add(new Pays("GN", ResourceLoader.GetForCurrentView("Pays").GetString("GN")));
            ListePays.Add(new Pays("GR", ResourceLoader.GetForCurrentView("Pays").GetString("GR")));
            ListePays.Add(new Pays("GT", ResourceLoader.GetForCurrentView("Pays").GetString("GT")));
            ListePays.Add(new Pays("GU", ResourceLoader.GetForCurrentView("Pays").GetString("GU")));
            ListePays.Add(new Pays("GW", ResourceLoader.GetForCurrentView("Pays").GetString("GW")));
            ListePays.Add(new Pays("GY", ResourceLoader.GetForCurrentView("Pays").GetString("GY")));
            ListePays.Add(new Pays("HK", ResourceLoader.GetForCurrentView("Pays").GetString("HK")));
            ListePays.Add(new Pays("HN", ResourceLoader.GetForCurrentView("Pays").GetString("HN")));
            ListePays.Add(new Pays("HR", ResourceLoader.GetForCurrentView("Pays").GetString("HR")));
            ListePays.Add(new Pays("HT", ResourceLoader.GetForCurrentView("Pays").GetString("HT")));
            ListePays.Add(new Pays("HU", ResourceLoader.GetForCurrentView("Pays").GetString("HU")));
            ListePays.Add(new Pays("ID", ResourceLoader.GetForCurrentView("Pays").GetString("ID")));
            ListePays.Add(new Pays("IE", ResourceLoader.GetForCurrentView("Pays").GetString("IE")));
            ListePays.Add(new Pays("IL", ResourceLoader.GetForCurrentView("Pays").GetString("IL")));
            ListePays.Add(new Pays("IN", ResourceLoader.GetForCurrentView("Pays").GetString("IN")));
            ListePays.Add(new Pays("IQ", ResourceLoader.GetForCurrentView("Pays").GetString("IQ")));
            ListePays.Add(new Pays("IR", ResourceLoader.GetForCurrentView("Pays").GetString("IR")));
            ListePays.Add(new Pays("IS", ResourceLoader.GetForCurrentView("Pays").GetString("IS")));
            ListePays.Add(new Pays("IT", ResourceLoader.GetForCurrentView("Pays").GetString("IT")));
            ListePays.Add(new Pays("JM", ResourceLoader.GetForCurrentView("Pays").GetString("JM")));
            ListePays.Add(new Pays("JO", ResourceLoader.GetForCurrentView("Pays").GetString("JO")));
            ListePays.Add(new Pays("JP", ResourceLoader.GetForCurrentView("Pays").GetString("JP")));
            ListePays.Add(new Pays("KE", ResourceLoader.GetForCurrentView("Pays").GetString("KE")));
            ListePays.Add(new Pays("KG", ResourceLoader.GetForCurrentView("Pays").GetString("KG")));
            ListePays.Add(new Pays("KH", ResourceLoader.GetForCurrentView("Pays").GetString("KH")));
            ListePays.Add(new Pays("KI", ResourceLoader.GetForCurrentView("Pays").GetString("KI")));
            ListePays.Add(new Pays("KM", ResourceLoader.GetForCurrentView("Pays").GetString("KM")));
            ListePays.Add(new Pays("KP", ResourceLoader.GetForCurrentView("Pays").GetString("KP")));
            ListePays.Add(new Pays("KR", ResourceLoader.GetForCurrentView("Pays").GetString("KR")));
            ListePays.Add(new Pays("KW", ResourceLoader.GetForCurrentView("Pays").GetString("KW")));
            ListePays.Add(new Pays("KZ", ResourceLoader.GetForCurrentView("Pays").GetString("KZ")));
            ListePays.Add(new Pays("LA", ResourceLoader.GetForCurrentView("Pays").GetString("LA")));
            ListePays.Add(new Pays("LB", ResourceLoader.GetForCurrentView("Pays").GetString("LB")));
            ListePays.Add(new Pays("LC", ResourceLoader.GetForCurrentView("Pays").GetString("LC")));
            ListePays.Add(new Pays("LI", ResourceLoader.GetForCurrentView("Pays").GetString("LI")));
            ListePays.Add(new Pays("LK", ResourceLoader.GetForCurrentView("Pays").GetString("LK")));
            ListePays.Add(new Pays("LR", ResourceLoader.GetForCurrentView("Pays").GetString("LR")));
            ListePays.Add(new Pays("LS", ResourceLoader.GetForCurrentView("Pays").GetString("LS")));
            ListePays.Add(new Pays("LT", ResourceLoader.GetForCurrentView("Pays").GetString("LT")));
            ListePays.Add(new Pays("LU", ResourceLoader.GetForCurrentView("Pays").GetString("LU")));
            ListePays.Add(new Pays("LV", ResourceLoader.GetForCurrentView("Pays").GetString("LV")));
            ListePays.Add(new Pays("LY", ResourceLoader.GetForCurrentView("Pays").GetString("LY")));
            ListePays.Add(new Pays("MA", ResourceLoader.GetForCurrentView("Pays").GetString("MA")));
            ListePays.Add(new Pays("MC", ResourceLoader.GetForCurrentView("Pays").GetString("MC")));
            ListePays.Add(new Pays("MD", ResourceLoader.GetForCurrentView("Pays").GetString("MD")));
            ListePays.Add(new Pays("MG", ResourceLoader.GetForCurrentView("Pays").GetString("MG")));
            ListePays.Add(new Pays("MK", ResourceLoader.GetForCurrentView("Pays").GetString("MK")));
            ListePays.Add(new Pays("ML", ResourceLoader.GetForCurrentView("Pays").GetString("ML")));
            ListePays.Add(new Pays("MM", ResourceLoader.GetForCurrentView("Pays").GetString("MM")));
            ListePays.Add(new Pays("MN", ResourceLoader.GetForCurrentView("Pays").GetString("MN")));
            ListePays.Add(new Pays("MR", ResourceLoader.GetForCurrentView("Pays").GetString("MR")));
            ListePays.Add(new Pays("MT", ResourceLoader.GetForCurrentView("Pays").GetString("MT")));
            ListePays.Add(new Pays("MV", ResourceLoader.GetForCurrentView("Pays").GetString("MV")));
            ListePays.Add(new Pays("MW", ResourceLoader.GetForCurrentView("Pays").GetString("MW")));
            ListePays.Add(new Pays("MX", ResourceLoader.GetForCurrentView("Pays").GetString("MX")));
            ListePays.Add(new Pays("MY", ResourceLoader.GetForCurrentView("Pays").GetString("MY")));
            ListePays.Add(new Pays("MZ", ResourceLoader.GetForCurrentView("Pays").GetString("MZ")));
            ListePays.Add(new Pays("NA", ResourceLoader.GetForCurrentView("Pays").GetString("NA")));
            ListePays.Add(new Pays("NE", ResourceLoader.GetForCurrentView("Pays").GetString("NE")));
            ListePays.Add(new Pays("NG", ResourceLoader.GetForCurrentView("Pays").GetString("NG")));
            ListePays.Add(new Pays("NI", ResourceLoader.GetForCurrentView("Pays").GetString("NI")));
            ListePays.Add(new Pays("NL", ResourceLoader.GetForCurrentView("Pays").GetString("NL")));
            ListePays.Add(new Pays("NO", ResourceLoader.GetForCurrentView("Pays").GetString("NO")));
            ListePays.Add(new Pays("NP", ResourceLoader.GetForCurrentView("Pays").GetString("NP")));
            ListePays.Add(new Pays("NR", ResourceLoader.GetForCurrentView("Pays").GetString("NR")));
            ListePays.Add(new Pays("NZ", ResourceLoader.GetForCurrentView("Pays").GetString("NZ")));
            ListePays.Add(new Pays("OM", ResourceLoader.GetForCurrentView("Pays").GetString("OM")));
            ListePays.Add(new Pays("PA", ResourceLoader.GetForCurrentView("Pays").GetString("PA")));
            ListePays.Add(new Pays("PE", ResourceLoader.GetForCurrentView("Pays").GetString("PE")));
            ListePays.Add(new Pays("PG", ResourceLoader.GetForCurrentView("Pays").GetString("PG")));
            ListePays.Add(new Pays("PH", ResourceLoader.GetForCurrentView("Pays").GetString("PH")));
            ListePays.Add(new Pays("PK", ResourceLoader.GetForCurrentView("Pays").GetString("PK")));
            ListePays.Add(new Pays("PL", ResourceLoader.GetForCurrentView("Pays").GetString("PL")));
            ListePays.Add(new Pays("PR", ResourceLoader.GetForCurrentView("Pays").GetString("PR")));
            ListePays.Add(new Pays("PS", ResourceLoader.GetForCurrentView("Pays").GetString("PS")));
            ListePays.Add(new Pays("PT", ResourceLoader.GetForCurrentView("Pays").GetString("PT")));
            ListePays.Add(new Pays("PY", ResourceLoader.GetForCurrentView("Pays").GetString("PY")));
            ListePays.Add(new Pays("QA", ResourceLoader.GetForCurrentView("Pays").GetString("QA")));
            ListePays.Add(new Pays("RO", ResourceLoader.GetForCurrentView("Pays").GetString("RO")));
            ListePays.Add(new Pays("RU", ResourceLoader.GetForCurrentView("Pays").GetString("RU")));
            ListePays.Add(new Pays("RW", ResourceLoader.GetForCurrentView("Pays").GetString("RW")));
            ListePays.Add(new Pays("SA", ResourceLoader.GetForCurrentView("Pays").GetString("SA")));
            ListePays.Add(new Pays("SC", ResourceLoader.GetForCurrentView("Pays").GetString("SC")));
            ListePays.Add(new Pays("SD", ResourceLoader.GetForCurrentView("Pays").GetString("SD")));
            ListePays.Add(new Pays("SE", ResourceLoader.GetForCurrentView("Pays").GetString("SE")));
            ListePays.Add(new Pays("SG", ResourceLoader.GetForCurrentView("Pays").GetString("SG")));
            ListePays.Add(new Pays("SI", ResourceLoader.GetForCurrentView("Pays").GetString("SI")));
            ListePays.Add(new Pays("SK", ResourceLoader.GetForCurrentView("Pays").GetString("SK")));
            ListePays.Add(new Pays("SL", ResourceLoader.GetForCurrentView("Pays").GetString("SL")));
            ListePays.Add(new Pays("SN", ResourceLoader.GetForCurrentView("Pays").GetString("SN")));
            ListePays.Add(new Pays("SO", ResourceLoader.GetForCurrentView("Pays").GetString("SO")));
            ListePays.Add(new Pays("SR", ResourceLoader.GetForCurrentView("Pays").GetString("SR")));
            ListePays.Add(new Pays("SY", ResourceLoader.GetForCurrentView("Pays").GetString("SY")));
            ListePays.Add(new Pays("SZ", ResourceLoader.GetForCurrentView("Pays").GetString("SZ")));
            ListePays.Add(new Pays("TD", ResourceLoader.GetForCurrentView("Pays").GetString("TD")));
            ListePays.Add(new Pays("TG", ResourceLoader.GetForCurrentView("Pays").GetString("TG")));
            ListePays.Add(new Pays("TH", ResourceLoader.GetForCurrentView("Pays").GetString("TH")));
            ListePays.Add(new Pays("TJ", ResourceLoader.GetForCurrentView("Pays").GetString("TJ")));
            ListePays.Add(new Pays("TL", ResourceLoader.GetForCurrentView("Pays").GetString("TL")));
            ListePays.Add(new Pays("TM", ResourceLoader.GetForCurrentView("Pays").GetString("TM")));
            ListePays.Add(new Pays("TN", ResourceLoader.GetForCurrentView("Pays").GetString("TN")));
            ListePays.Add(new Pays("TO", ResourceLoader.GetForCurrentView("Pays").GetString("TO")));
            ListePays.Add(new Pays("TR", ResourceLoader.GetForCurrentView("Pays").GetString("TR")));
            ListePays.Add(new Pays("TT", ResourceLoader.GetForCurrentView("Pays").GetString("TT")));
            ListePays.Add(new Pays("TV", ResourceLoader.GetForCurrentView("Pays").GetString("TV")));
            ListePays.Add(new Pays("TW", ResourceLoader.GetForCurrentView("Pays").GetString("TW")));
            ListePays.Add(new Pays("TZ", ResourceLoader.GetForCurrentView("Pays").GetString("TZ")));
            ListePays.Add(new Pays("UA", ResourceLoader.GetForCurrentView("Pays").GetString("UA")));
            ListePays.Add(new Pays("UG", ResourceLoader.GetForCurrentView("Pays").GetString("UG")));
            ListePays.Add(new Pays("US", ResourceLoader.GetForCurrentView("Pays").GetString("US")));
            ListePays.Add(new Pays("UY", ResourceLoader.GetForCurrentView("Pays").GetString("UY")));
            ListePays.Add(new Pays("UZ", ResourceLoader.GetForCurrentView("Pays").GetString("UZ")));
            ListePays.Add(new Pays("VA", ResourceLoader.GetForCurrentView("Pays").GetString("VA")));
            ListePays.Add(new Pays("VC", ResourceLoader.GetForCurrentView("Pays").GetString("VC")));
            ListePays.Add(new Pays("VE", ResourceLoader.GetForCurrentView("Pays").GetString("VE")));
            ListePays.Add(new Pays("VN", ResourceLoader.GetForCurrentView("Pays").GetString("VN")));
            ListePays.Add(new Pays("VU", ResourceLoader.GetForCurrentView("Pays").GetString("VU")));
            ListePays.Add(new Pays("WS", ResourceLoader.GetForCurrentView("Pays").GetString("WS")));
            ListePays.Add(new Pays("YE", ResourceLoader.GetForCurrentView("Pays").GetString("YE")));
            ListePays.Add(new Pays("ZA", ResourceLoader.GetForCurrentView("Pays").GetString("ZA")));
            ListePays.Add(new Pays("ZM", ResourceLoader.GetForCurrentView("Pays").GetString("ZM")));
            ListePays.Add(new Pays("ZW", ResourceLoader.GetForCurrentView("Pays").GetString("ZW")));
            ListePays.Sort((x, y) => string.CompareOrdinal(x.Libelle, y.Libelle));
        }

        /// <summary>
        /// Genere les devises
        /// </summary>
        public static void GenerateDevise()
        {
            ListeDevises.Clear();
            //Afrique
            ListeDevises.Add(new Devise(144, "dinar", "DT", 0.45479, new List<string> { "TN" }));
            ListeDevises.Add(new Devise(143, "metical", "metical", 0.02121, new List<string> { "MZ" }));
            ListeDevises.Add(new Devise(142, "dinar", "dinar", 0.66641, new List<string> { "LY" }));
            ListeDevises.Add(new Devise(141, "dollar", "dollar", 0.01070, new List<string> { "LR" }));
            ListeDevises.Add(new Devise(1, "Dinar", "د.ج", 0.00848, new List<string> { "DZ" }));
            ListeDevises.Add(new Devise(3, "Dirham", "د.م", 0.092420, new List<string> { "MA" }));
            ListeDevises.Add(new Devise(4, "Ouguiya",   "أوقية", 0.00304778, new List<string> { "MR" }));
            ListeDevises.Add(new Devise(5, "Livre", "s£", 0.15, new List<string> { "SD" }));
            ListeDevises.Add(new Devise(6, "Franc CFA(UEMA)", "F CFA", 0.00152449, new List<string> { "BJ","BF","CI","GW","ML","NE","SN","TG" }));
            ListeDevises.Add(new Devise(7, "escudo", "escudo", 0.00906900, new List<string> { "CV" }));
            ListeDevises.Add(new Devise(8, "dalasi", "dalasi", 0.02, new List<string> { "GM" }));
            ListeDevises.Add(new Devise(9, "Cedi", "₵", 0.23911, new List<string> { "GH" }));
            ListeDevises.Add(new Devise(10, "Franc guinéen", "F", 0.000117293, new List<string> { "GN" }));
            ListeDevises.Add(new Devise(11, "Naira", "N", 0.0045, new List<string> { "NG" }));
            ListeDevises.Add(new Devise(12, "leone", "leone", 0.000201296, new List<string> { "SL" }));
            ListeDevises.Add(new Devise(13, "Franc CFA(CEMAC)", "F CFA", 0.0015, new List<string> { "CM", "CF", "CD", "GA", "TD" }));
            ListeDevises.Add(new Devise(14, "franc", "F", 0.00098, new List<string> { "CD" }));
            ListeDevises.Add(new Devise(15, "franc", "F", 0.000583516, new List<string> { "BI" }));
            ListeDevises.Add(new Devise(16, "franc", "F", 0.0051, new List<string> { "DJ" }));
            ListeDevises.Add(new Devise(17, "nafka", "nafka", 0.0592, new List<string> { "ER" }));
            ListeDevises.Add(new Devise(18, "birr", "birr", 0.0431, new List<string> { "ET" }));
            ListeDevises.Add(new Devise(19, "shilling", "shilling", 0.0089, new List<string> { "KE" }));
            ListeDevises.Add(new Devise(20, "shilling", "shilling", 0.0003, new List<string> { "UG" }));
            ListeDevises.Add(new Devise(21, "franc", "F", 0.0012, new List<string> { "RW" }));
            ListeDevises.Add(new Devise(22, "roupie", "roupie", 0.0700, new List<string> { "SC" }));
            ListeDevises.Add(new Devise(23, "shilling somalien", "shilling", 0.0014, new List<string> { "SO" }));
            ListeDevises.Add(new Devise(24, "shilling tanzanien", "shilling", 0.0004, new List<string> { "TZ" }));
            ListeDevises.Add(new Devise(25, "rand", "R", 0.0662, new List<string> { "ZA" }));
            ListeDevises.Add(new Devise(26, "Livre", "E£", 0.11, new List<string> { "EG", "PS" }));
            ListeDevises.Add(new Devise(27, "kwanza", "Kz", 0.0067, new List<string> { "AO" }));
            ListeDevises.Add(new Devise(28, "pula", "pula", 0.0866, new List<string> { "BW" }));
            ListeDevises.Add(new Devise(29, "franc comorien", "F", 0.0020, new List<string> { "KM" }));
            ListeDevises.Add(new Devise(30, "loti", "loti", 0.0661, new List<string> { "LS" }));
            ListeDevises.Add(new Devise(31, "ariary", "ariary", 0.0003, new List<string> { "MG" }));
            ListeDevises.Add(new Devise(32, "kwacha malawien", "kwacha", 0.0016, new List<string> { "MW" }));
            ListeDevises.Add(new Devise(33, "dollar namibien", "dollar", 0.0661, new List<string> { "NA" }));
            ListeDevises.Add(new Devise(34, "lilangeni", "lilangeni", 0.0661, new List<string> { "SZ" }));
            ListeDevises.Add(new Devise(35, "kwacha de Zambie", "kwacha", 0.0002, new List<string> { "ZM" }));

            //asie
            ListeDevises.Add(new Devise(36, "tenge", "tеңге", 0.0033, new List<string> { "KZ" }));
            ListeDevises.Add(new Devise(37, "som", "som", 0.0131, new List<string> { "KG" }));
            ListeDevises.Add(new Devise(38, "sum", "sum", 0.0003, new List<string> { "UZ" }));
            ListeDevises.Add(new Devise(39, "somoni", "somoni", 0.1370, new List<string> { "TJ" }));
            ListeDevises.Add(new Devise(40, "manat turkmène", "manat", 0.0001, new List<string> { "TM" }));
            ListeDevises.Add(new Devise(41, "dram", "դրամ", 0.0019, new List<string> { "AM" }));
            ListeDevises.Add(new Devise(42, "manat", "lilangeni", 0.8651, new List<string> { "AZ" }));
            ListeDevises.Add(new Devise(43, "lari", "ლარი", 0.3764, new List<string> { "GE" }));
            ListeDevises.Add(new Devise(44, "rouble", "₽", 0.0142, new List<string> { "RU" }));
            ListeDevises.Add(new Devise(45, "Yen", "Y", 0.00748, new List<string> { "JP" }));
            ListeDevises.Add(new Devise(46, "yuan", "Ұ", 0.1425, new List<string> { "CN" }));
            ListeDevises.Add(new Devise(47, "Won", "￦", 0.1425, new List<string> { "KP" }));
            ListeDevises.Add(new Devise(48, "Won", "￦", 0.0008, new List<string> { "KR" }));
            ListeDevises.Add(new Devise(49, "Dollar", "$", 0.1168, new List<string> { "HK" }));
            ListeDevises.Add(new Devise(50, "tugrik", "Y", 0.0005, new List<string> { "MN" }));
            ListeDevises.Add(new Devise(51, "Dollar", "NT$", 0.0089, new List<string> { "TW" }));
            ListeDevises.Add(new Devise(52, "Afghani", "؋", 0.01, new List<string> { "AF" }));
            ListeDevises.Add(new Devise(53, "riyal", "riyal", 0.01, new List<string> { "SA" }));
            ListeDevises.Add(new Devise(54, "dinar", "dinar", 0.2414, new List<string> { "BH" }));
            ListeDevises.Add(new Devise(55, "dirham", "DH", 0.2465, new List<string> { "AE" }));
            ListeDevises.Add(new Devise(56, "rial", "ریال ایران", 0.00003, new List<string> { "IR" }));
            ListeDevises.Add(new Devise(57, "dinar", "dinar", 0.00076, new List<string> { "IQ" }));
            ListeDevises.Add(new Devise(58, "shekel", "₪", 0.23383, new List<string> { "IL","PS" }));
            ListeDevises.Add(new Devise(59, "dinar", "dinar", 1.27402, new List<string> { "JO", "PS" }));
            ListeDevises.Add(new Devise(60, "dinar", "dinar", 2.984, new List<string> { "KW" }));
            ListeDevises.Add(new Devise(61, "livre", "£L", 0.00060, new List<string> { "LB" }));
            ListeDevises.Add(new Devise(62, "rial", "rial", 2.34752, new List<string> { "OM" }));
            ListeDevises.Add(new Devise(63, "riyal", "rial", 0.24828, new List<string> { "QA" }));
            ListeDevises.Add(new Devise(64, "livre", "S£", 0.00479, new List<string> { "SY" }));
            ListeDevises.Add(new Devise(65, "livre", "£", 0.31180, new List<string> { "TR" }));
            ListeDevises.Add(new Devise(66, "riyal", "rial", 0.00421, new List<string> { "YE" }));
            ListeDevises.Add(new Devise(67, "kyat", "kyat", 0.00071, new List<string> { "MM" }));
            ListeDevises.Add(new Devise(68, "dollar", "B$", 0.64944, new List<string> { "BN" }));
            ListeDevises.Add(new Devise(69, "riel", "riel", 0.00022, new List<string> { "KH" }));
            ListeDevises.Add(new Devise(70, "rupiah", "roupie", 0.00007, new List<string> { "ID" }));
            ListeDevises.Add(new Devise(71, "kip", "₭", 0.00011, new List<string> { "LA" }));
            ListeDevises.Add(new Devise(72, "ringgit", "RM", 0.21226, new List<string> { "MY" }));
            ListeDevises.Add(new Devise(73, "peso", "peso", 0.01934, new List<string> { "PH" }));
            ListeDevises.Add(new Devise(74, "dollar", "S$", 0.64937, new List<string> { "SG" }));
            ListeDevises.Add(new Devise(75, "baht", "฿", 0.02549, new List<string> { "TH" }));
            ListeDevises.Add(new Devise(76, "dong", "đồng", 0.00004, new List<string> { "VN" }));
            ListeDevises.Add(new Devise(77, "taka", "taka", 0.01161, new List<string> { "BD" }));
            ListeDevises.Add(new Devise(78, "ngultrum", "Nu", 0.01392, new List<string> { "BT" }));
            ListeDevises.Add(new Devise(79, "roupie", "₹", 0.01392, new List<string> { "IN" }));
            ListeDevises.Add(new Devise(80, "rufiyaa", "rf", 0.05875, new List<string> { "MV" }));
            ListeDevises.Add(new Devise(81, "roupie", "रूपैयाँ", 0.00870, new List<string> { "NP" }));
            ListeDevises.Add(new Devise(82, "roupie", "Rs", 0.00860, new List<string> { "PK" }));
            ListeDevises.Add(new Devise(83, "roupie", "Rs", 0.00641, new List<string> { "LK" }));

            //amérique
            ListeDevises.Add(new Devise(84, "Dollar US", "$", 0.907, new List<string> { "US", "ZW", "PR", "EC", "PA", "GU", "TL", "FM", "WS", "NL" }));
            ListeDevises.Add(new Devise(85, "Dollar", "$", 0.90359, new List<string> { "BM" }));
            ListeDevises.Add(new Devise(86, "Dollar", "$", 0.68538, new List<string> { "CA" }));
            ListeDevises.Add(new Devise(87, "peso", "$", 0.05458, new List<string> { "MX" }));
            ListeDevises.Add(new Devise(88, "dollar", "BZ$", 0.45271, new List<string> { "BZ" }));
            ListeDevises.Add(new Devise(89, "colón", "colón", 0.00169, new List<string> { "CR" }));
            ListeDevises.Add(new Devise(90, "quetzal", "quetzal", 0.11734, new List<string> { "GT" }));
            ListeDevises.Add(new Devise(91, "lempira", "lempira", 0.04094, new List<string> { "HN" }));
            ListeDevises.Add(new Devise(92, "córdoba", "córdoba", 0.03261, new List<string> { "NI" }));
            ListeDevises.Add(new Devise(93, "balboa", "balboa", 0.90325, new List<string> { "PA" }));
            ListeDevises.Add(new Devise(94, "peso", "$", 0.09480, new List<string> { "AR" }));
            ListeDevises.Add(new Devise(95, "boliviano", "boliviano", 0.13068, new List<string> { "BO" }));
            ListeDevises.Add(new Devise(96, "reais", "R$", 0.23161, new List<string> { "BR" }));
            ListeDevises.Add(new Devise(97, "peso", "$", 0.00131, new List<string> { "CL" }));
            ListeDevises.Add(new Devise(98, "peso", "$", 0.00031, new List<string> { "CO" }));
            ListeDevises.Add(new Devise(99, "livre", "£", 1.39379, new List<string> { "FK" }));
            ListeDevises.Add(new Devise(100, "dollar", "G$", 0.00436, new List<string> { "GY" }));
            ListeDevises.Add(new Devise(101, "guaraní", "₲", 0.00016, new List<string> { "PY" }));
            ListeDevises.Add(new Devise(102, "nuevo sol", "S/", 0.27631, new List<string> { "PE" }));
            ListeDevises.Add(new Devise(103, "dollar", "dollar", 0.27393, new List<string> { "SR" }));
            ListeDevises.Add(new Devise(104, "peso", "$", 0.03068, new List<string> { "UY" }));
            ListeDevises.Add(new Devise(105, "bolívar", "bolívar", 0.14228, new List<string> { "VE" }));
            ListeDevises.Add(new Devise(106, "dollar", "TTD", 0.14262, new List<string> { "TT" }));
            ListeDevises.Add(new Devise(107, "dollar", "EC$", 0.33495, new List<string> { "LC","VC", "GD", "DM", "AI", "AG" }));
            ListeDevises.Add(new Devise(108, "dollar", "B$", 0.90408, new List<string> { "BH" }));
            ListeDevises.Add(new Devise(109, "dollar", "Bds$", 0.45212, new List<string> { "BB" }));
            ListeDevises.Add(new Devise(110, "peso", "$", 0.90424, new List<string> { "CU" }));
            ListeDevises.Add(new Devise(111, "peso", "$", 0.01996, new List<string> { "DO" }));
            ListeDevises.Add(new Devise(112, "gourde", "gourde", 0.01697, new List<string> { "HT" }));
            ListeDevises.Add(new Devise(113, "dollar", "J$", 0.00758, new List<string> { "JM" }));
            ListeDevises.Add(new Devise(139, "Florin", "Afl", 0.50617, new List<string> { "AW" }));
            ListeDevises.Add(new Devise(140, "dollar", "B$", 0.90535, new List<string> { "BS" }));

            //europe
            ListeDevises.Add(new Devise(IdEuro, "Euro","€", 1, new List<string> {"FR","ES","AD","DE","GR","IT","AT","FI","PT","CY","IR","NL","SK","LV","LT","BE","EE","SI","MT","MC","LU","VA","ZW","IE"}));
            ListeDevises.Add(new Devise(114, "Livre", "L", 1.54, new List<string> { "GB", "UK", "ZW" }));
            ListeDevises.Add(new Devise(115, "kroner", "kr", 0.13406, new List<string> { "DK", "GL", "FO" }));
            ListeDevises.Add(new Devise(116, "lev", "лев", 0.51173, new List<string> { "BG" }));
            ListeDevises.Add(new Devise(117, "kuna", "kuna", 0.13122, new List<string> { "HR" }));
            ListeDevises.Add(new Devise(118, "Forin", "Ft", 0.00320, new List<string> { "HU" }));
            ListeDevises.Add(new Devise(119, "złoty", "złoty", 0.23351, new List<string> { "PL" }));
            ListeDevises.Add(new Devise(120, "krona", "krona", 0.10654, new List<string> { "SE" }));
            ListeDevises.Add(new Devise(121, "koruna", "koruna", 0.03690, new List<string> { "CZ" }));
            ListeDevises.Add(new Devise(122, "leu", "leu", 0.22517, new List<string> { "RO" }));
            ListeDevises.Add(new Devise(123, "lek", "lek", 0.00719, new List<string> { "AL" }));
            ListeDevises.Add(new Devise(124, "króna", "króna", 0.00704, new List<string> { "IS" }));
            ListeDevises.Add(new Devise(125, "Franc suisse", "CHF", 0.91918, new List<string> { "CH","LI" }));
            ListeDevises.Add(new Devise(126, "mark", "марка", 0.51061, new List<string> { "BA" }));
            ListeDevises.Add(new Devise(127, "denar", "ден", 0.01623, new List<string> { "MK" }));
            ListeDevises.Add(new Devise(128, "leu", "leu", 0.04526, new List<string> { "MD" }));
            ListeDevises.Add(new Devise(129, "krone", "krone", 0.10687, new List<string> { "NO" }));
            ListeDevises.Add(new Devise(130, "dinar", "РСД ", 0.00829, new List<string> { "CS" }));
            ListeDevises.Add(new Devise(131, "hryvnia", "₴", 0.03950, new List<string> { "UA" }));

            //océanie
            ListeDevises.Add(new Devise(132, "dollar", "$", 0.65405, new List<string> { "AU", "KI", "NR" , "TV"}));
            ListeDevises.Add(new Devise(133, "dollar", "$", 0.42357, new List<string> { "FJ" }));
            ListeDevises.Add(new Devise(134, "dollar", "$", 0.61553, new List<string> { "NZ" }));
            ListeDevises.Add(new Devise(135, "kina", "kina", 0.31134, new List<string> { "PG" }));
            ListeDevises.Add(new Devise(136, "tala", "tala", 0.35042, new List<string> { "WS" }));
            ListeDevises.Add(new Devise(137, "paʻanga", "paʻanga", 0.41689, new List<string> { "TO" }));
            ListeDevises.Add(new Devise(138, "vatu", "vatu", 0.00804, new List<string> { "VU" }));

            foreach (var devise in ListeDevises)
            {
                var listePaysDevise = ListePays.Where(x => devise.IdPaysListe.Contains(x.Id)).ToList();
                foreach (var pays in listePaysDevise)
                {
                    pays.Devises.Add(devise);
                }
            }
            ListeDevises.Sort((x, y) => string.CompareOrdinal(x.Libelle, y.Libelle));
        }

        /// <summary>
        /// Converti une somme entre deux devises
        /// </summary>
        /// <param name="idDeviseActuelle">l'id de la devise de la monnaie actuel</param>
        /// <param name="idDeviseAttendue">l'id de la devise attendue</param>
        /// <param name="valeur"></param>
        /// <returns>la monnaie converti</returns>
        public static double ConvertisseurMonnaie(int idDeviseActuelle, int idDeviseAttendue, double valeur)
        {
            if (valeur > 0)
            {
                var tauxDeviseActuelle = ListeDevises.FirstOrDefault(x => x.Id == idDeviseActuelle).TauxChangeEuro;
                var tauxDeviseAttendue = ListeDevises.FirstOrDefault(x => x.Id == idDeviseAttendue).TauxChangeEuro;

                var valeurEuro = valeur*tauxDeviseActuelle;
                return Math.Round(valeurEuro / tauxDeviseAttendue,2);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Retourne une devise à partir de son id
        /// </summary>
        /// <param name="id">l'id de la devise</param>
        /// <returns>la devise</returns>
        public static Devise GetDevise(int id)
        {
            return ListeDevises.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Retourne le signe d'une devise
        /// </summary>
        /// <param name="id">l'id de la devise</param>
        /// <returns>le signe de la devise</returns>
        public static string GetDiminutifDevise(int id)
        {
            return ListeDevises.FirstOrDefault(x => x.Id == id).Signe;
        }

        /// <summary>
        /// Retourne un pays à partir de son id
        /// </summary>
        /// <param name="id">l'id du pays</param>
        /// <returns>le pays</returns>
        public static Pays GetPays(string id)
        {
            return ListePays.FirstOrDefault(x => x.Id == id);
        }

    }
}
