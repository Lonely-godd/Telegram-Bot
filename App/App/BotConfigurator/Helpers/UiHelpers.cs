using System;
using System.Drawing;
using System.Windows.Forms;

namespace BotConfigurator
{
    internal static class UiHelpers
    {
        public static void StyleTextBox(TextBox tb)
        {
            tb.Enter += (_, _) => ((TextBox)tb).BackColor = Color.White;
            tb.Leave += (_, _) => ((TextBox)tb).BackColor = Theme.PageBg;
        }

        public static Button MakeSidebarButton(string text, Color bg, Color hover)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.White,
                BackColor = bg,
                FlatStyle = FlatStyle.Flat,
                Height = 36,
                Margin = new Padding(0, 0, 0, 6),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            return btn;
        }

        public static string BuildPath(TreeNode node)
        {
            var parts = new List<string>();
            var n = node;
            while (n != null)
            {
                parts.Insert(0, n.Text);
                n = n.Parent;
            }
            return string.Join(" / ", parts);
        }
    }
}