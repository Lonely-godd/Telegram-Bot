using System.Drawing;
using System.Windows.Forms;

namespace BotConfigurator
{
    public class LanguageCardControl : Panel
    {
        public TextBox TitleBox { get; private set; }
        public TextBox ContentBox { get; private set; }

        public LanguageCardControl(string header, string titlePlaceholder, string contentPlaceholder, Padding margin)
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.Surface;
            Margin = margin;
            Padding = new Padding(20, 16, 20, 16);

            Paint += (s, e) =>
            {
                using var pen = new Pen(Theme.Border);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };

            // заголовок языка
            var lbl = new Label { Text = header, Font = Theme.FontSection, ForeColor = Theme.TextPrimary, Dock = DockStyle.Top, Height = 38 };
            var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Theme.Border };
            var spacer1 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Theme.Surface };

            var lblTitle = new Label { Text = "ЗАГОЛОВОК", Font = Theme.FontCaps, ForeColor = Theme.TextSecondary, Dock = DockStyle.Top, Height = 22 };

            TitleBox = new TextBox { Dock = DockStyle.Top, Height = 34, Font = Theme.FontBase, BackColor = Theme.PageBg, ForeColor = Theme.TextPrimary, BorderStyle = BorderStyle.FixedSingle, PlaceholderText = titlePlaceholder };
            UiHelpers.StyleTextBox(TitleBox);

            var spacer2 = new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Theme.Surface };

            var lblContent = new Label { Text = "ТЕКСТ СООБЩЕНИЯ", Font = Theme.FontCaps, ForeColor = Theme.TextSecondary, Dock = DockStyle.Top, Height = 22 };

            ContentBox = new TextBox { Dock = DockStyle.Fill, Font = Theme.FontBase, BackColor = Theme.PageBg, ForeColor = Theme.TextPrimary, BorderStyle = BorderStyle.FixedSingle, Multiline = true, ScrollBars = ScrollBars.Vertical, PlaceholderText = contentPlaceholder };
            UiHelpers.StyleTextBox(ContentBox);

            Controls.Add(ContentBox);
            Controls.Add(lblContent);
            Controls.Add(spacer2);
            Controls.Add(TitleBox);
            Controls.Add(lblTitle);
            Controls.Add(spacer1);
            Controls.Add(divider);
            Controls.Add(lbl);
        }
    }
}