using System;
using System.Drawing;
using System.Windows.Forms;

namespace BotConfigurator
{
    public class SidebarControl : Panel
    {
        public TreeView Tree { get; private set; }

        public event EventHandler AddRootClicked;
        public event EventHandler AddSubClicked;
        public event EventHandler DeleteClicked;

        public SidebarControl()
        {
            Dock = DockStyle.Left;
            Width = 290;
            BackColor = Theme.SidebarBg;

            // заголовок дерева
            var treeHeader = new Panel { Dock = DockStyle.Top, Height = 38, BackColor = Color.FromArgb(18, 18, 25), Padding = new Padding(16, 0, 8, 0) };
            treeHeader.Controls.Add(new Label { Text = "СТРУКТУРА РАЗДЕЛОВ", Font = Theme.FontCaps, ForeColor = Theme.SidebarMuted, AutoSize = true, Location = new Point(16, 12) });

            Tree = new TreeView
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.SidebarBg,
                ForeColor = Theme.SidebarText,
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.None,
                ShowLines = true,
                ShowPlusMinus = true,
                ItemHeight = 32,
                Indent = 18,
                HotTracking = true,
                DrawMode = TreeViewDrawMode.OwnerDrawText,
                AllowDrop = true
            };
            Tree.DrawNode += Tree_DrawNode;

            // кнопки
            var buttonArea = new Panel { Dock = DockStyle.Bottom, Height = 148, BackColor = Color.FromArgb(17, 17, 24), Padding = new Padding(12, 10, 12, 10) };

            var btnAddRoot = UiHelpers.MakeSidebarButton("＋ Добавить раздел", Theme.Accent, Theme.AccentDark);
            var btnAddSub = UiHelpers.MakeSidebarButton("↳ Добавить подраздел", Color.FromArgb(55, 65, 90), Color.FromArgb(45, 55, 78));
            var btnDelete = UiHelpers.MakeSidebarButton("✕ Удалить выбранное", Theme.Danger, Theme.DangerDark);

            btnAddRoot.Click += (_, _) => AddRootClicked?.Invoke(this, EventArgs.Empty);
            btnAddSub.Click += (_, _) => AddSubClicked?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (_, _) => DeleteClicked?.Invoke(this, EventArgs.Empty);

            foreach (var b in new[] { btnDelete, btnAddSub, btnAddRoot })
            {
                b.Dock = DockStyle.Top;
                buttonArea.Controls.Add(b);
            }

            Controls.Add(Tree);
            Controls.Add(treeHeader);
            Controls.Add(buttonArea);
        }

        private void Tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var bounds = e.Bounds;
            bool selected = (e.State & TreeNodeStates.Selected) != 0;
            bool hot = (e.State & TreeNodeStates.Hot) != 0;

            e.Graphics.FillRectangle(new SolidBrush(selected ? Theme.SidebarSelected : hot ? Theme.SidebarHover : Theme.SidebarBg), bounds);

            var prefix = e.Node.Level > 0 ? " " + new string(' ', e.Node.Level * 2) : "";
            var icon = e.Node.Nodes.Count > 0 ? (selected ? "▾ " : "▸ ") : " · ";
            var color = selected ? Color.White : (hot ? Color.White : Theme.SidebarText);

            TextRenderer.DrawText(e.Graphics, prefix + icon + e.Node.Text, Tree.Font,
                new Point(bounds.X + 4, bounds.Y + (bounds.Height - Tree.Font.Height) / 2), color);
        }
    }
}