using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace IlyosClient;
public partial class AimTrainerWindow : Window
{
    private readonly Random random = new();
    private int hits;
    public AimTrainerWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => SpawnTarget();
        TrainingCanvas.SizeChanged += (_, _) => SpawnTarget();
    }
    private void SpawnTarget()
    {
        if (TrainingCanvas.ActualWidth < 50 || TrainingCanvas.ActualHeight < 50)
            return;
        TrainingCanvas.Children.Clear();
        var target = new Ellipse
        {
            Width = 34, Height = 34,
            Fill = new SolidColorBrush(Color.FromRgb(39, 135, 255)),
            Stroke = Brushes.White, StrokeThickness = 1, Cursor = Cursors.Hand
        };
        target.MouseLeftButtonDown += (_, e) =>
        {
            e.Handled = true;
            hits++;
            ResultText.Text = $"Попаданий: {hits}";
            SpawnTarget();
        };
        Canvas.SetLeft(target, random.Next(0, Math.Max(1, (int)TrainingCanvas.ActualWidth - 34)));
        Canvas.SetTop(target, random.Next(0, Math.Max(1, (int)TrainingCanvas.ActualHeight - 34)));
        TrainingCanvas.Children.Add(target);
    }
    private void Canvas_Click(object sender, MouseButtonEventArgs e)
    {
        ResultText.Text = $"Попаданий: {hits} — клик мимо.";
    }
}
