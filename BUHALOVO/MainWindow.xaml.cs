using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BUHALOVO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static double FullMoney {  get; set; }
        static int selectedIndex;
        static int noteIndex;
        static bool IsSelected = false;
        static DateOnly date = new();
        public static ObservableCollection<string> types = new();
        static ObservableCollection<Note> notes = new();
        static ObservableCollection<Note> notesOnDays = new();
        static Note note;
        public MainWindow()
        {
            InitializeComponent();
            notes = FileManager.Deserialization<Note>("notes");
            types = FileManager.Deserialization<string>("types");
            ChoiceTypeMenu.ItemsSource = types;
            DatePic.SelectedDate = DateTime.Now;
        }
        private void Create()
        {
            Note note = new(NoteNameTBox.Text, ChoiceTypeMenu.Text, Double.Parse(NoteMoneyTBox.Text), true, date);
            notes.Add(note);
            notesOnDays.Add(note);
            FileManager.Serialize(notes, "notes");
            TextChange();
            NoteInfo(-1);
        }

        private void Update(int index)
        {
            if (index != -1)
            {
                double amount = Double.Parse(NoteMoneyTBox.Text);
                notesOnDays[index].Name = NoteNameTBox.Text;
                notesOnDays[index].AmountOfMoney = amount;
                notesOnDays[index].Type = ChoiceTypeMenu.Text;
                notesOnDays[index].IsReceipt = amount >= 0;
                notes[noteIndex].Name = NoteNameTBox.Text;
                notes[noteIndex].AmountOfMoney = amount;
                notes[noteIndex].Type = ChoiceTypeMenu.Text;
                NoteDataGrid.ItemsSource = null;
                NoteDataGrid.ItemsSource = notesOnDays;
                FileManager.Serialize(notes, "notes");
                TextChange();
            }
        }

        private void Delete(int index)
        {
            if (index != -1)
            {
                notesOnDays.RemoveAt(index);
                notes.RemoveAt(noteIndex);
                FileManager.Serialize(notes, "notes");
                TextChange();
            }
        }

        private void CreateNewType_Click(object sender, RoutedEventArgs e)
        {
            NewTypeWin();
        }
        private void NewTypeWin()
        {
            CreateTypeWin win = new();
            win.Show();
        }

        private void CreateNote_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private void ChangeNote_Click(object sender, RoutedEventArgs e)
        {
            Update(selectedIndex);
        }

        private void DelNote_Click(object sender, RoutedEventArgs e)
        {
            Delete(selectedIndex);
        }

        private void NoteDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            note = NoteDataGrid.SelectedItem as Note;
            selectedIndex = NoteDataGrid.SelectedIndex;
            if (note != null)
            {
                IsSelected = true;
            }
            FindIndex();
            NoteInfo(selectedIndex);
        }

        private void FindIndex()
        {
            int a = 0;
            foreach (var item in notes)
            {
                if (note == item)
                {
                    noteIndex = a;
                    break;
                }
                a++;
            }
        }

        private void NoteInfo(int index)
        {
            if (index != -1)
            {
                string t = notesOnDays[index].Type;
                NoteNameTBox.Text = notesOnDays[index].Name;
                NoteMoneyTBox.Text = notesOnDays[index].AmountOfMoney.ToString();
                try
                {
                    ChoiceTypeMenu.Text = t;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                NoteNameTBox.Text = null;
                NoteMoneyTBox.Text = null;
                ChoiceTypeMenu.Text = null;
            }
        }

        private void NoteMoneyTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,-]+"); // регулярное выражение для проверки вводимых данных
            e.Handled = regex.IsMatch(e.Text);

            if (e.Handled == false)
            {
                string text = ((TextBox)sender).Text.Insert(((TextBox)sender).CaretIndex, e.Text);
                int count = text.Count(x => x == ',');
                int count2 = text.Count(x => x == '-');
                if (count > 1 || (count == 1 && text.First() == ','))
                {
                    e.Handled = true;
                }
                if (count2 > 1)
                {
                    e.Handled = true;
                }
            }
        }

        private void ChoiceTypeMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChoiceTypeMenu.Text = (string)ChoiceTypeMenu.SelectedValue;
        }

        private void TextChange()
        {
            FullMoney = 0;
            foreach (var item in notes)
            {
                FullMoney += item.AmountOfMoney;
            }
            MoneyCounter.Text = $"Итог: {FullMoney}";
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FullMoney = 0;
            notesOnDays.Clear();
            NoteDataGrid.ItemsSource = null;
            DateTime? dateTime = DatePic.SelectedDate;
            date = dateTime.HasValue ? new DateOnly(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day) : DateOnly.MinValue;
            foreach (var note in notes)
            {
                if (note.Date == date)
                {
                    notesOnDays.Add(note);
                }
            }
            NoteDataGrid.ItemsSource = notesOnDays;
            TextChange();
        }
    }
}
