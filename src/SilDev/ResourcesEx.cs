#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ResourcesEx.cs
// Version:  2017-06-28 08:51
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Forms;

    /// <summary>
    ///     Provides static methods for the usage of data resources.
    /// </summary>
    public static class ResourcesEx
    {
        /// <summary>
        ///     Provides enumerated symbol index values of the Windows Image Resource dynamic
        ///     link library ('imageres.dll').
        /// </summary>
        public enum IconIndex : uint
        {
#pragma warning disable CS1591
            Asterisk = 0x4c,
            Barrier = 0x51,
            BmpFile = 0x42,
            Cam = 0x29,
            Cd = 0x38,
            CdR = 0x39,
            CdRom = 0x3a,
            CdRw = 0x3b,
            Chip = 0x1d,
            Clipboard = 0xf1,
            Close = 0xeb,
            CommandPrompt = 0x106,
            Computer = 0x68,
            Defrag = 0x6a,
            Desktop = 0x69,
            Directory = 0x3,
            DirectorySearch = 0xd,
            DiscDrive = 0x19,
            DllFile = 0x3e,
            Dvd = 0x33,
            DvdDrive = 0x20,
            DvdR = 0x21,
            DvdRam = 0x22,
            DvdRom = 0x23,
            DvdRw = 0x24,
            Eject = 0xa7,
            Error = 0x5d,
            ExeFile = 0xb,
            Explorer = 0xcb,
            Favorite = 0xcc,
            FloppyDrive = 0x17,
            Games = 0xa,
            HardDrive = 0x1e,
            Help = 0x5e,
            HelpShield = 0x63,
            InfFile = 0x40,
            Install = 0x52,
            JpgFile = 0x43,
            Key = 0x4d,
            Network = 0x14,
            OneDrive = 0xdc,
            Pin = 0xea,
            Play = 0x118,
            PngFile = 0x4e,
            Printer = 0x2e,
            Question = 0x5e,
            RecycleBinEmpty = 0x32,
            RecycleBinFull = 0x31,
            Retry = 0xfb,
            Run = 0x5f,
            Screensaver = 0x60,
            Search = 0xa8,
            Security = 0x36,
            SharedMarker = 0x9b,
            Sharing = 0x53,
            ShortcutMarker = 0x9a,
            Stop = 0xcf,
            SystemControl = 0x16,
            SystemDrive = 0x1f,
            TaskManager = 0x90,
            Uac = 0x49,
            Undo = 0xff,
            UnknownDrive = 0x46,
            Unpin = 0xe9,
            User = 0xd0,
            UserDir = 0x75,
            Warning = 0x4f,
            ZipFile = 0xa5
#pragma warning restore CS1591
        }

        /// <summary>
        ///     Returns the specified <see cref="Icon"/> resource of a file.
        /// </summary>
        /// <param name="path">
        ///     The file to read.
        /// </param>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="large">
        ///     true to return the large image; otherwise, false.
        /// </param>
        public static Icon GetIconFromFile(string path, int index = 0, bool large = false)
        {
            try
            {
                path = PathEx.Combine(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var ptrs = new IntPtr[1];
                var file = PathEx.Combine(path);
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                WinApi.NativeMethods.ExtractIconEx(file, index, large ? ptrs : new IntPtr[1], !large ? ptrs : new IntPtr[1], 1);
                var ptr = ptrs[0];
                if (ptr == IntPtr.Zero)
                    throw new ArgumentNullException(nameof(ptr));
                var ico = (Icon)Icon.FromHandle(ptr).Clone();
                WinApi.NativeMethods.DestroyIcon(ptr);
                return ico;
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Returns the specified <see cref="Icon"/> resource from the system file
        ///     "imageres.dll".
        /// </summary>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="large">
        ///     true to return the large image; otherwise, false.
        /// </param>
        /// <param name="location">
        ///     The directory where the "imageres.dll" file is located.
        /// </param>
        public static Icon GetSystemIcon(IconIndex index, bool large = false, string location = "%system%")
        {
            try
            {
                var path = PathEx.Combine(location);
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                if (Data.IsDir(path))
                    path = Path.Combine(path, "imageres.dll");
                if (!File.Exists(path))
                    path = PathEx.Combine("%system%\\imageres.dll");
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var ico = GetIconFromFile(path, (int)index, large);
                return ico;
            }
            catch (PathNotFoundException ex)
            {
                Log.Write(ex);
            }
            catch
            {
                // ignored
            }
            return null;
        }

        /// <summary>
        ///     Returns the specified <see cref="Icon"/> resource from the system file
        ///     "imageres.dll".
        /// </summary>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="location">
        ///     The directory where the "imageres.dll" file is located.
        /// </param>
        public static Icon GetSystemIcon(IconIndex index, string location) =>
            GetSystemIcon(index, false, location);

        /// <summary>
        ///     Returns an file type icon of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to get the file type icon.
        /// </param>
        /// <param name="large">
        ///     true to return the large image; otherwise, false.
        /// </param>
        public static Icon GetFileTypeIcon(string path, bool large = false)
        {
            try
            {
                path = PathEx.Combine(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var shfi = new WinApi.ShFileInfo();
                var flags = WinApi.FileInfoFlags.Icon | WinApi.FileInfoFlags.UseFileAttributes;
                flags |= large ? WinApi.FileInfoFlags.LargeIcon : WinApi.FileInfoFlags.SmallIcon;
                WinApi.NativeMethods.SHGetFileInfo(path, 0x80, ref shfi, (uint)Marshal.SizeOf(shfi), flags);
                var ico = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
                WinApi.NativeMethods.DestroyIcon(shfi.hIcon);
                return ico;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Extracts the specified resources from the current process to a new file.
        /// </summary>
        /// <param name="resData">
        ///     The resource to extract.
        /// </param>
        /// <param name="destPath">
        ///     The file to create.
        /// </param>
        /// <param name="reverseBytes">
        ///     true to invert the order of the bytes in the specified sequence before extracting;
        ///     otherwise, false.
        /// </param>
        public static void Extract(byte[] resData, string destPath, bool reverseBytes = false)
        {
            try
            {
                var path = PathEx.Combine(destPath);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentNullException(nameof(dir));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                using (var ms = new MemoryStream(resData))
                {
                    var data = ms.ToArray();
                    if (reverseBytes)
                        data = data.Reverse().ToArray();
                    using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                        fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Displays a dialog box that prompts to the user to browse the icon resource of a file.
        ///     <see cref="OpenFileDialog"/>
        /// </summary>
        public sealed class IconBrowserDialog : Form
        {
            private static readonly object Locker = new object();
            private readonly List<IconBox> _boxes = new List<IconBox>();
            private readonly Button _button;
            private readonly Panel _buttonPanel;
            private readonly IContainer _components;
            private readonly Panel _panel;
            private readonly ProgressCircle _progressCircle;
            private readonly TextBox _textBox;
            private readonly Timer _timer;
            private string _path;

            /// <summary>
            ///     Initializes an instance of the <see cref="IconBrowserDialog"/> class.
            /// </summary>
            /// <param name="path">
            ///     The path of the file to open.
            /// </param>
            /// <param name="backColor">
            ///     The background color of the dialog box.
            /// </param>
            /// <param name="foreColor">
            ///     The foreground color of the dialog box.
            /// </param>
            /// <param name="buttonFace">
            ///     The button color of the dialog box.
            /// </param>
            /// <param name="buttonText">
            ///     The button text color of the dialog box.
            /// </param>
            /// <param name="buttonHighlight">
            ///     The button highlight color of the dialog box.
            /// </param>
            public IconBrowserDialog(string path = "%system%\\imageres.dll", Color? backColor = null, Color? foreColor = null, Color? buttonFace = null, Color? buttonText = null, Color? buttonHighlight = null)
            {
                _components = new Container();
                SuspendLayout();
                var resPath = PathEx.Combine(path);
                if (Data.IsDir(resPath))
                    resPath = PathEx.Combine(path, "imageres.dll");
                if (!File.Exists(resPath))
                    resPath = PathEx.Combine("%system%", "imageres.dll");
                var resLoc = Path.GetDirectoryName(resPath);
                BackColor = backColor ?? SystemColors.Control;
                ForeColor = foreColor ?? SystemColors.ControlText;
                Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Icon = GetSystemIcon(IconIndex.DirectorySearch, true, resLoc);
                MaximizeBox = false;
                MaximumSize = new Size(680, Screen.FromHandle(Handle).WorkingArea.Height);
                MinimizeBox = false;
                MinimumSize = new Size(680, 448);
                Name = "IconBrowserForm";
                Size = MinimumSize;
                SizeGripStyle = SizeGripStyle.Hide;
                StartPosition = FormStartPosition.CenterScreen;
                Text = @"Icon Resource Browser";
                var tableLayoutPanel = new TableLayoutPanel
                {
                    BackColor = Color.Transparent,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    Dock = DockStyle.Fill,
                    Name = "tableLayoutPanel",
                    RowCount = 2
                };
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
                Controls.Add(tableLayoutPanel);
                _panel = new Panel
                {
                    AutoScroll = true,
                    BackColor = buttonFace ?? SystemColors.ButtonFace,
                    BorderStyle = BorderStyle.FixedSingle,
                    Enabled = false,
                    ForeColor = buttonText ?? SystemColors.ControlText,
                    Dock = DockStyle.Fill,
                    Name = "panel",
                    TabIndex = 0
                };
                _panel.Scroll += (s, e) => (s as Panel)?.Update();
                tableLayoutPanel.Controls.Add(_panel, 0, 0);
                var innerTableLayoutPanel = new TableLayoutPanel
                {
                    BackColor = Color.Transparent,
                    ColumnCount = 2,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    Dock = DockStyle.Fill,
                    Name = "innerTableLayoutPanel"
                };
                innerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                innerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24));
                tableLayoutPanel.Controls.Add(innerTableLayoutPanel, 0, 1);
                _textBox = new TextBox
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = DockStyle.Top,
                    Font = Font,
                    Name = "textBox",
                    TabIndex = 1
                };
                _textBox.TextChanged += TextBox_TextChanged;
                innerTableLayoutPanel.Controls.Add(_textBox, 0, 0);
                _buttonPanel = new Panel
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.Transparent,
                    BorderStyle = BorderStyle.FixedSingle,
                    Name = "buttonPanel",
                    Size = new Size(20, 20)
                };
                innerTableLayoutPanel.Controls.Add(_buttonPanel, 1, 0);
                _button = new Button
                {
                    BackColor = buttonFace ?? SystemColors.ButtonFace,
                    BackgroundImage = GetSystemIcon(IconIndex.Directory, false, resLoc).ToBitmap(),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    Font = Font,
                    ForeColor = buttonText ?? SystemColors.ControlText,
                    Name = "button",
                    TabIndex = 2,
                    UseVisualStyleBackColor = false
                };
                _button.FlatAppearance.BorderSize = 0;
                _button.FlatAppearance.MouseOverBackColor = buttonHighlight ?? ProfessionalColors.ButtonSelectedHighlight;
                _button.Click += Button_Click;
                _buttonPanel.Controls.Add(_button);
                _progressCircle = new ProgressCircle
                {
                    Active = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    ForeColor = (backColor ?? SystemColors.Control).InvertRgb().ToGrayScale(),
                    RotationSpeed = 80,
                    Thickness = 2,
                    Visible = true
                };
                _timer = new Timer(_components)
                {
                    Interval = 1
                };
                _timer.Tick += Timer_Tick;
                Shown += (sender, args) => TaskBar.Progress.SetState(Handle, TaskBar.Progress.Flags.Indeterminate);
                ResumeLayout(false);
                PerformLayout();
                var curPath = PathEx.Combine(path);
                if (!File.Exists(curPath))
                    curPath = resPath;
                if (!File.Exists(curPath))
                    return;
                _textBox.Text = curPath;
            }

            /// <summary>
            ///     Disposes of the resources (other than memory) used by the <see cref="Form"/>.
            /// </summary>
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    _components.Dispose();
                base.Dispose(disposing);
            }

            private void TextBox_TextChanged(object sender, EventArgs e)
            {
                var textBox = sender as TextBox;
                if (textBox == null)
                    return;
                var path = PathEx.Combine(textBox.Text);
                if (string.IsNullOrWhiteSpace(path) || _path == path || !File.Exists(path) || GetIconFromFile(path, 0, true) == null)
                    return;
                TaskBar.Progress.SetState(Handle, TaskBar.Progress.Flags.Indeterminate);
                _path = path;
                _panel.Enabled = false;
                _textBox.Enabled = false;
                _buttonPanel.SuspendLayout();
                _buttonPanel.BorderStyle = BorderStyle.None;
                _buttonPanel.Controls.Clear();
                _buttonPanel.Controls.Add(_progressCircle);
                _buttonPanel.ResumeLayout(false);
                _progressCircle.Active = true;
                _timer.Enabled = true;
            }

            private void Button_Click(object sender, EventArgs e)
            {
                using (var dialog = new OpenFileDialog
                {
                    InitialDirectory = PathEx.LocalDir,
                    Multiselect = false,
                    RestoreDirectory = false
                })
                {
                    dialog.ShowDialog(new Form
                    {
                        ShowIcon = false,
                        TopMost = true
                    });
                    if (File.Exists(dialog.FileName))
                        _textBox.Text = dialog.FileName;
                }
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                lock (Locker)
                {
                    var timer = sender as Timer;
                    if (timer == null)
                        return;
                    if (_boxes.Count == 0 && _panel.Controls.Count > 0)
                        _panel.Controls.Clear();
                    const int speed = 3; // Higher value loads the icons faster, but hangs more the UI while loading
                    for (var i = 0; i < speed; i++)
                        try
                        {
                            _boxes.Add(new IconBox(_path, _boxes.Count, _button.BackColor, _button.ForeColor, _button.FlatAppearance.MouseOverBackColor));
                            if (_boxes.Count <= 0)
                                continue;
                            var last = _boxes.Last();
                            if (_panel.Controls.Contains(last))
                                continue;
                            _panel.SuspendLayout();
                            _panel.Controls.Add(last);
                            _panel.ResumeLayout(false);
                        }
                        catch
                        {
                            timer.Enabled = false;
                            if (_boxes.Count > 0)
                                _boxes.Clear();
                            break;
                        }
                    var max = _panel.Width / _panel.Controls[0].Width;
                    for (var i = 0; i < _panel.Controls.Count; i++)
                    {
                        if (_panel.Controls[i] == null)
                            continue;
                        var line = i / max;
                        var column = i - line * max;
                        _panel.Controls[i].Location = new Point(column * _panel.Controls[i].Width, line * _panel.Controls[i].Height);
                    }
                    if (timer.Enabled || _panel.Enabled)
                        return;
                    _panel.Enabled = true;
                    _textBox.Enabled = true;
                    _progressCircle.Active = false;
                    _buttonPanel.SuspendLayout();
                    _buttonPanel.Controls.Clear();
                    _buttonPanel.Controls.Add(_button);
                    _buttonPanel.BorderStyle = BorderStyle.FixedSingle;
                    _buttonPanel.ResumeLayout(false);
                    TaskBar.Progress.SetState(Handle, TaskBar.Progress.Flags.NoProgress);
                    if (!_panel.Focus())
                        _panel.Select();
                }
            }

            private sealed class IconBox : UserControl
            {
                private static string _file;
                private static IntPtr[] _icons;
                private readonly Button _button;
                private readonly IContainer _components = null;

                public IconBox(string path, int index, Color? buttonFace = null, Color? buttonText = null, Color? buttonHighlight = null)
                {
                    SuspendLayout();
                    BackColor = buttonFace ?? SystemColors.ButtonFace;
                    ForeColor = buttonText ?? SystemColors.ControlText;
                    Name = "IconBox";
                    Size = new Size(58, 62);
                    _button = new Button
                    {
                        BackColor = BackColor,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = ForeColor,
                        ImageAlign = ContentAlignment.TopCenter,
                        Location = new Point(3, 3),
                        Name = "button",
                        Size = new Size(52, 56),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.BottomCenter,
                        UseVisualStyleBackColor = false
                    };
                    _button.FlatAppearance.BorderSize = 0;
                    _button.FlatAppearance.MouseOverBackColor = buttonHighlight ?? ProfessionalColors.ButtonSelectedHighlight;
                    _button.Click += Button_Click;
                    Controls.Add(_button);
                    ResumeLayout(false);
                    if (_file != null && _file != path)
                        _icons = null;
                    _file = path;
                    var myIcon = GetIcons(index);
                    _button.Image = new Bitmap(myIcon.ToBitmap(), myIcon.Width, myIcon.Height);
                    _button.Text = index.ToString();
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                        _components?.Dispose();
                    base.Dispose(disposing);
                }

                private static Icon GetIcons(int index)
                {
                    if (_icons != null)
                        return index > _icons.Length - 1 ? null : Icon.FromHandle(_icons[index]);
                    _icons = new IntPtr[short.MaxValue];
                    WinApi.NativeMethods.ExtractIconEx(_file, 0, _icons, new IntPtr[short.MaxValue], short.MaxValue);
                    return index > _icons.Length - 1 ? null : Icon.FromHandle(_icons[index]);
                }

                private void Button_Click(object sender, EventArgs e)
                {
                    if (ParentForm == null)
                        return;
                    ParentForm.Text = File.Exists(_file) ? $"{_file},{_button.Text}" : string.Empty;
                    ParentForm.Close();
                }
            }
        }
    }
}
