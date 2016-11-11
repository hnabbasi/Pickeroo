using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;

namespace Pickeroo.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title {
            get { return _title; }
            set { SetProperty (ref _title, value); }
        }

        private string _byTitle;
        public string ByTitle {
            get { return _byTitle; }
            set { SetProperty (ref _byTitle, value); }
        }

        public MainPageViewModel ()
        {
            Title = "Research a model";

            Names = new List<string>
            {
                "Hussain",
                "Nasir",
                "Abbasi",
                "Zoya"
            };

            SelectedName = Names.ElementAt (2);

            Cars = new List<Car>
            {
                new Car(1998, "Toyota", "Corolla"),
                new Car(2000, "Honda", "Pilot"),
                new Car(2016, "BMW", "435i")
            };

            SelectedCar = Cars.ElementAt (1);
        }

        public void OnNavigatedFrom (NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo (NavigationParameters parameters)
        {
        }

        public List<string> PickerList
        {
            get {
                var retVal = new List<string> ();
                foreach (var car in Cars){
                    retVal.Add (car.Model);
                }
                return retVal;
            }
        }

        private List<Car> _cars;
        public List<Car> Cars {
            get { return _cars;}
            set { SetProperty (ref _cars, value);}
        }

        private Car _car;
        public Car SelectedCar {
            get { return _car; }
            set 
            { 
                SetProperty (ref _car, value); 
                Title = $"Selection: {_car.DisplayText}"; 
            }
        }

        private List<string> _names;
        private List<string> Names {
            get { return _names; }
            set { SetProperty (ref _names, value);}
        }

        private string _name;
        private string SelectedName {
            get { return _name; }
            set 
            { 
                SetProperty (ref _name, value);
                ByTitle = $"Selected by: {_name}";
            }
        }
    }

    public class Car : IPickerItem
    {
        public Car (int year, string make, string model)
        {
            Year = year;
            Make = make;
            Model = model;
        }

        public int Year {
            get;
            set;
        }

        public string Make {
            get;
            set;
        }

        public string Model {
            get;
            set;
        }

        public string DisplayText {
            get {
                return $"{Year} {Make} {Model}";
            }
        }
    }
}

