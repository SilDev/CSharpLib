#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IconBrowserDialog.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using Drawing;
    using Properties;
    using Timer = System.Windows.Forms.Timer;

    /// <summary>
    ///     Displays a dialog box that prompts to the user to browse the icon resource
    ///     of a file.
    /// </summary>
    public sealed class IconBrowserDialog : Form
    {
        private static object _syncObject;
        private static string _filePath;
        private static IntPtr[] _iconPointers;
        private readonly Button _button;
        private readonly Panel _buttonPanel;
        private readonly IContainer _components;
        private readonly TableLayoutPanel _innerTableLayoutPanel;
        private readonly Panel _panel;
        private readonly ProgressCircle _progressCircle;
        private readonly TableLayoutPanel _tableLayoutPanel;
        private readonly TextBox _textBox;
        private readonly Timer _timer;
        private int _count;
        private bool _isResizing;
        private string _path;

        /// <summary>
        ///     Gets the icon resource path.
        /// </summary>
        public string IconPath { get; private set; }

        /// <summary>
        ///     Gets the icon resource identifier.
        /// </summary>
        public int IconId { get; private set; }

        private static object SyncObject
        {
            get
            {
                if (_syncObject != null)
                    return _syncObject;
                var obj = new object();
                Interlocked.CompareExchange<object>(ref _syncObject, obj, null);
                return _syncObject;
            }
        }

        private static string FilePath
        {
            get => _filePath;
            set
            {
                lock (SyncObject)
                    _filePath = value;
            }
        }

        private static IntPtr[] IconPointers
        {
            get => _iconPointers;
            set
            {
                lock (SyncObject)
                {
                    if (value == null)
                        _iconPointers?.Where(x => x != IntPtr.Zero).ForEach(x => WinApi.NativeMethods.DestroyIcon(x));
                    _iconPointers = value;
                }
            }
        }

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
            if (PathEx.IsDir(resPath))
                resPath = PathEx.Combine(path, "imageres.dll");
            if (!File.Exists(resPath))
                resPath = PathEx.Combine("%system%", "imageres.dll");
            var resLoc = Path.GetDirectoryName(resPath);
            AutoScaleDimensions = new SizeF(96f, 96f);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = backColor ?? SystemColors.Control;
            ForeColor = foreColor ?? SystemColors.ControlText;
            Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = ResourcesEx.GetSystemIcon(ImageResourceSymbol.OpenSearch, true, resLoc);
            MaximizeBox = false;
            MaximumSize = new Size(680, Screen.FromHandle(Handle).WorkingArea.Height);
            MinimizeBox = false;
            MinimumSize = new Size(680, 448);
            Name = "IconBrowserForm";
            Size = MinimumSize;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = UIStrings.ResourceBrowser;
            _tableLayoutPanel = new TableLayoutPanel
            {
                BackColor = Color.Transparent,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Dock = DockStyle.Fill,
                Name = "tableLayoutPanel",
                RowCount = 2
            };
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            Controls.Add(_tableLayoutPanel);
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
            _panel.Scroll += (s, _) => (s as Panel)?.Update();
            _tableLayoutPanel.Controls.Add(_panel, 0, 0);
            _innerTableLayoutPanel = new TableLayoutPanel
            {
                BackColor = Color.Transparent,
                ColumnCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Dock = DockStyle.Fill,
                Name = "innerTableLayoutPanel"
            };
            _innerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            _innerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.Controls.Add(_innerTableLayoutPanel, 0, 1);
            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Font = Font,
                Name = "textBox",
                TabIndex = 1
            };
            _textBox.TextChanged += TextBox_TextChanged;
            _innerTableLayoutPanel.Controls.Add(_textBox, 0, 0);
            _buttonPanel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle,
                Name = "buttonPanel",
                Size = new Size(20, 20)
            };
            _innerTableLayoutPanel.Controls.Add(_buttonPanel, 1, 0);
            _button = new Button
            {
                BackColor = buttonFace ?? SystemColors.ButtonFace,
                BackgroundImage = ResourcesEx.GetSystemIcon(ImageResourceSymbol.Directory, false, resLoc).ToBitmap(),
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
            ResizeBegin += (_, _) => _isResizing = true;
            ResizeEnd += (_, _) => _isResizing = false;
            Shown += (_, _) => TaskBarProgress.SetState(Handle, TaskBarProgressState.Indeterminate);
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
        ///     Disposes of the resources (other than memory) used by the
        ///     <see cref="Form"/>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _components.Dispose();
            if (_panel.Controls.Count > 0)
            {
                for (var i = 0; i < _panel.Controls.Count; i++)
                    _panel.Controls[i]?.Dispose();
                IconPointers = null;
            }
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Dispose();
            }
            if (_progressCircle != null)
            {
                _progressCircle.Enabled = false;
                _progressCircle.Dispose();
            }
            _button?.Dispose();
            _buttonPanel?.Dispose();
            _textBox?.Dispose();
            _innerTableLayoutPanel?.Dispose();
            _panel?.Dispose();
            _tableLayoutPanel?.Dispose();
            base.Dispose(disposing);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is not TextBox textBox)
                return;
            var path = PathEx.Combine(textBox.Text);
            if (string.IsNullOrWhiteSpace(path) || _path == path || !File.Exists(path) || ResourcesEx.GetIconFromFile(path, 0, true) == null)
                return;
            TaskBarProgress.SetState(Handle, TaskBarProgressState.Indeterminate);
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
            using var dialog = new OpenFileDialog();
            dialog.InitialDirectory = PathEx.LocalDir;
            dialog.Multiselect = false;
            dialog.RestoreDirectory = false;
            using (var owner = new Form())
            {
                owner.ShowIcon = false;
                owner.TopMost = true;
                dialog.ShowDialog(owner);
            }
            if (File.Exists(dialog.FileName))
                _textBox.Text = dialog.FileName;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isResizing || sender is not Timer timer)
                return;
            lock (SyncObject)
            {
                if (_count == 0 && _panel.Controls.Count > 0)
                    _panel.Controls.Clear();
                for (var i = 0; i < 4; i++)
                {
                    if (_isResizing)
                        return;
                    _panel.SuspendLayout();
                    try
                    {
                        _panel.Controls.Add(new IconBox(_path, _count++, _button.BackColor, _button.ForeColor, _button.FlatAppearance.MouseOverBackColor));
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        Log.Write(ex);
                        _count = 0;
                    }
                    finally
                    {
                        _panel.ResumeLayout(false);
                    }
                    if (_count > 0 && _count < (IconPointers?.Length ?? 0))
                        continue;
                    _count = 0;
                    timer.Enabled = false;
                    break;
                }
                if (_panel.Controls.Count > 0)
                {
                    var max = _panel.Width / _panel.Controls[0].Width;
                    for (var i = 0; i < _panel.Controls.Count; i++)
                    {
                        if (_panel.Controls[i] == null)
                            continue;
                        var line = i / max;
                        var column = i - line * max;
                        _panel.Controls[i].Location = new Point(column * _panel.Controls[i].Width, line * _panel.Controls[i].Height);
                    }
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
                TaskBarProgress.SetState(Handle, TaskBarProgressState.NoProgress);
                if (!_panel.Focus())
                    _panel.Select();
            }
        }

        private sealed class IconBox : UserControl
        {
            private readonly Button _button;
            private readonly IContainer _components = null;

            public IconBox(string path, int index, Color? buttonFace = null, Color? buttonText = null, Color? buttonHighlight = null)
            {
                SuspendLayout();
                AutoScaleDimensions = new SizeF(96f, 96f);
                AutoScaleMode = AutoScaleMode.Dpi;
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
                if (!FilePath?.EqualsEx(path) ?? false)
                    IconPointers = null;
                FilePath = path;
                var icon = GetIcons(index) ?? throw new ArgumentOutOfRangeException(nameof(index));
                _button.Image = new Bitmap(icon.ToBitmap(), icon.Width, icon.Height);
                _button.Text = index.ToStringDefault();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    _components?.Dispose();
                _button?.Dispose();
                base.Dispose(disposing);
            }

            private static Icon GetIcons(int index)
            {
                if (IconPointers != null)
                    return index >= IconPointers.Length ? null : Icon.FromHandle(IconPointers[index]);
                var count = WinApi.NativeMethods.ExtractIconEx(FilePath, -1, null, null, 0);
                if (count < 1)
                {
                    IconPointers = Array.Empty<IntPtr>();
                    return null;
                }
                IconPointers = new IntPtr[count];
                count = WinApi.NativeMethods.ExtractIconEx(FilePath, 0, IconPointers, null, count);
                return index >= count ? null : Icon.FromHandle(IconPointers[index]);
            }

            private void Button_Click(object sender, EventArgs e)
            {
                if (ParentForm is not IconBrowserDialog dialog)
                    return;
                if (int.TryParse(_button.Text, out var index))
                {
                    var path = EnvironmentEx.GetVariableWithPath(FilePath, false, false);
                    dialog.IconPath = path;
                    if (path.Any(char.IsSeparator))
                        path = $"\"{path}\"";
                    dialog.IconId = index;
                    dialog.Text = $@"{path},{index}";
                }
                dialog.Close();
            }
        }
    }
}
