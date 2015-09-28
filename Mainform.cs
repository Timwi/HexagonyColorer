using System;
using RT.Util.ExtensionMethods;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RT.Util;
using RT.Util.Dialogs;
using RT.Util.Drawing;
using RT.Util.Forms;
using RT.Util.Json;
using RT.Util.Serialization;

namespace HexagonyColorer
{
    partial class Mainform : ManagedForm
    {
        public Mainform(HCSettings settings)
            : base(settings.FormSettings)
        {
            _settings = settings;
            InitializeComponent();
        }

        private HCSettings _settings;
        private HCFile _file = new HCFile();
        private DateTime _lastFileTime;
        private Bitmap _lastRendering;
        private PointAxial _lastMouseDown;

        private string _currentFilePathBacking;
        private string _currentFilePath
        {
            get { return _currentFilePathBacking; }
            set
            {
                _currentFilePathBacking = value;
                updateUi();
            }
        }

        private bool _anyChangesBacking;
        private bool _anyChanges
        {
            get { return _anyChangesBacking; }
            set
            {
                _anyChangesBacking = value;
                updateUi();
            }
        }

        private bool canDestroy()
        {
            if (!_anyChanges)
                return true;

            var result = DlgMessage.Show("Would you like to save your changes to this file?", "Hexagony Colorer", DlgType.Question, "&Save", "&Discard", "&Cancel");
            if (result == 2)
                return false;
            if (result == 1)
                return true;

            return save() == DialogResult.OK;
        }

        private void open(object _, EventArgs __)
        {
            if (!canDestroy())
                return;

            using (var open = new OpenFileDialog { Title = "Open file", DefaultExt = "hxgc", Filter = "Hexagony Colorations (*.hxgc)|*.hxgc" })
            {
                if (HCProgram.Settings.LastDirectory != null)
                    try { open.InitialDirectory = HCProgram.Settings.LastDirectory; }
                    catch { }
                if (open.ShowDialog() == DialogResult.Cancel)
                    return;
                HCProgram.Settings.LastDirectory = Path.GetDirectoryName(open.FileName);

                openCore(open.FileName);
            }
        }

        private void openCore(string filePath)
        {
            if (!File.Exists(filePath))
            {
                DlgMessage.Show("The specified file does not exist.", "Error", DlgType.Error);
                return;
            }
            _currentFilePath = filePath;
            _file = ClassifyJson.Deserialize<HCFile>(JsonValue.Parse(File.ReadAllText(_currentFilePath)));
            _anyChanges = false;
            _lastFileTime = File.GetLastWriteTimeUtc(_currentFilePath);
            updateList();
            rerender();
        }

        private void updateList()
        {
            var prevSelection = lstPaths.SelectedItem;
            lstPaths.Items.Clear();
            lstPaths.Items.AddRange(_file.Paths.Cast<object>().ToArray());
            _ignoreOneListChange = true;
            lstPaths.SelectedItem = prevSelection;
            _ignoreOneListChange = false;
        }

        private void save(object _, EventArgs __)
        {
            save();
        }

        private void saveAs(object _, EventArgs __)
        {
            saveAs();
        }

        private DialogResult save()
        {
            if (_currentFilePath == null)
                return saveAs();
            saveCore();
            return DialogResult.OK;
        }

        private DialogResult saveAs()
        {
            using (var save = new SaveFileDialog { Title = "Save file", DefaultExt = "hxgc", Filter = "Hexagony Colorations (*.hxgc)|*.hxgc" })
            {
                var result = save.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _currentFilePath = save.FileName;
                    saveCore();
                }
                return result;
            }
        }

        private void saveCore()
        {
            File.WriteAllText(_currentFilePath, ClassifyJson.Serialize(_file).ToStringIndented());
            _lastFileTime = File.GetLastWriteTimeUtc(_currentFilePath);
            _anyChanges = false;
        }

        private void revert(object sender, EventArgs e)
        {
            if (_currentFilePath != null && DlgMessage.Show("Revert all changes made since last save?", "Revert", DlgType.Question, "&Revert", "&Cancel") == 0)
                openCore(_currentFilePath);
        }

        private void updateUi()
        {
            // Construct the window titlebar
            var text = _currentFilePath == null ? "(unnamed)" : _currentFilePath;
            text += " — Hexagony Colorer";
            if (_anyChanges)
                text += " •";
            Text = text;
        }

