using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BotConfigurator
{
    public class MainForm : Form
    {
        private SidebarControl _sidebar;
        private EditorControl _editor;
        private Panel _emptyState;
        private SplitContainer _split;
        private TreeView _tree;
        private BotSection _current;

        public MainForm()
        {
            Text = "Bot Configurator";
            Size = new Size(1120, 740);
            MinimumSize = new Size(920, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.PageBg;
            Font = Theme.FontBase;
            DoubleBuffered = true;

            BuildLayout();
            WireEvents();
        }

        private void BuildLayout()
        {
            Controls.Add(BuildHeader());
            Controls.Add(BuildBottomBar());

            _split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 1,
                SplitterDistance = 340,  
                BackColor = Color.FromArgb(45, 45, 58)
            };

            _sidebar = new SidebarControl();
            _split.Panel1.Controls.Add(_sidebar);
            _tree = _sidebar.Tree;

            var workspace = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.PageBg,
                Padding = new Padding(40, 32, 40, 24) 
            };

            _emptyState = BuildEmptyState();
            _editor = new EditorControl();
            _editor.Visible = false;

            workspace.Controls.Add(_editor);
            workspace.Controls.Add(_emptyState);

            _split.Panel2.Controls.Add(workspace);
            Controls.Add(_split);
        }
        private Panel BuildHeader()
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,                        
                BackColor = Theme.HeaderBg
            };

            var menu = new MenuStrip
            {
                BackColor = Theme.HeaderBg,
                ForeColor = Color.White,
                Dock = DockStyle.Top
            };
            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Новый проект", null, (_, _) => NewProject());
            fileMenu.DropDownItems.Add("Открыть JSON...", null, (_, _) => OpenJson());
            fileMenu.DropDownItems.Add("Экспортировать JSON", null, (_, _) => ExportJson());
            menu.Items.Add(fileMenu);
            header.Controls.Add(menu);

            header.Controls.Add(new Label
            {
                Text = "⚙",
                Font = new Font("Segoe UI", 16f),
                ForeColor = Theme.Accent,
                AutoSize = true,
                Location = new Point(20, 32)
            });
            header.Controls.Add(new Label
            {
                Text = "Bot Configurator",
                Font = Theme.FontAppTitle,
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(46, 35)
            });
            header.Controls.Add(new Label
            {
                Text = "Telegram · Visual Editor",
                Font = Theme.FontSmall,
                ForeColor = Theme.SidebarMuted,
                AutoSize = true,
                Location = new Point(48, 53)
            });

            return header;
        }

        private Panel BuildEmptyState()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Theme.PageBg };
            panel.Controls.Add(new Label
            {
                Text = "📂",
                Font = new Font("Segoe UI", 48f),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Theme.TextMuted
            });
            panel.Controls.Add(new Label
            {
                Text = "Выберите раздел или создайте новый",
                Font = new Font("Segoe UI", 13f),
                ForeColor = Theme.TextMuted,
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Bottom,
                Height = 220
            });
            return panel;
        }
        private Panel BuildBottomBar()
        {
            var bar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 68, 
                BackColor = Theme.Surface,
                Padding = new Padding(32, 12, 32, 12)
            };

            bar.Paint += (s, e) =>
            {
                using var pen = new Pen(Theme.Border);
                e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
            };
            var btnExport = new Button
            {
                Text = "⬇ ЭКСПОРТИРОВАТЬ JSON",
                Font = Theme.FontButton,
                ForeColor = Color.White,
                BackColor = Theme.Success,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(240, 42),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatAppearance.MouseOverBackColor = Theme.SuccessDark;
            btnExport.Location = new Point(bar.ClientSize.Width - 264, 10);
            btnExport.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnExport.Click += (_, _) => ExportJson();
            var statusLbl = new Label
            {
                Text = "Готов к работе",
                Font = Theme.FontSmall,
                ForeColor = Theme.TextMuted,
                AutoSize = true,
                Location = new Point(0, 22)
            };
            bar.Controls.Add(btnExport);
            bar.Controls.Add(statusLbl);
            return bar;
        }

        private void WireEvents()
        {
            _sidebar.AddRootClicked += (_, _) => AddNode(null);
            _sidebar.AddSubClicked += (_, _) => { if (_tree.SelectedNode != null) AddNode(_tree.SelectedNode); };
            _sidebar.DeleteClicked += (_, _) => DeleteSelected();

            _editor.SaveClicked += (_, _) => SaveCurrent();
            _tree.AfterSelect += Tree_AfterSelect;

            _tree.ItemDrag += (_, e) => _tree.DoDragDrop(e.Item, DragDropEffects.Move);
            _tree.DragEnter += (_, e) => e.Effect = DragDropEffects.Move;
            _tree.DragDrop += Tree_DragDrop;
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _current = (BotSection)e.Node.Tag;
            _editor.LoadSection(_current);
            _editor.SectionPath.Text = UiHelpers.BuildPath(e.Node);
            _emptyState.Visible = false;
            _editor.Visible = true;
        }

        private void Tree_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TreeNode)) is not TreeNode dragged) return;
            var target = _tree.GetNodeAt(_tree.PointToClient(new Point(e.X, e.Y)));
            if (target == null || target == dragged) return;
            dragged.Remove();
            if (target.Nodes.Count == 0 && target.Level > 0)
                target.Nodes.Add(dragged);
            else
            {
                target.Nodes.Add(dragged);
                target.Expand();
            }
            _tree.SelectedNode = dragged;
        }

        private void AddNode(TreeNode parent)
        {
            var section = new BotSection();
            var node = new TreeNode("Новый раздел") { Tag = section };
            if (parent == null)
                _tree.Nodes.Add(node);
            else
            {
                parent.Nodes.Add(node);
                parent.Expand();
            }
            _tree.SelectedNode = node;
        }

        private void DeleteSelected()
        {
            if (_tree.SelectedNode == null) return;
            var msg = $"Удалить «{_tree.SelectedNode.Text}»?";
            if (MessageBox.Show(msg, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _tree.SelectedNode.Remove();
            _current = null;
            _editor.Visible = false;
            _emptyState.Visible = true;
            _editor.Clear();
        }

        private void SaveCurrent()
        {
            if (_current == null) return;
            _editor.SaveToSection(_current);
            if (_tree.SelectedNode != null)
                _tree.SelectedNode.Text = string.IsNullOrWhiteSpace(_current.Titles["ru"]) ? "Новый раздел" : _current.Titles["ru"];
        }

        private void NewProject()
        {
            _tree.Nodes.Clear();
            _current = null;
            _editor.Visible = false;
            _emptyState.Visible = true;
            _editor.Clear();
        }

        private void OpenJson()
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Открыть конфигурацию",
                Filter = "JSON-файл (*.json)|*.json"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var json = File.ReadAllText(ofd.FileName);
            var config = JsonConvert.DeserializeObject<BotConfig>(json);
            _tree.Nodes.Clear();
            foreach (var section in config.Sections)
                AddSectionToTree(section, null);
            _current = null;
            _editor.Visible = false;
            _emptyState.Visible = true;
            _editor.Clear();
        }

        private void AddSectionToTree(BotSection section, TreeNode parent)
        {
            var node = new TreeNode(section.ToString()) { Tag = section };
            if (parent == null)
                _tree.Nodes.Add(node);
            else
                parent.Nodes.Add(node);
            foreach (var sub in section.SubSections)
                AddSectionToTree(sub, node);
        }

        private void ExportJson()
        {
            if (_editor.Visible) SaveCurrent();
            var config = new BotConfig();
            foreach (TreeNode node in _tree.Nodes)
                config.Sections.Add(CollectSection(node));
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            using var sfd = new SaveFileDialog
            {
                Title = "Сохранить конфигурацию",
                Filter = "JSON-файл (*.json)|*.json",
                FileName = "bot_config.json"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(sfd.FileName, json);
            MessageBox.Show("Конфигурация успешно экспортирована!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static BotSection CollectSection(TreeNode node)
        {
            var section = (BotSection)node.Tag;
            section.SubSections.Clear();
            foreach (TreeNode child in node.Nodes)
                section.SubSections.Add(CollectSection(child));
            return section;
        }
    }
}