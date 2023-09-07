using System.Windows;

namespace _2048
{
    public partial class GameWonWindow : Window
    {
        public GameWonWindow(double width, double height, double left, double top)
        {
            InitializeComponent();
            Width = width;
            Height = height;
            Left = left;
            Top = top;
        }
    }
}