        private void newFile(object sender, EventArgs e)
        {
            if (!canDestroy())
                return;

            using (var open = new OpenFileDialog { Title = "Open file", DefaultExt = "hxg", Filter = "Hexagony source (*.hxg)|*.hxg" })
            {
                if (HCProgram.Settings.LastDirectory != null)
                    try { open.InitialDirectory = HCProgram.Settings.LastDirectory; }
                    catch { }
                if (open.ShowDialog() == DialogResult.Cancel)
                    return;
                HCProgram.Settings.LastDirectory = Path.GetDirectoryName(open.FileName);

                _currentFilePath = null;
                _anyChanges = false;
                _file = new HCFile
                {
                    HexagonySource = Regex.Replace(File.ReadAllText(open.FileName).Replace("`", ""), @"\s+", "")
                };
                rerender();
            }
        }

        private void rerender()
        {
            var getX = Ut.Lambda((PointAxial p) => (2 * (p.Q + _file.Grid.Size - 1) + p.R) * _file.XTextSpacing + _file.XPadding);
            var getY = Ut.Lambda((PointAxial p) => (p.R + _file.Grid.Size - 1) * _file.YTextSpacing + _file.YPadding);
            var getEndAngle = Ut.Lambda((Direction dir) =>
                dir is NorthEast ? 300 : dir is East ? 0 : dir is SouthEast ? 60 :
                dir is SouthWest ? 120 : dir is West ? 180 : dir is NorthWest ? 240 : Ut.Throw<int>(new InvalidOperationException()));
            var getStartAngle = Ut.Lambda((Direction dir) => (getEndAngle(dir) + 180) % 360);
            var html = new List<string>();

            _lastRendering = GraphicsUtil.DrawBitmap(2 * (2 * (_file.Grid.Size - 1) * _file.XTextSpacing + _file.XPadding), 2 * ((_file.Grid.Size - 1) * _file.YTextSpacing + _file.YPadding), g =>
            {
                g.Clear(Color.White);

                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                var startCapPath = new GraphicsPath();
                startCapPath.AddPolygon(new[] { new PointF(-1, .2f), new PointF(-1, -.5f), new PointF(1, -.5f), new PointF(1, .2f), new PointF(0, -.5f) });
                var endCapPath = new GraphicsPath();
                endCapPath.AddPolygon(new[] { new PointF(-1, .5f), new PointF(1, .5f), new PointF(0, 1.2f) });

                foreach (var path in _file.Paths)
                {
                    if (path == _blinkingPath)
                        continue;
                    var ip = path.IpStartPos;
                    var ipDir = path.IpStartDirection;
                    var numInstr = path.NumInstructions;
                    var instructionsExecuted = "";
                    bool drawArrowStart = path.DrawStart;
                    var visited = new HashSet<Tuple<PointAxial, Direction>>();

                    while (numInstr == null || (numInstr = numInstr.Value - 1).Value >= 0)
                    {
                        // Infinite loop detection
                        if (!visited.Add(Tuple.Create(ip, ipDir)))
                            break;

                        var x = getX(ip);
                        var y = getY(ip);
                        var p = new PointF(x, y);

                        var arrowPath = new GraphicsPath();
                        var arrowPen = new Pen(new SolidBrush(Color.FromArgb(64, path.Color)), 17.5f) { LineJoin = LineJoin.Bevel };

                        if (drawArrowStart)
                        {
                            arrowPath.AddLine(new PointF((float) (x + _file.ArrowLength * Math.Cos(getStartAngle(ipDir) * Math.PI / 180)), (float) (y + _file.ArrowLength * Math.Sin(getStartAngle(ipDir) * Math.PI / 180))), p);
                            arrowPen.CustomStartCap = new CustomLineCap(null, startCapPath) { WidthScale = 1f / 3f };
                        }
                        drawArrowStart = true;

                        bool finish = false;
                        switch (_file.Grid[ip])
                        {
                            case '_': ipDir = ipDir.ReflectAtUnderscore; break;
                            case '|': ipDir = ipDir.ReflectAtPipe; break;
                            case '/': ipDir = ipDir.ReflectAtSlash; break;
                            case '\\': ipDir = ipDir.ReflectAtBackslash; break;
                            case '<':
                                if (ipDir is East && path.TakeBranch == null)
                                    finish = true;
                                else
                                    ipDir = ipDir.ReflectAtLessThan(path.TakeBranch ?? false);
                                break;
                            case '>':
                                if (ipDir is West && path.TakeBranch == null)
                                    finish = true;
                                else
                                    ipDir = ipDir.ReflectAtGreaterThan(path.TakeBranch ?? false);
                                break;

                            case '[':
                            case ']':
                            case '@':
                            case '#':
                                instructionsExecuted += _file.Grid[ip];
                                if (path.StopAtIpChanger || _file.Grid[ip] == '@')
                                    finish = true;
                                break;

                            case '.':
                            case '$':
                                break;

                            default:
                                instructionsExecuted += _file.Grid[ip];
                                break;
                        }

                        if (!finish && (numInstr == null || numInstr.Value > 0) || path.DrawEnd)
                        {
                            arrowPath.AddLine(p, new PointF((float) (x + _file.ArrowLength * Math.Cos(getEndAngle(ipDir) * Math.PI / 180)), (float) (y + _file.ArrowLength * Math.Sin(getEndAngle(ipDir) * Math.PI / 180))));
                            arrowPen.CustomEndCap = new CustomLineCap(null, endCapPath) { WidthScale = .2f };
                        }
                        g.DrawPath(arrowPen, arrowPath);

                        if (finish)
                            break;

                        for (int i = _file.Grid[ip] == '$' ? 0 : 1; i < 2; i++)
                        {
                            var newIp = handleEdges(ip + ipDir.Vector, ipDir, path.TakeBranch);
                            if (newIp == null)
                            {
                                finish = true;
                                break;
                            }
                            ip = newIp.Value;
                        }

                        if (finish)
                            break;
                    }
                    while (Regex.IsMatch(instructionsExecuted, @"\{""|""\{|\}'|'\}|==|\(\)|\)\("))
                        instructionsExecuted = instructionsExecuted.Replace("{\"", "").Replace("\"{", "").Replace("}'", "").Replace("'}", "").Replace("==", "").Replace("()", "").Replace(")(", "");
                    html.Add(@"<li style='background: rgba({5}, {6}, {7}, .25)'><div>{8}</div><div>Start: {1}, {2}</div><pre>{0}</pre><div>End: {3}, {4}</div></li>".Fmt(
                        /* 0 */ instructionsExecuted.HtmlEscape(),
                        /* 1, 2 */ path.IpStartPos, path.IpStartDirection,
                        /* 3, 4 */ ip, ipDir,
                        /* 5, 6, 7 */ path.Color.R, path.Color.G, path.Color.B,
                        /* 8 */ path.Name));
                }

                using (var font = new Font("DejaVu Sans Mono", 15f))
                    foreach (var instr in _file.Grid.Everything)
                        g.DrawString(instr.Item2.ToString(), font, Brushes.Black, getX(instr.Item1), getY(instr.Item1), sf);
            });

            ctImage.Size = _lastRendering.Size;
            ctImage.Refresh();

            //File.WriteAllText(imgInf.OutputHtml,
            //    @"<head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><body><img src='{0}'><br><ul>{1}</ul>".Fmt(
            //        imgInf.OutputPng.HtmlEscape(),
            //        html.JoinString()));
        }

