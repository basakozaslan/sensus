﻿using Sensus.DataStores;
using Sensus.DataStores.Local;
using Sensus.DataStores.Remote;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Sensus.UI
{
    public class CreateDataStorePage : ContentPage
    {
        public static event EventHandler<CreateDataStoreEventArgs> CreateDataStoreTapped;

        public class CreateDataStoreEventArgs : EventArgs
        {
            public DataStore DataStore { get; set; }
            public Protocol Protocol { get; set; }
            public bool Local { get; set; }
        }

        public CreateDataStorePage(Protocol protocol, bool local)
        {
            Title = "Create " + (local ? "Local" : "Remote") + " Data Store";

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
            };

            Type dataStoreType = local ? typeof(LocalDataStore) : typeof(RemoteDataStore);

            foreach (DataStore dataStore in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(dataStoreType)).Select(t => Activator.CreateInstance(t)))
            {
                Button createDataStoreButton = new Button
                {
                    Text = "New " + dataStore.Name + " Data Store"
                };

                createDataStoreButton.Clicked += (o, e) =>
                    {
                        CreateDataStoreTapped(o, new CreateDataStoreEventArgs { DataStore = dataStore, Protocol = protocol, Local = local });
                    };

                (Content as StackLayout).Children.Add(createDataStoreButton);
            }
        }
    }
}
