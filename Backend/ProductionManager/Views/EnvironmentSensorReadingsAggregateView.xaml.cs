﻿using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class EnvironmentSensorReadingsAggregateView : IEnvironmentSensorReadingsAggregateView
    {
        public EnvironmentSensorReadingsAggregateView(IEnvironmentSensorReadingsAggregateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IEnvironmentSensorReadingsAggregateViewModel EnvironmentSensorReadingsAggregateViewModel => DataContext as IEnvironmentSensorReadingsAggregateViewModel;
    }
}