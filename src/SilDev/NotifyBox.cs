#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: NotifyBox.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Media;
    using System.Threading;
    using System.Windows.Forms;
    using Timer = System.Windows.Forms.Timer;

    /// <summary>
    ///     Displays a notification window, simliar with a system tray notification, which presents a
    ///     notification to the user.
    /// </summary>
    public class NotifyBox
    {
        /// <summary>
        ///     Provides enumerated constants used to retrieves the play sound of the notify box.
        /// </summary>
        public enum NotifyBoxSound
        {
            Asterisk,
            Warning,
            Notify,
            Question,
            None
        }

        /// <summary>
        ///     Provides enumerated constants used to retrieves the start position of the notify box.
        /// </summary>
        public enum NotifyBoxStartPosition
        {
            Center,
            CenterLeft,
            CenterRight,
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }

        private double _opacity = .95d;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotifyBox"/> class.
        /// </summary>
        [SuppressMessage("ReSharper", "EmptyConstructor")]
        public NotifyBox() { }

        /// <summary>
        ///     Gets or sets the opacity level for the notify box.
        /// </summary>
        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value < .2d ? .2d : value > 1d ? 1d : value; }
        }

        /// <summary>
        ///     Gets or sets the background color for the notify box.
        /// </summary>
        public Color BackColor { get; set; } = SystemColors.Menu;

        /// <summary>
        ///     Gets or sets the border color for the notify box.
        /// </summary>
        public Color BorderColor { get; set; } = SystemColors.MenuHighlight;

        /// <summary>
        ///     Gets or sets the caption color for the notify box.
        /// </summary>
        public Color CaptionColor { get; set; } = SystemColors.MenuHighlight;

        /// <summary>
        ///     Gets or sets the text color for the notify box.
        /// </summary>
        public Color TextColor { get; set; } = SystemColors.MenuText;

        private NotifyForm NotifyWindow { get; set; }
        private Thread NotifyThread { get; set; }

        /// <summary>
        ///     Gets a value indicating the execution status of the current notify box.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                try
                {
                    return NotifyThread.IsAlive;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Displays a notify box with the specified text, caption, position, sound, duration, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="position">
        ///     The window position for the notify box.
        /// </param>
        /// <param name="sound">
        ///     The play sound for the notify box.
        /// </param>
        /// <param name="duration">
        ///     The duration of the time, in milliseconds, which the notify box remains active.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxStartPosition position = NotifyBoxStartPosition.BottomRight, NotifyBoxSound sound = NotifyBoxSound.None, ushort duration = 0, bool borders = true)
        {
            try
            {
                if (IsAlive)
                    throw new NotSupportedException();
                NotifyWindow = new NotifyForm(text, caption, position, duration, borders, _opacity, BackColor, BorderColor, CaptionColor, TextColor);
                NotifyThread = new Thread(() => NotifyWindow.ShowDialog());
                NotifyThread.Start();
                switch (sound)
                {
                    case NotifyBoxSound.Asterisk:
                        SystemSounds.Asterisk.Play();
                        break;
                    case NotifyBoxSound.Warning:
                        SystemSounds.Hand.Play();
                        break;
                    case NotifyBoxSound.Question:
                        SystemSounds.Question.Play();
                        break;
                    case NotifyBoxSound.Notify:
                        var wavPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media\\Windows Notify System Generic.wav");
                        if (File.Exists(wavPath))
                            new SoundPlayer(wavPath).Play();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Displays a notify box with the specified text, caption, position, sound, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="position">
        ///     The window position for the notify box.
        /// </param>
        /// <param name="sound">
        ///     The play sound for the notify box.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxStartPosition position, NotifyBoxSound sound, bool borders) =>
            Show(text, caption, position, sound, 0, borders);

        /// <summary>
        ///     Displays a notify box with the specified text, caption, position, duration, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="position">
        ///     The window position for the notify box.
        /// </param>
        /// <param name="duration">
        ///     The duration of the time, in milliseconds, which the notify box remains active.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxStartPosition position, ushort duration, bool borders = true) =>
            Show(text, caption, position, NotifyBoxSound.None, duration, borders);

        /// <summary>
        ///     Displays a notify box with the specified text, caption, position, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="position">
        ///     The window position for the notify box.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxStartPosition position, bool borders) =>
            Show(text, caption, position, NotifyBoxSound.None, 0, borders);

        /// <summary>
        ///     Displays a notify box with the specified text, caption, sound, duration, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="sound">
        ///     The play sound for the notify box.
        /// </param>
        /// <param name="duration">
        ///     The duration of the time, in milliseconds, which the notify box remains active.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxSound sound, ushort duration = 0, bool borders = true) =>
            Show(text, caption, NotifyBoxStartPosition.BottomRight, sound, duration, borders);

        /// <summary>
        ///     Displays a notify box with the specified text, caption, sound, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="sound">
        ///     The play sound for the notify box.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, NotifyBoxSound sound, bool borders) =>
            Show(text, caption, NotifyBoxStartPosition.BottomRight, sound, 0, borders);

        /// <summary>
        ///     Displays a notify box with the specified text, caption, duration, and borders.
        /// </summary>
        /// <param name="text">
        ///     The notification text to display in the notify box.
        /// </param>
        /// <param name="caption">
        ///     The caption text to display in the notify box.
        /// </param>
        /// <param name="duration">
        ///     The duration of the time, in milliseconds, which the notify box remains active.
        /// </param>
        /// <param name="borders">
        ///     true to visible the window borders; otherwise, false.
        /// </param>
        public void Show(string text, string caption, ushort duration, bool borders = true) =>
            Show(text, caption, NotifyBoxStartPosition.BottomRight, NotifyBoxSound.None, duration, borders);

        /// <summary>
        ///     Closes the current notify box.
        /// </summary>
        public void Close()
        {
            try
            {
                NotifyWindow?.Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Terminates the current notify box.
        /// </summary>
        public void Abort()
        {
            Close();
            try
            {
                if (IsAlive)
                    NotifyThread.Abort();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private sealed class NotifyForm : Form
        {
            private readonly BackgroundWorker _asyncWait = new BackgroundWorker();
            private readonly IContainer _components = null;
            private readonly int _duration;
            private readonly Timer _loadingDots = new Timer();
            private readonly Label _textLabel;

            public NotifyForm(string text, string title, NotifyBoxStartPosition position, ushort duration, bool borders, double opacity, Color backColor, Color borderColor, Color captionColor, Color textColor)
            {
                SuspendLayout();
                _asyncWait.DoWork += AsyncWait_DoWork;
                _asyncWait.RunWorkerCompleted += AsyncWait_RunWorkerCompleted;
                _loadingDots.Interval = 256;
                _loadingDots.Tick += LoadingDots_Tick;
                var titleLabel = new Label
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 11.25f, FontStyle.Bold),
                    ForeColor = captionColor,
                    Location = new Point(3, 3),
                    Text = title
                };
                Controls.Add(titleLabel);
                _textLabel = new Label
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 8.25f, FontStyle.Regular),
                    ForeColor = textColor,
                    Location = new Point(8, 24),
                    Text = text
                };
                Controls.Add(_textLabel);
                if (borders)
                    for (var i = 0; i < 4; i++)
                        Controls.Add(new Panel
                        {
                            AutoSize = false,
                            BackColor = borderColor,
                            Dock = i == 0 ? DockStyle.Top : i == 1 ? DockStyle.Right : i == 2 ? DockStyle.Bottom : DockStyle.Left,
                            Location = new Point(0, 0),
                            Size = new Size(1, 1)
                        });
                AutoScaleDimensions = new SizeF(6f, 13f);
                AutoScaleMode = AutoScaleMode.Font;
                BackColor = backColor;
                ClientSize = new Size(48, 44);
                Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
                ForeColor = textColor;
                FormBorderStyle = FormBorderStyle.None;
                Opacity = opacity;
                ShowIcon = false;
                ShowInTaskbar = false;
                Size = new Size((titleLabel.Size.Width < _textLabel.Size.Width ? _textLabel.Size.Width : titleLabel.Size.Width) + 12, titleLabel.Size.Height + _textLabel.Size.Height + 12);
                StartPosition = position == NotifyBoxStartPosition.Center ? FormStartPosition.CenterScreen : FormStartPosition.Manual;
                if (StartPosition == FormStartPosition.Manual)
                {
                    var taskBarLocation = TaskBar.GetLocation();
                    var taskBarSize = TaskBar.GetSize();
                    switch (position)
                    {
                        case NotifyBoxStartPosition.CenterLeft:
                        case NotifyBoxStartPosition.TopLeft:
                        case NotifyBoxStartPosition.BottomLeft:
                            Location = new Point(taskBarLocation == TaskBar.Location.Left ? taskBarSize + 3 : 3, Location.Y);
                            break;
                        case NotifyBoxStartPosition.CenterRight:
                        case NotifyBoxStartPosition.TopRight:
                        case NotifyBoxStartPosition.BottomRight:
                            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - (taskBarLocation == TaskBar.Location.Right ? taskBarSize + 3 : 3), Location.Y);
                            break;
                    }
                    switch (position)
                    {
                        case NotifyBoxStartPosition.CenterLeft:
                        case NotifyBoxStartPosition.CenterRight:
                            Location = new Point(Location.X, Screen.PrimaryScreen.Bounds.Height / 2 - Height / 2);
                            break;
                        case NotifyBoxStartPosition.BottomLeft:
                        case NotifyBoxStartPosition.BottomRight:
                            Location = new Point(Location.X, Screen.PrimaryScreen.Bounds.Height - Height - (taskBarLocation == TaskBar.Location.Bottom ? taskBarSize + 3 : 3));
                            break;
                        case NotifyBoxStartPosition.TopLeft:
                        case NotifyBoxStartPosition.TopRight:
                            Location = new Point(Location.X, taskBarLocation == TaskBar.Location.Top ? taskBarSize + 3 : 3);
                            break;
                    }
                }
                Shown += NotifyForm_Shown;
                ResumeLayout(false);
                PerformLayout();
                _duration = duration;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    _components?.Dispose();
                base.Dispose(disposing);
            }

            private void NotifyForm_Shown(object sender, EventArgs e)
            {
                if (_textLabel.Text.EndsWith(" . . ."))
                    _loadingDots.Enabled = true;
                if (_duration >= 100)
                    _asyncWait.RunWorkerAsync();
            }

            private void AsyncWait_DoWork(object sender, DoWorkEventArgs e) =>
                Thread.Sleep(_duration);

            private void AsyncWait_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) =>
                Close();

            private void LoadingDots_Tick(object sender, EventArgs e)
            {
                var s = _textLabel.Text;
                if (!s.EndsWith(""))
                {
                    ((Timer)sender).Enabled = false;
                    return;
                }
                if (s.EndsWith(" . . ."))
                {
                    s = s.Replace(" . . .", " .");
                    while (s.EndsWith(" . ."))
                        s = s.Replace(" . .", " .");
                }
                else
                    s = s.EndsWith(" . .") ? s.Replace(" . .", " . . .") : s.Replace(" .", " . .");
                _textLabel.Text = s;
            }
        }
    }
}