        private PointAxial? handleEdges(PointAxial ip, Direction dir, bool? isPositive)
        {
            if (_file.Grid.Size == 1)
                return new PointAxial(0, 0);

            var x = ip.Q;
            var z = ip.R;
            var y = -x - z;

            if (Ut.Max(Math.Abs(x), Math.Abs(y), Math.Abs(z)) < _file.Grid.Size)
                return ip;

            var xBigger = Math.Abs(x) >= _file.Grid.Size;
            var yBigger = Math.Abs(y) >= _file.Grid.Size;
            var zBigger = Math.Abs(z) >= _file.Grid.Size;

            // Move the pointer back to the hex near the edge
            ip -= dir.Vector;

            // If two values are still in range, we are wrapping around an edge (not a corner).
            if (!xBigger && !yBigger)
                return new PointAxial(ip.Q + ip.R, -ip.R);
            else if (!yBigger && !zBigger)
                return new PointAxial(-ip.Q, ip.Q + ip.R);
            else if (!zBigger && !xBigger)
                return new PointAxial(-ip.R, -ip.Q);

            // If two values are out of range, we navigated into a corner.
            if (isPositive == null)
                return null;
            // We teleport to a location that depends on the current memory value.
            if ((!xBigger && !isPositive.Value) || (!yBigger && isPositive.Value))
                return new PointAxial(ip.Q + ip.R, -ip.R);
            else if ((!yBigger && !isPositive.Value) || (!zBigger && isPositive.Value))
                return new PointAxial(-ip.Q, ip.Q + ip.R);
            else if ((!zBigger && !isPositive.Value) || (!xBigger && isPositive.Value))
                return new PointAxial(-ip.R, -ip.Q);

            // This should never be reached
            throw new InvalidOperationException();
        }

