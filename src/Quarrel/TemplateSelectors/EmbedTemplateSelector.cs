﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Quarrel.Controls.Messages.Embeds;
using DiscordAPI.Models;

namespace Quarrel.TemplateSelectors
{
    /// <summary>
    /// A template selector for the line to display in the console view
    /// </summary>
    public sealed class EmbedTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (container is FrameworkElement parent)
            {
                switch ((item as Embed).Type)
                {
                    case "image": return parent.FindResource<DataTemplate>("ImageEmbedTemplate");
                }
            }

            return null;
        }
    }
}
