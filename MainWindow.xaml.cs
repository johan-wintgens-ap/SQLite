using System;
using System.Collections.Generic;
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
using System.Data.SQLite;

namespace SQLlite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private SQLiteConnection mDbconnection;

        
        public MainWindow()
        {

            InitializeComponent();

            //Bestand linken
            mDbconnection = new SQLiteConnection("Data Source=school_sqlite;Version=3;");
            //Verbinding maken
            mDbconnection.Open();

            //SQL-Statement om alles uit de table "School" op te vragen
            string sql = "SELECT * FROM School ORDER BY score DESC";
            SQLiteCommand command = new SQLiteCommand(sql, mDbconnection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            //File uitlezen

            try
            {
                List<dbEntry> Entries = new List<dbEntry>();
                while (reader.Read())
                {
                    dbEntry temp = new dbEntry();
                    temp.id = reader.GetInt32(0);
                    temp.naamVak = reader.GetString(1);
                    temp.score = reader.GetInt32(2);
                    temp.datum = Convert.ToDateTime(reader.GetString(3));
                    temp.opmerking = reader.GetString(4);
                    temp.credit = reader.GetInt32(5);

                    vakkenListBox.Items.Add(temp);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                MessageBox.Show("Er is iets misgegaan bij het inladen van de DataBase");
            }

            mDbconnection.Close();
        }

        private void vakkenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }     
    }
}
