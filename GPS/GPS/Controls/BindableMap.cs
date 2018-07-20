using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPS.Controls
{
    public class BindableMap : Map
    {

        public BindableMap()
        {

        }

        public IList<Position> ShapeCoordinates
        {
            get { return (IList<Position>)GetValue(ShapeCoordinatesProperty); }
            set { SetValue(ShapeCoordinatesProperty, value); }
        }


        public static readonly BindableProperty ShapeCoordinatesProperty = BindableProperty.Create(
                 nameof(ShapeCoordinates),
                 typeof(ObservableCollection<Position>),
                 typeof(BindableMap),
                 new ObservableCollection<Position>(),
                 propertyChanged: (b, o, n) =>
                 {
                     var bindable = (BindableMap)b;
                     bindable.ShapeCoordinates.Clear();

                     var collection = (ObservableCollection<Position>)n;
                     foreach (var item in collection)
                         bindable.ShapeCoordinates.Add(item);
                     collection.CollectionChanged += (sender, e) =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             switch (e.Action)
                             {
                                 case NotifyCollectionChangedAction.Add:
                                 case NotifyCollectionChangedAction.Replace:
                                 case NotifyCollectionChangedAction.Remove:
                                     if (e.OldItems != null)
                                         foreach (var item in e.OldItems)
                                             bindable.ShapeCoordinates.Remove((Position)item);
                                     if (e.NewItems != null)
                                         foreach (var item in e.NewItems)
                                             bindable.ShapeCoordinates.Add((Position)item);
                                     break;
                                 case NotifyCollectionChangedAction.Reset:
                                     bindable.Pins.Clear();
                                     break;
                             }
                         });
                     };
                 });




        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
                 nameof(Pins),
                 typeof(ObservableCollection<Pin>),
                 typeof(BindableMap),
                 new ObservableCollection<Pin>(),
                 propertyChanged: (b, o, n) =>
                 {
                     var bindable = (BindableMap)b;
                     bindable.Pins.Clear();

                     var collection = (ObservableCollection<Pin>)n;
                     foreach (var item in collection)
                         bindable.Pins.Add(item);
                     collection.CollectionChanged += (sender, e) =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             switch (e.Action)
                             {
                                 case NotifyCollectionChangedAction.Add:
                                 case NotifyCollectionChangedAction.Replace:
                                 case NotifyCollectionChangedAction.Remove:
                                     if (e.OldItems != null)
                                         foreach (var item in e.OldItems)
                                             bindable.Pins.Remove((Pin)item);
                                     if (e.NewItems != null)
                                         foreach (var item in e.NewItems)
                                             bindable.Pins.Add((Pin)item);
                                     break;
                                 case NotifyCollectionChangedAction.Reset:
                                     bindable.Pins.Clear();
                                     break;
                             }
                         });
                     };
                 });
        public IList<Pin> MapPins { get; set; }

   

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
                 nameof(MapPosition),
                 typeof(Position),
                 typeof(BindableMap),
                 new Position(0, 0),
                 propertyChanged: (b, o, n) =>
                 {
                     ((BindableMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                          (Position)n,
                          Distance.FromMeters(500)));
                 });

        public Position MapPosition { get; set; }
    }
}
