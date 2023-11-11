#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProgressCircle.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    ///     Represents a <see cref="ProgressCircle"/> control.
    /// </summary>
    public class ProgressCircle : Control
    {
        private readonly Timer _timer;
        private double[] _angles;
        private PointF _centerPoint;
        private Color[] _colors;
        private Color _foreColor;
        private int _innerRadius = 6;
        private bool _isActive;
        private int _outerRadius = 7;
        private int _progressValue;
        private int _spokes = 9;
        private int _thickness = 4;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ProgressCircle"/>
        ///     is active.
        /// </summary>
        public bool Active
        {
            get => _isActive;
            set
            {
                _isActive = value;
                ActiveTimer();
            }
        }

        /// <summary>
        ///     Gets or sets the number of spokes.
        /// </summary>
        public int Spokes
        {
            get
            {
                if (_spokes <= 0)
                    _spokes = 9;
                return _spokes;
            }
            set
            {
                if (_spokes == value || _spokes <= 0)
                    return;
                _spokes = value;
                GenerateColorsPallet();
                GetSpokesAngles();
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the spoke thickness.
        /// </summary>
        public int Thickness
        {
            get
            {
                if (_thickness <= 0)
                    _thickness = 4;
                return _thickness;
            }
            set
            {
                _thickness = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the inner circle radius.
        /// </summary>
        public int InnerRadius
        {
            get
            {
                if (_innerRadius <= 0)
                    _innerRadius = 6;
                return _innerRadius;
            }
            set
            {
                _innerRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the outer circle radius.
        /// </summary>
        public int OuterRadius
        {
            get
            {
                if (_outerRadius <= 0)
                    _outerRadius = 7;
                return _outerRadius;
            }
            set
            {
                _outerRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the rotation speed.
        /// </summary>
        public int RotationSpeed
        {
            get => _timer.Interval;
            set
            {
                if (value > 0)
                    _timer.Interval = value;
            }
        }

        /// <summary>
        ///     Gets or sets the foreground color of the control.
        /// </summary>
        public override Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                GenerateColorsPallet();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressCircle"/> class.
        /// </summary>
        public ProgressCircle()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            _timer = new Timer();
            _timer.Tick += Timer_Tick;
            Disposed += OnDisposed;
            ActiveTimer();

            Resize += ProgressCircle_Resize;
        }

        /// <summary>
        ///     Sets the circle appearance.
        /// </summary>
        /// <param name="spokes">
        ///     The number of spokes.
        /// </param>
        /// <param name="thickness">
        ///     The spoke thickness.
        /// </param>
        /// <param name="innerRadius">
        ///     The inner circle radius.
        /// </param>
        /// <param name="outerRadius">
        ///     The outer circle radius.
        /// </param>
        public void SetAppearance(int spokes, int thickness, int innerRadius, int outerRadius)
        {
            Spokes = spokes;
            Thickness = thickness;
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
            Invalidate();
        }

        /// <summary>
        ///     Retrieves the size of a rectangular area into which a control can be
        ///     fitted.
        /// </summary>
        /// <param name="size">
        ///     The custom-sized area for a control.
        /// </param>
        public override Size GetPreferredSize(Size size) =>
            size with { Width = size.Width = (_outerRadius + _thickness) * 2 };

        /// <summary>
        ///     Raises the <see cref="Control"/>.Paint event.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_spokes < 1)
            {
                base.OnPaint(e);
                return;
            }
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pos = _progressValue;
            for (var i = 0; i < _spokes; i++)
            {
                var brush = default(SolidBrush);
                try
                {
                    brush = new SolidBrush(_colors[i]);
                    pos %= _spokes;
                    using (var pen = new Pen(brush, _thickness))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(pen, GetCoordinate(_centerPoint, _innerRadius, _angles[pos]), GetCoordinate(_centerPoint, _outerRadius, _angles[pos]));
                    }
                    pos++;
                }
                finally
                {
                    brush?.Dispose();
                }
            }
            base.OnPaint(e);
        }

        private static PointF GetCoordinate(PointF circleCenter, int radius, double angle)
        {
            var d = Math.PI * angle / 180d;
            return new PointF(circleCenter.X + radius * (float)Math.Cos(d), circleCenter.Y + radius * (float)Math.Sin(d));
        }

        private void ProgressCircle_Resize(object sender, EventArgs e) =>
            GetControlCenterPoint();

        private void Timer_Tick(object sender, EventArgs e)
        {
            _progressValue = ++_progressValue % _spokes;
            Invalidate();
        }

        private void OnDisposed(object s, EventArgs e)
        {
            if (Disposing)
                _timer?.Dispose();
        }

        private void GenerateColorsPallet()
        {
            var colors = new Color[_spokes];
            var increment = (byte)(byte.MaxValue / _spokes);
            byte percentageOfDarken = 0;
            for (var i = 0; i < _spokes; i++)
                if (Active)
                    if (i == 0 || i < _spokes - _spokes)
                        colors[i] = ForeColor;
                    else
                    {
                        percentageOfDarken += increment;
                        colors[i] = Color.FromArgb(percentageOfDarken, ForeColor.R, ForeColor.G, ForeColor.B);
                    }
                else
                    colors[i] = ForeColor;
            _colors = colors;
        }

        private void GetControlCenterPoint() =>
            _centerPoint = new PointF(Width / 2f, Height / 2f - 1);

        private void GetSpokesAngles()
        {
            var angles = new double[_spokes];
            var angle = 360d / _spokes;
            for (var i = 0; i < _spokes; i++)
                angles[i] = i == 0 ? angle : angles[i - 1] + angle;
            _angles = angles;
        }

        private void ActiveTimer()
        {
            if (_isActive)
                _timer?.Start();
            else
            {
                _timer?.Stop();
                _progressValue = 0;
            }
            GenerateColorsPallet();
            Invalidate();
        }
    }
}
