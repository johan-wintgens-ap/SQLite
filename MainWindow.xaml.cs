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
        dbEntry selectedEntry;
        bool writeToDatabase;
        
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
            readFile();

            //In listbox laden
            updateListbox();
            
            //Connectie sluiten
            mDbconnection.Close();
        }

        private void vakkenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vakkenListBox.SelectedIndex != -1)
            {
                selectedEntry = Entries[vakkenListBox.SelectedIndex];

                idBlock.Text = Convert.ToString(selectedEntry.id);
                vakBlock.Text = selectedEntry.naamVak;
                scoreBlock.Text = Convert.ToString(selectedEntry.score);
                datumBlock.SelectedDate = selectedEntry.datum;
                opmerkingBlock.Text = selectedEntry.opmerking;
                creditBlock.Text = Convert.ToString(selectedEntry.credit);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //savePressed detecteerd of er op de save-knop gedrukt wordt. Indien dit het geval is wordt de database met de nieuwe gegevens overschreven bij het afsluiten van het programma
            writeToDatabase = true;
            selectedEntry = Entries[vakkenListBox.SelectedIndex];

            selectedEntry.naamVak = vakBlock.Text;
            selectedEntry.score = Convert.ToInt32(scoreBlock.Text);
            selectedEntry.datum = (DateTime)datumBlock.SelectedDate;
            selectedEntry.opmerking = opmerkingBlock.Text;
            selectedEntry.credit = Convert.ToInt32(creditBlock.Text);

            updateListbox();
        }

        private void updateListbox()
        {
            try
            {
                int rememberSelected = vakkenListBox.SelectedIndex;
                vakkenListBox.Items.Clear();

                foreach (var item in Entries)
                {
                    vakkenListBox.Items.Add(item);
                }

                vakkenListBox.SelectedIndex = rememberSelected;
            }
            catch (Exception e)
            {
                MessageBox.Show("Er is iets misgegaan bij het laden van de gegevens in de listbox");
            }
        }

        private void readFile()
        {
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
        }

        private void addCourseButton_Click(object sender, RoutedEventArgs e)
        {
            writeToDatabase = true;
        }
    }
}
