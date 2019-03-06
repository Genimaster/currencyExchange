using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using currencyExchange.Models;
using System.Linq;
using System.Xml;

namespace currencyExchange
{
    public partial class MainPage : ContentPage
    {

        private Dictionary<string, string> PickerItems = new Dictionary<string, string>();

        //private List<CurrencyList> CurrenciesList = new List<CurrencyList>();

        public MainPage()
        {
            InitializeComponent();
            PickerItems.Add("EUR", "1");
            GetExchangeRatesAPI();
            inputCurrencyPicker.ItemsSource = PickerItemList;
            inputCurrencyPicker.SelectedIndex = 0;
            outputCurrencyPicker.ItemsSource = PickerItemList;
            outputCurrencyPicker.SelectedIndex = 1;
            OutputCurrencyEntry.IsEnabled = false;
        }

        public List<KeyValuePair<string, string>> PickerItemList
        {
            get => PickerItems.ToList();
        }

        private KeyValuePair<string, string> _selectedItem;
        public KeyValuePair<string, string> SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        public async void GetExchangeRatesAPI()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            if (response.IsSuccessStatusCode)
            {
                XmlReader xmlReader = XmlReader.Create("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
                PickerItems.Clear();
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Cube"))
                    {
                        if (xmlReader.HasAttributes)
                        {
                            if(!string.IsNullOrEmpty(xmlReader.GetAttribute("currency")) && !string.IsNullOrEmpty(xmlReader.GetAttribute("rate")))
                            {
                                /*
                                CurrencyList currencies = new CurrencyList
                                {
                                    Base = false,
                                    Date = DateTime.Now,
                                    Name = xmlReader.GetAttribute("currency"),
                                    Rate = xmlReader.GetAttribute("rate")
                                };
                                CurrenciesList.Add(currencies);
                                */
                                PickerItems.Add(xmlReader.GetAttribute("currency"), xmlReader.GetAttribute("rate"));

                            }
                        }
                    }
                }
            }
            //testLabel.Text = CurrenciesList.ToString();
        }

        private async void InputCurrencyEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputCurrencyPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please select a Country", "OK");
                return;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    OutputCurrencyEntry.Text = e.NewTextValue + " - " + inputCurrencyPicker.SelectedItem.ToString();
                }
                else
                {
                    OutputCurrencyEntry.Text = "";
                }
            }
        }
    }
}
