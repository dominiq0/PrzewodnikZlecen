using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;
using System.Globalization;
//using System.Data.Sql;
using System.Windows.Markup;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Ookii.Dialogs.WinForms;
using System.Reflection;
using static PrzewodnikZlecen.MainWindow;
using System.Configuration;



namespace PrzewodnikZlecen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        ObservableCollection<ZlList> ZlecOBS = new ObservableCollection<ZlList>();
        ObservableCollection<DaneZlec> DaneZlecOB = new ObservableCollection<DaneZlec>();
        List<string> FileListpath = new List<string>();
           DateTime  oDate = Convert.ToDateTime(DataPl);


   







        static string connecString = "";

        string currentDirName = System.IO.Directory.GetCurrentDirectory();
        string PL_prod = "0122471_DALKIA_2022_GP.MET";

        
        private SQLiteConnection _conn;

        public object ConvertDS(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double sliderValue = (double)value;
            return new Thickness(sliderValue, 0, 0, 0);
        }


        public class DaneZlec
        {
            public string Pozycja { get; set; }
            public string Rodzaj { get; set; }
            public string Nazwa { get; set; }
            public string Opis { get; set; }
            public string Ilosc { get; set; }
            public string Jednostka { get; set; }
            public string DlProfila { get; set; }
            public string KatL { get; set; }
            public string KatP { get; set; }

        }


            public class ZlList
        {
            public string Nazwa { get; set; }
            public override string ToString()
            {
                return this.Nazwa;
            }
            public string lokalizacja { get; set; }
        }


        public MainWindow()
        {

       //     string connecString = currentDirName + "\\" + PL_prod;

            _conn = new SQLiteConnection("Data Source=" + connecString + ";Version=3;New=True;");
          //  ZlecOBS.Add(new ZlList() { Nazwa = "Zlec1" });
          //  ZlecOBS.Add(new ZlList() { Nazwa = "Zlec2" });

            



            InitializeComponent();
            
            LV.ItemsSource = ZlecOBS;
            ElementyV.ItemsSource = DaneZlecOB;

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            int hrz = Convert.ToInt32(UstawieniaMG.ReadSetting("SplitH"));
            grid1.RowDefinitions[3].Height = new GridLength(hrz, GridUnitType.Pixel);
            int vrt = Convert.ToInt32(UstawieniaMG.ReadSetting("SplitV"));
            grid1.ColumnDefinitions[2].Width = new GridLength(vrt, GridUnitType.Pixel);
            try
            {
                window1.Height = Convert.ToInt32(UstawieniaMG.ReadSetting("scrH"));// wysokośc okna
                window1.Width = Convert.ToInt32(UstawieniaMG.ReadSetting("scrW"));    // szerokosc okna
            }
            catch
            {
            }




            ZakresDanych zakresDanych = new ZakresDanych();
            zakresDanych.ZakDanych();
            
           foreach (string st in ZakresDanych.FileListpath )
            {

                FileInfo fin = new FileInfo(st);

                    string nazwa = fin.Name;

      
                ZlecOBS.Add(new ZlList() { Nazwa = nazwa, lokalizacja = st });


            }

        }

        private void ss_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
                UstawieniaMG.ReadAllSettings();
            string sds = UstawieniaMG.ReadSetting("Setting1");
               UstawieniaMG.ReadSetting("NotValid");
                UstawieniaMG.AddUpdateAppSettings("NewSetting", "May 7, 2014");
                UstawieniaMG.AddUpdateAppSettings("Setting1", "May 8, 2014");
                UstawieniaMG.ReadAllSettings();
            





          //  PicTConn();
        }
        public void PicTConn() {

            _conn = new SQLiteConnection("Data Source=" + connecString + ";Version=3;New=True;");

            SQLiteCommand getEmp = new SQLiteCommand(
       "SELECT Picture " +
       "FROM ConsData " +
       "WHERE ConIdx = '1'", _conn);


            SQLiteCommand getEmp2 = new SQLiteCommand("SELECT Data " + "FROM BinData " + "WHERE Idd = '-1'", _conn);
            _conn.Open();
            SQLiteDataReader myReader = getEmp.ExecuteReader(CommandBehavior.SequentialAccess);
            DaneZBloob daneZBloob = new DaneZBloob();
            ss.Source = daneZBloob.GetPictB(myReader, 0);
            //  GetBytes(myReader, 0);//dzalecie


            getEmp = new SQLiteCommand("SELECT * FROM 'LPData'", _conn);
            myReader = getEmp.ExecuteReader(CommandBehavior.SequentialAccess);
            DaneZlecOB.Clear();
            while (myReader.Read())
            {
                string sdad = myReader.GetValue(11).ToString();
                string TkatL = myReader.GetValue(11).ToString();
                if (TkatL.Equals("")) { TkatL = "0"; }
                string TKatP = myReader.GetValue(12).ToString();
                if (TKatP.Equals("")) { TKatP = "0"; }
                //  ZlecOBS.Add(new ZlList() { Nazwa = nazwa, lokalizacja = st });
                //	plany.Add(new Items {IDen = false,R = "  ", Nazwa = Path.GetFileName(s) });
                DaneZlecOB.Add(new DaneZlec { Pozycja =myReader.GetInt32(0).ToString(),

                    Rodzaj = myReader.GetString(1),
                    Nazwa = myReader.GetString(4),
                    Opis = myReader.GetString(5),
                    Ilosc = myReader.GetString(8),
                    Jednostka = myReader.GetString(9),
                    DlProfila = myReader.GetValue(10).ToString(),
                    KatL = TkatL,
                    KatP = TKatP


                });

           
            
            }



        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            LV.Visibility = Visibility.Collapsed;
            ss.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            LV.Visibility = Visibility.Visible;
            ss.Opacity = 0.3;
        }

      //  private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
     //   {
    //        Tg_Btn.IsChecked = false;
     //   }




        void gspl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string pos = grid1.RowDefinitions[2].Height.ToString();

            //  Qm.queryB = "UPDATE Settings SET  Wartosc ='" + pos + "' WHERE Nazwa = 'gsH';";
            //  Qm.ExecuteQuery();
            //	grid1.RowDefinitions[3].Height = new GridLength(285,GridUnitType.Star);
        }


      

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          int  index = this.LV.SelectedIndex;

            var item = this.LV.Items[index];
            var Nazwainfo = item.GetType().GetProperty("Nazwa"); // propertyInfo for test1
            var NazwaWartosc = Nazwainfo.GetValue(item, null);

            var scie = item.GetType().GetProperty("lokalizacja");
            connecString = scie.GetValue(item, null).ToString();

            PicTConn();


        }

        private void Unchecked(object sender, MouseEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }




        public class ToINTConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime date = (DateTime)value;
                return date.ToShortDateString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string strValue = value as string;
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
                return DependencyProperty.UnsetValue;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           

            UstawieniaMG.AddUpdateAppSettings("scrH", Convert.ToInt32(window1.Height).ToString());
            UstawieniaMG.AddUpdateAppSettings("ScrW", Convert.ToInt32(window1.Width).ToString());
            UstawieniaMG.AddUpdateAppSettings("SplitH", grid1.RowDefinitions[3].Height.ToString());
            UstawieniaMG.AddUpdateAppSettings("SplitV", grid1.ColumnDefinitions[2].Width.ToString());

        }

        private void ElementyV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ElementyV.SelectedItem != null)
            {



                int index = this.ElementyV.SelectedIndex;

                var item = this.ElementyV.Items[index];
                var Rlinfo = item.GetType().GetProperty("Opis"); // własciwosci dla R 
                var RWartosc = Rlinfo.GetValue(item, null);
                var Nazwainfo = item.GetType().GetProperty("Nazwa"); // propertyInfo for test1
                var NazwaWartosc = Nazwainfo.GetValue(item, null);
                string selLine = NazwaWartosc.ToString();

                try { 
                BitmapImage image = new BitmapImage(
                 new Uri(currentDirName + "\\RysTest\\" + selLine + ".png"));
                ElPic.Source = image;

                }
                catch
                {
                    BitmapImage image = new BitmapImage(
                 new Uri(currentDirName + "\\RysTest\\null.png"));
                    ElPic.Source = image;
                }



            }
        }
    }


 


}
