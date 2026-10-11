using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
namespace IlyosClient;
public partial class MainWindow : Window
{
    private readonly Random random = new();
    private readonly DispatcherTimer timer = new();
    private DateTime targetTime;
    private bool waiting;
    private bool signalShown;
    private readonly Stopwatch stopwatch = new();
    public MainWindow()
    {
        InitializeComponent();
        timer.Interval = TimeSpan.FromMilliseconds(20);
        timer.Tick += (_, _) =>
        {
            if (waiting && !signalShown && DateTime.Now >= targetTime)
            {
                signalShown = true;
                stopwatch.Restart();
                ReactionButton.Content = "ЖМИ СЕЙЧАС!";
                ReactionButton.Background = Brushes.SeaGreen;
                StatusText.Text = "Сигнал! Нажми кнопку.";
            }
        };
    }
    private void Home_Click(object sender, RoutedEventArgs e)
    {
        PageTitle.Text = "Главная";
        PageDescription.Text = "Твой тренировочный центр.";
    }
    private void Training_Click(object sender, RoutedEventArgs e)
    {
        PageTitle.Text = "Тренировка";
        PageDescription.Text = "Выбери упражнение для тренировки реакции и точности.";
        StatusText.Text = "Готов к тренировке.";
    }
    private void About_Click(object sender, RoutedEventArgs e)
    {
        PageTitle.Text = "О программе";
        PageDescription.Text = "Ilyos Client • Preview 0.1.0 • Windows x64";
        StatusText.Text = ".NET 8 WPF • автономный тренировочный интерфейс.";
    }
    private void Aim_Click(object sender, RoutedEventArgs e)
    {
        new AimTrainerWindow { Owner = this }.ShowDialog();
    }
    private void Reaction_Click(object sender, RoutedEventArgs e)
    {
        if (!waiting)
        {
            waiting = true;
            signalShown = false;
            targetTime = DateTime.Now.AddMilliseconds(random.Next(1500, 4000));
            ReactionButton.Content = "Жди сигнала...";
            ReactionButton.Background = new SolidColorBrush(Color.FromRgb(39, 135, 255));
            StatusText.Text = "Не нажимай до изменения цвета.";
            timer.Start();
            return;
        }
        if (!signalShown)
        {
            timer.Stop();
            waiting = false;
            ReactionButton.Content = "Слишком рано — снова";
            ReactionButton.Background = new SolidColorBrush(Color.FromRgb(190, 65, 75));
            StatusText.Text = "Фальстарт! Нажми кнопку, чтобы попробовать снова.";
            return;
        }
        long ms = stopwatch.ElapsedMilliseconds;
        timer.Stop();
        waiting = false;
        signalShown = false;
        ReactionButton.Content = "Начать снова";
        ReactionButton.Background = new SolidColorBrush(Color.FromRgb(39, 135, 255));
        StatusText.Text = $"Время реакции: {ms} мс.";
    }
}