        private void paintImage(object sender, PaintEventArgs e)
        {
            if (_lastRendering != null)
                e.Graphics.DrawImage(_lastRendering, 0, 0);
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (_lastRendering != null)
            {
                _lastMouseDown = _file.FromScreen(e.X, e.Y);
                if (e.Button == MouseButtons.Right)
                    mnuContext.Show(ctImage, e.Location);
                else
                    DlgMessage.Show("The position you clicked is: " + _lastMouseDown.ToString(), "Axial coordinates", DlgType.Info);
            }
        }

        private int _pathSelectionCounter = 0;
        private HCPath _blinkingPath;
        private bool _ignoreOneListChange = false;
        private void selectPath(object sender, EventArgs e)
        {
            if (_ignoreOneListChange)
            {
                _ignoreOneListChange = false;
                return;
            }

            _pathSelectionCounter++;
            var thisCounter = _pathSelectionCounter;
            var timer = new Timer() { Enabled = true, Interval = 300 };
            var path = (HCPath) lstPaths.SelectedItem;
            if (path == null)
                return;
            var off = true;
            var origColor = path.Color;
            var iter = 7;

            timer.Tick += delegate
            {
                iter--;
                if (_pathSelectionCounter > thisCounter || iter == 0)
                {
                    _blinkingPath = null;
                    timer.Enabled = false;
                    timer.Dispose();
                }
                else
                {
                    _blinkingPath = off ? path : null;
                    off = !off;
                }
                rerender();
            };
        }

        private void editPath(object sender, EventArgs e)
        {
            editPath();
        }

        private void editPath()
        {
            var path = (HCPath) lstPaths.SelectedItem;
            if (path == null)
                return;

            using (var dlg = new ManagedForm(_settings.EditDialogSettings)
            {
                Text = "Edit path",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimizeBox = false,
                MaximizeBox = false
            })
            {
                var layout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));

                int row = 0;

