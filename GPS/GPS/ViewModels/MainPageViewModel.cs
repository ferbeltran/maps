using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace GPS.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        protected IPageDialogService DialogService { get; private set; }

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base (navigationService)
        {
            Title = "Main Page";
            Location = "No Location";
            Distance = 0;
            DialogService = dialogService;
        }

        private ObservableCollection<Pin> pins = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }

        private ObservableCollection<Position> points = new ObservableCollection<Position>();
        public ObservableCollection<Position> Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }

        private Position currentPosition;
        public Position CurrentPosition 
        {
            get { return currentPosition; }
            set { SetProperty(ref currentPosition, value); }
        }   

        private string location;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        private double distance    ;
        public double Distance
        {
            get { return distance; }
            set { SetProperty(ref distance, value); }
        }

        private DelegateCommand pinAndPointLocationCommand;
        public DelegateCommand PinAndPointLocationCommand =>
            pinAndPointLocationCommand ?? (pinAndPointLocationCommand = new DelegateCommand(PinAndPointLocation));

        private DelegateCommand finishAreaCommand;
        public DelegateCommand FinishAreaCommand =>
            finishAreaCommand ?? (finishAreaCommand = new DelegateCommand(FinishArea));

        async void FinishArea()
        {
            if (Pins.Count < 3)
            {
                await DialogService.DisplayAlertAsync("Error", "You need at least 3 points to generate an area", "OK");
                return;
            }



        }


        private async Task<Position> GetCurrentPosition(GeolocationRequest request)
        {
            Position myPosition = new Position();
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                myPosition = new Position(location.Latitude, location.Longitude);
            }

            return myPosition;
        }


        async void PinAndPointLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);

                var pos = await GetCurrentPosition(request);
                Pins.Add(new Pin() {
                    Position = pos,
                    Label = "Pin"
                });

                Points.Add(pos);

            }

            catch(PermissionException pEx)
            {

            }
        }

        private void CalculateDistanceBetweenTwoPositions()
        {
            Location firstLocation = new Location(Pins[0].Position.Latitude, Pins[0].Position.Longitude);
            Location secondLocation = new Location(Pins[1].Position.Latitude, Pins[1].Position.Longitude);
            var km = Xamarin.Essentials.Location.CalculateDistance(firstLocation, secondLocation, DistanceUnits.Kilometers);
            Distance = (km / 1000);
        }


        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            CurrentPosition = await GetCurrentPosition(request);
        }
    }
}
