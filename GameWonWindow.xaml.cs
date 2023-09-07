using System.Windows;

namespace _2048
{
    public partial class GameWonWindow : Window
    {
        public GameWonWindow(double width, double height, double left, double top)
        {
            InitializeComponent();
            this.Width = width;
            this.Height = height;
            this.Left = left;
            this.Top = top;
        }
    }
}
