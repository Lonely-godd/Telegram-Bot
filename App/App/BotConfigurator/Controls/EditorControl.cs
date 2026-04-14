using System;
using System.Drawing;
using System.Windows.Forms;

namespace BotConfigurator
{
	public class EditorControl : Panel
	{
		public Label SectionPath { get; private set; }
		public LanguageCardControl CardRu { get; private set; }
		public LanguageCardControl CardUa { get; private set; }
		public Button SaveButton { get; private set; }

		public event EventHandler SaveClicked;

		public EditorControl()
		{
			Dock = DockStyle.Fill;
			BackColor = Theme.PageBg;

			SectionPath = new Label
			{
				Dock = DockStyle.Top,
				Height = 28,
				Font = Theme.FontSmall,
				ForeColor = Theme.TextMuted
			};

			var cardRow = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				ColumnCount = 2,
				RowCount = 1,
				Height = 330
			};
			cardRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
			cardRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

			CardRu = new LanguageCardControl("🇷🇺 Русский язык", "Заголовок (RU)", "Текст сообщения (RU)", new Padding(0, 0, 10, 0));
			CardUa = new LanguageCardControl("🇺🇦 Українська мова", "Заголовок (UA)", "Текст повідомлення (UA)", new Padding(10, 0, 0, 0));

			cardRow.Controls.Add(CardRu, 0, 0);
			cardRow.Controls.Add(CardUa, 1, 0);

			var spacer = new Panel { Dock = DockStyle.Top, Height = 16, BackColor = Theme.PageBg };

			SaveButton = new Button
			{
				Text = "СОХРАНИТЬ ИЗМЕНЕНИЯ",
				Dock = DockStyle.Top,
				Height = 44,
				Font = Theme.FontButton,
				ForeColor = Color.White,
				BackColor = Theme.Accent,
				FlatStyle = FlatStyle.Flat,
				Cursor = Cursors.Hand
			};
			SaveButton.FlatAppearance.BorderSize = 0;
			SaveButton.FlatAppearance.MouseOverBackColor = Theme.AccentDark;
			SaveButton.Click += (s, e) => SaveClicked?.Invoke(this, e);

			Controls.Add(SaveButton);
			Controls.Add(spacer);
			Controls.Add(cardRow);
			Controls.Add(SectionPath);
		}

		public void LoadSection(BotSection section)
		{
			CardRu.TitleBox.Text = section.Titles["ru"];
			CardRu.ContentBox.Text = section.Content["ru"];
			CardUa.TitleBox.Text = section.Titles["ua"];
			CardUa.ContentBox.Text = section.Content["ua"];
		}

		public void SaveToSection(BotSection section)
		{
			section.Titles["ru"] = CardRu.TitleBox.Text;
			section.Titles["ua"] = CardUa.TitleBox.Text;
			section.Content["ru"] = CardRu.ContentBox.Text;
			section.Content["ua"] = CardUa.ContentBox.Text;
		}

		public void Clear()
		{
			CardRu.TitleBox.Clear();
			CardRu.ContentBox.Clear();
			CardUa.TitleBox.Clear();
			CardUa.ContentBox.Clear();
		}
	}
}