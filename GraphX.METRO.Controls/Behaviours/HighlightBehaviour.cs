﻿using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GraphX
{
    public static class HighlightBehaviour
    {
        #region Attached props
        //trigger
        public static readonly DependencyProperty HighlightedProperty = DependencyProperty.RegisterAttached("Highlighted", typeof(bool), typeof(HighlightBehaviour), new PropertyMetadata(false));
        //settings
        public static readonly DependencyProperty IsHighlightEnabledProperty = DependencyProperty.RegisterAttached("IsHighlightEnabled", typeof(bool), typeof(HighlightBehaviour), new PropertyMetadata(false, OnIsHighlightEnabledPropertyChanged));
        public static readonly DependencyProperty HighlightControlProperty = DependencyProperty.RegisterAttached("HighlightControl", typeof(GraphControlType), typeof(HighlightBehaviour), new PropertyMetadata(GraphControlType.VertexAndEdge));
        public static readonly DependencyProperty HighlightEdgesProperty = DependencyProperty.RegisterAttached("HighlightEdges", typeof(EdgesType), typeof(HighlightBehaviour), new PropertyMetadata(EdgesType.Out));
        public static readonly DependencyProperty HighlightStrategyProperty = DependencyProperty.RegisterAttached("HighlightStrategy", typeof(HighlightStrategy), typeof(HighlightBehaviour), new PropertyMetadata(HighlightStrategy.UseExistingControls));


        public static bool GetIsHighlightEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHighlightEnabledProperty);
        }

        public static void SetIsHighlightEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHighlightEnabledProperty, value);
        }

        public static bool GetHighlighted(DependencyObject obj)
        {
            return (bool)obj.GetValue(HighlightedProperty);
        }

        public static void SetHighlighted(DependencyObject obj, bool value)
        {
            obj.SetValue(HighlightedProperty, value);
        }

        public static GraphControlType GetHighlightControl(DependencyObject obj)
        {
            return (GraphControlType)obj.GetValue(HighlightControlProperty);
        }

        public static void SetHighlightControl(DependencyObject obj, GraphControlType value)
        {
            obj.SetValue(HighlightControlProperty, value);
        }

        public static EdgesType GetHighlightEdges(DependencyObject obj)
        {
            return (EdgesType)obj.GetValue(HighlightEdgesProperty);
        }

        public static void SetHighlightEdges(DependencyObject obj, EdgesType value)
        {
            obj.SetValue(HighlightEdgesProperty, value);
        }

        public static HighlightStrategy GetHighlightStrategy(DependencyObject obj)
        {
            return (HighlightStrategy)obj.GetValue(HighlightStrategyProperty);
        }

        public static void SetHighlightStrategy(DependencyObject obj, HighlightStrategy value)
        {
            obj.SetValue(HighlightStrategyProperty, value);
        }

        #endregion

        #region PropertyChanged callbacks
        private static void OnIsHighlightEnabledPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as FrameworkElement;
            if (element == null)
            {//REMOVED content element
                    return;
            }

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                //registering on the FrameworkElement
                element.PointerEntered += element_MouseEnter;
                element.PointerExited += element_MouseLeave;

                Debug.WriteLine("HighlightBehaviour registered.", "DEBUG");
            }
            else
            {
                //unregistering on the FrameworkElement
                element.PointerEntered -= element_MouseEnter;
                element.PointerExited -= element_MouseLeave;
                Debug.WriteLine("HighlightBehaviour unregistered.", "DEBUG");
            }
        }

        static void element_MouseLeave(object sender, PointerRoutedEventArgs e)
        {
            if (sender is DependencyObject == false) return;
            var ctrl = sender as IGraphControl;
            if (ctrl == null) return;

            var type = GetHighlightControl(sender as DependencyObject);
            var edgesType = GetHighlightEdges(sender as DependencyObject);
            SetHighlighted(sender as DependencyObject, false);
            foreach (var item in ctrl.RootArea.GetRelatedControls(ctrl, type, edgesType))
            {
                SetHighlighted(item as Control, false);
            }
        }

        static void element_MouseEnter(object sender, PointerRoutedEventArgs e)
        {
            if(sender is DependencyObject == false) return;
            var ctrl = sender as IGraphControl;
            if(ctrl == null) return;

            var type = GetHighlightControl(sender as DependencyObject);
            var edgesType = GetHighlightEdges(sender as DependencyObject);
            SetHighlighted(sender as DependencyObject, true);
            foreach (var item in ctrl.RootArea.GetRelatedControls(ctrl, type, edgesType))
            {
                SetHighlighted(item as Control, true);
            }
        }
        #endregion

        public enum HighlightType
        {
            Vertex,
            Edge,
            VertexAndEdge
        }
    }
}