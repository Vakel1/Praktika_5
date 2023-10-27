using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawingApp
{
    public partial class MainWindow : Window
    {
        private enum DrawingMode { None, Line, Circle, Square }
        private DrawingMode currentMode = DrawingMode.None;
        private Point startPoint;
        private Shape currentShape;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(drawingCanvas);

            switch (currentMode)
            {
                case DrawingMode.Line:
                    currentShape = new Line
                    {
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y,
                        Stroke = Brushes.Black
                    };
                    break;

                case DrawingMode.Circle:
                    currentShape = new Ellipse
                    {
                        Width = 0,
                        Height = 0,
                        Stroke = Brushes.Black
                    };
                    Canvas.SetLeft(currentShape, startPoint.X);
                    Canvas.SetTop(currentShape, startPoint.Y);
                    break;

                case DrawingMode.Square:
                    currentShape = new Rectangle
                    {
                        Width = 0,
                        Height = 0,
                        Stroke = Brushes.Black
                    };
                    Canvas.SetLeft(currentShape, startPoint.X);
                    Canvas.SetTop(currentShape, startPoint.Y);
                    break;

                case DrawingMode.None:
                    return; // Не добавлять фигуры, если режим не выбран
            }

            drawingCanvas.Children.Add(currentShape);
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (currentShape != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(drawingCanvas);

                switch (currentMode)
                {
                    case DrawingMode.Line:
                        var line = (Line)currentShape;
                        line.X2 = currentPoint.X;
                        line.Y2 = currentPoint.Y;
                        break;

                    case DrawingMode.Circle:
                    case DrawingMode.Square:
                        double width = currentPoint.X - startPoint.X;
                        double height = currentPoint.Y - startPoint.Y;
                        double size = Math.Max(width, height);

                        if (currentMode == DrawingMode.Circle)
                        {
                            var ellipse = (Ellipse)currentShape;
                            if (size < 0)
                            {
                                ellipse.Width = 0;
                                ellipse.Height = 0;
                            }
                            else
                            {
                                ellipse.Width = size;
                                ellipse.Height = size;
                            }
                        }
                        else if (currentMode == DrawingMode.Square)
                        {
                            var rectangle = (Rectangle)currentShape;
                            if (size < 0)
                            {
                                rectangle.Width = 0;
                                rectangle.Height = 0;
                            }
                            else
                            {
                                rectangle.Width = size;
                                rectangle.Height = size;
                            }
                        }
                        break;
                }
            }
        }

        private void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            currentShape = null;
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Children.Clear();
        }

        private void OnLineButtonClick(object sender, RoutedEventArgs e)
        {
            currentMode = DrawingMode.Line;
        }

        private void OnCircleButtonClick(object sender, RoutedEventArgs e)
        {
            currentMode = DrawingMode.Circle;
        }

        private void OnSquareButtonClick(object sender, RoutedEventArgs e)
        {
            currentMode = DrawingMode.Square;
        }
    }
}
