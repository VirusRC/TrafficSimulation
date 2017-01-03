using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DroneControlCalculation
{
    /// <summary>
    /// This project is used to perform the control calculation for the drone.
    /// It reformats the incoming coordinates from the camera system and the unity app to a common format.
    /// Since the drone has 7 degrees of freedom, Multi-Threading is used.
    /// If one of those system send the coordinates faster, a buffer needs to be implemented. 
    /// This buffer should then calculates the mean of the coordinates between certain timestamps (This means that timestamps of incoming coordinates have to be saved.)
    /// To enable a higher performance the coordinates should be brought from float into integer.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Debug.Print(IPCWrapper.IPCWrapper.Intf_client_InitConfiguration(".", "testpipename").ToString());
            Debug.Print(IPCWrapper.IPCWrapper.Intf_client_ClientConnectToServerPipe().ToString());

            for (int i = 0; i < 10; i++)
            {
                Debug.Print(IPCWrapper.IPCWrapper.Intf_client_ClientSendMessage("This is almost custom text!!!").ToString());
            }








		}
    }
}
