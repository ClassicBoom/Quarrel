﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;
using Quarrel.ViewModels.Models.Bindables;

namespace Quarrel.Xaml.Behaviors
{
    class TriggerSemanticZoomAction : DependencyObject, IAction
    {

        public SemanticZoom SemanticZoom { get; set; }
        public Type Type { get; set; }

        public object Execute(object sender, object parameter)
        {
            if ((parameter as ItemClickEventArgs)?.ClickedItem?.GetType() == Type)
            {
                SemanticZoom.IsZoomedInViewActive = false;
            }
            return null;
        }
    }
}
