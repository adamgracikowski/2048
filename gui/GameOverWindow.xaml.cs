using System.Windows;

namespace _2048
{
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(double width, double height, double left, double top)
        {
            InitializeComponent();
            Width = width;
            Height = height;
            Left = left;
            Top = top;
        }
    }
}
