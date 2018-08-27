using System;
using System.Windows;
using System.Windows.Threading;

namespace Toast
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }
 
        //Close the notification window
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
