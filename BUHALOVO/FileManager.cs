using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BUHALOVO
{
    public class FileManager
    {
        public static void Serialize<T>(ObservableCollection<T> list, string fileName)
        {
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText($"{fileName}.json", $"\r\n{json}");
        }


        public static ObservableCollection<T> Deserialization<T>(string fileName)
        {
            string info = File.ReadAllText($"{fileName}.json");
            ObservableCollection<T> list = JsonConvert.DeserializeObject<ObservableCollection<T>>(info);
            return list;
        }
    }
    public class MyMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values - это массив значений Text из TextBox и ComboBox
            // parameter - это дополнительный параметр, который можно передать в xaml
            // culture - это культура текущего потока

            // Проверяем, что values не null и содержит три элемента
            if (values != null && values.Length == 3)
            {
                // Приводим каждый элемент к строке и проверяем его на пустоту или пробелы
                string text1 = (string)values[0];
                string text2 = (string)values[1];
                string text3 = (string)values[2];

                return !string.IsNullOrWhiteSpace(text1) && !string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3);
            }

            // Возвращаем false по умолчанию
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