                var addControl = Ut.Lambda((string label, Control control) =>
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    var lbl = new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Right };
                    layout.Controls.Add(lbl, 0, row);
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    control.Tag = lbl;
                    layout.Controls.Add(control, 1, row);
                    row++;
                    return control;
                });

                var addCheckbox = Ut.Lambda((string label) =>
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    var chk = new CheckBox { Text = label, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top, AutoSize = true };
                    layout.Controls.Add(chk, 1, row);
                    row++;
                    return chk;
                });

                var txtName = addControl("&Name:", new TextBox());
                var layoutColor = (TableLayoutPanel) addControl("Colo&r:", new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink });
                var chkDrawStart = addCheckbox("Draw &start");
                var chkDrawEnd = addCheckbox("Draw &end");
                var ddStartDir = (ComboBox) addControl("IP start &direction:", new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList });
                var chkMaxInstr = addCheckbox("&Limit number of instructions");
                var txtMaxInstr = (NumericUpDown) addControl("&Max instructions:", new NumericUpDown { Minimum = 1, DecimalPlaces = 0 });
                var ddBranch = (ComboBox) addControl("Take &branches:", new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList });
                var chkStop = addCheckbox("S&top at [, ], #");

                // Color
                var pnlColor = new Panel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
                layoutColor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                layoutColor.Controls.Add(pnlColor, 0, 0);

                var btnColor = new Button { Text = "...", Width = 50 };
                layoutColor.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layoutColor.Controls.Add(btnColor, 1, 0);

                // OK/Cancel buttons
                var flowLayout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
                var btnOk = new Button { Text = "&OK", DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "&Cancel", DialogResult = DialogResult.Cancel };
                flowLayout.Controls.Add(btnCancel);
                flowLayout.Controls.Add(btnOk);
                layout.Controls.Add(flowLayout, 0, row);
                layout.SetColumnSpan(flowLayout, 2);
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;
                row++;

                // “IP start direction” drop-down box
                ddStartDir.Items.Add(Direction.NorthEast);
                ddStartDir.Items.Add(Direction.East);
                ddStartDir.Items.Add(Direction.SouthEast);
                ddStartDir.Items.Add(Direction.SouthWest);
                ddStartDir.Items.Add(Direction.West);
                ddStartDir.Items.Add(Direction.NorthWest);

                // “Take branches” drop-down box
                var ddBranchMap = new[] { Tuple.Create((bool?) null, "Don’t"), Tuple.Create((bool?) true, "True"), Tuple.Create((bool?) false, "False") };
                foreach (var tup in ddBranchMap)
                    ddBranch.Items.Add(tup.Item2);

                // Populate all controls with the data
                txtName.Text = path.Name;
                pnlColor.BackColor = path.Color;
                chkDrawStart.Checked = path.DrawStart;
                chkDrawEnd.Checked = path.DrawEnd;
                ddStartDir.SelectedItem = path.IpStartDirection;
                chkMaxInstr.Checked = path.NumInstructions.HasValue;
                txtMaxInstr.Value = path.NumInstructions ?? 10;
                ddBranch.SelectedItem = ddBranchMap.First(tup => object.Equals(path.TakeBranch, tup.Item1)).Item2;
                chkStop.Checked = path.StopAtIpChanger;

                var updateDlgUi = Ut.Lambda(() =>
                {
                    txtMaxInstr.Visible = chkMaxInstr.Checked;
                    ((Control) txtMaxInstr.Tag).Visible = chkMaxInstr.Checked;
                });
                updateDlgUi();

                // Set events
                chkMaxInstr.Click += delegate { updateDlgUi(); };
                btnColor.Click += delegate
                {
                    using (var colorDlg = new ColorDialog { Color = pnlColor.BackColor, FullOpen = true, AnyColor = true })
                        if (colorDlg.ShowDialog() == DialogResult.OK)
                            pnlColor.BackColor = colorDlg.Color;
                };

                btnOk.Click += delegate
                {
                    // Put all the values from the controls back into the object
                    path.Name = txtName.Text;
                    path.Color = pnlColor.BackColor;
                    path.DrawStart = chkDrawStart.Checked;
                    path.DrawEnd = chkDrawEnd.Checked;
                    path.IpStartDirection = (Direction) ddStartDir.SelectedItem;
                    path.NumInstructions = chkMaxInstr.Checked ? (int) txtMaxInstr.Value : (int?) null;
                    path.TakeBranch = ddBranchMap.First(tup => tup.Item2 == ddBranch.SelectedItem).Item1;
                    path.StopAtIpChanger = chkStop.Checked;
                    rerender();
                    updateList();
                    _anyChanges = true;
                };

                dlg.Controls.Add(layout);
                dlg.ShowDialog();
            }
        }

        private void lstKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                editPath();
        }

        private void exiting(object sender, FormClosingEventArgs e)
        {
            if (!canDestroy())
                e.Cancel = true;
        }

        private void startPath(object sender, EventArgs e)
        {
            var newPath = new HCPath { IpStartPos = _lastMouseDown, Color = Color.Black, IpStartDirection = Direction.East, Name = "(new path)" };
            _file.Paths.Add(newPath);
            updateList();
            lstPaths.SelectedItem = newPath;
            editPath();
        }

        private void deletePath(object sender, EventArgs e)
        {
            if (DlgMessage.Show("Are you sure you wish to delete this path?", "Confirmation", DlgType.Question, "&Delete path", "&Cancel") == 0)
            {
                _file.Paths.Remove((HCPath) lstPaths.SelectedItem);
                updateList();
                rerender();
            }
        }

        private void setStartPos(object sender, EventArgs e)
        {
            var path = (HCPath) lstPaths.SelectedItem;
            if (path != null)
            {
                path.IpStartPos = _lastMouseDown;
                rerender();
            }
        }

        private void export(object _, EventArgs __)
        {
            if (_lastRendering != null)
                using (var save = new SaveFileDialog { Title = "Export as PNG", DefaultExt = "png", Filter = "PNG image files (*.png)|*.png" })
                {
                    var result = save.ShowDialog();
                    if (result == DialogResult.OK)
                        _lastRendering.Save(save.FileName);
                }
        }
    }
}
