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
        List<dbEntry> Entries = new List<dbEntry>();
        string sql;
        SQLiteCommand command;
        SQLiteDataReader reader;
        
        public MainWindow()
        {
            InitializeComponent();

            //Bestand linken
            mDbconnection = new SQLiteConnection("Data Source=school_sqlite;Version=3;");
            //Verbinding maken
            mDbconnection.Open();

            //SQL-Statement om alles uit de table "School" op te vragen
            sql = "SELECT * FROM School ORDER BY score DESC";
            command = new SQLiteCommand(sql, mDbconnection);
            reader = command.ExecuteReader();

            //File uitlezen
            try
            {
                while (reader.Read())
                {
                    dbEntry temp = new dbEntry();
                    temp.id = reader.GetInt32(0);
                    temp.naamVak = reader.GetString(1);
                    temp.score = reader.GetInt32(2);
                    temp.datum = Convert.ToDateTime(reader.GetString(3));
                    temp.opmerking = reader.GetString(4);
                    temp.credit = reader.GetInt32(5);

                    Entries.Add(temp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Er is iets misgegaan bij het inladen van de DataBase");
            }

            //In listbox laden
            try
            {
                foreach (var item in Entries)
                {
                    vakkenListBox.Items.Add(item);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Er is iets misgegaan bij het inladen van de gegevens in de listbox");
            }

            mDbconnection.Close();
        }

        private void vakkenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vakkenListBox.SelectedIndex != -1)
            {
                idBlock.Text = Convert.ToString(Entries[vakkenListBox.SelectedIndex].id);
                vakBlock.Text = Entries[vakkenListBox.SelectedIndex].naamVak;
                scoreBlock.Text = Convert.ToString(Entries[vakkenListBox.SelectedIndex].score);
                datumBlock.SelectedDate = Entries[vakkenListBox.SelectedIndex].datum;
                opmerkingBlock.Text = Entries[vakkenListBox.SelectedIndex].opmerking;
                creditBlock.Text = Convert.ToString(Entries[vakkenListBox.SelectedIndex].credit);
            }
        }     
    }
}
