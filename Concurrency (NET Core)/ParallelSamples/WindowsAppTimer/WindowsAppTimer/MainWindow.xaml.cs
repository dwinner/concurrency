﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WindowsAppTimer
{
   /// <summary>
   ///    An empty window that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class MainWindow : Window, INotifyPropertyChanged
   {
      private readonly DispatcherTimer _timer = new();

      private double _timerAngle;

      public MainWindow()
      {
         Title = "WinUI Dispatcher Timer App";
         InitializeComponent();
         _timer.Tick += OnTick;
         _timer.Interval = TimeSpan.FromSeconds(1);
      }

      public double TimerAngle
      {
         get => _timerAngle;
         set
         {
            if (!EqualityComparer<double>.Default.Equals(_timerAngle, value))
            {
               _timerAngle = value;
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimerAngle)));
            }
         }
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      private void OnStartTimer() => _timer.Start();

      private void OnTick(object? sender, object e) =>
         TimerAngle = (TimerAngle + 6) % 360;

      private void OnStopTimer() => _timer.Stop();
   }
}