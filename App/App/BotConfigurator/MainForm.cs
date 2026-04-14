using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BotConfigurator
{
    public partial class MainForm : Form
    {
        private TreeView treeView; 
        private TextBox txtTitleRu, txtTitleUa, txtContentRu, txtContentUa;
        private Button btnAddRoot, btnAddSub, btnDelete, btnExport, btnSaveChanges;
        private BotSection selectedSection;

        public MainForm()
        {
            this.Text = "Telegram Bot Configurator (Visual Editor)";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            InitializeLayout();
        }

        private void InitializeLayout()
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            // Left Side: Tree & Buttons
            Panel leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            treeView = new TreeView { Dock = DockStyle.Fill, ShowLines = true, ShowPlusMinus = true };
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.BeforeSelect += TreeView_BeforeSelect;

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 100 };
            btnAddRoot = new Button { Text = "Добавить раздел", Size = new Size(140, 35) };
            btnAddSub = new Button { Text = "Добавить подраздел", Size = new Size(140, 35) };
            btnDelete = new Button { Text = "Удалить", Size = new Size(140, 35), ForeColor = Color.Red };
            btnSaveChanges = new Button { Text = "Сохранить изменение", Size = new Size(140, 35) };

            btnAddRoot.Click += (s, e) => AddSection(null);
            btnAddSub.Click += (s, e) => { if (treeView.SelectedNode != null) AddSection(treeView.SelectedNode); };
            btnDelete.Click += (s, e) => { if (treeView.SelectedNode != null) { treeView.SelectedNode.Remove(); selectedSection = null; ClearFields(); } };

            buttonPanel.Controls.AddRange(new Control[] { btnAddRoot, btnAddSub, btnDelete, btnSaveChanges });
            leftPanel.Controls.Add(treeView);
            leftPanel.Controls.Add(buttonPanel);

            // Right Side: Editor
            Panel rightPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            Label lblRu = new Label { Text = "РУССКИЙ ЯЗЫК", Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 30 };
            txtTitleRu = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Заголовок (RU)" };
            txtContentRu = new TextBox { Dock = DockStyle.Top, Multiline = true, Height = 100, PlaceholderText = "Текст сообщения (RU)" };
            
            Label lblUa = new Label { Text = "УКРАЇНСЬКА МОВА", Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 30, Margin = new Padding(0, 20, 0, 0) };
            txtTitleUa = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Заголовок (UA)" };
            txtContentUa = new TextBox { Dock = DockStyle.Top, Multiline = true, Height = 100, PlaceholderText = "Текст повідомлення (UA)" };

            btnExport = new Button { Text = "ЭКСПОРТ В JSON", Dock = DockStyle.Bottom, Height = 50, BackColor = Color.LightGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnExport.Click += BtnExport_Click;

            // Update events
            btnSaveChanges.Click += SyncData;

            rightPanel.Controls.Add(txtContentUa);
            rightPanel.Controls.Add(new Control { Height = 10, Dock = DockStyle.Top });
            rightPanel.Controls.Add(txtTitleUa);
            rightPanel.Controls.Add(lblUa);
            rightPanel.Controls.Add(new Control { Height = 30, Dock = DockStyle.Top });
            rightPanel.Controls.Add(txtContentRu);
            rightPanel.Controls.Add(new Control { Height = 10, Dock = DockStyle.Top });
            rightPanel.Controls.Add(txtTitleRu);
            rightPanel.Controls.Add(lblRu);
            rightPanel.Controls.Add(btnExport);

            mainLayout.Controls.Add(leftPanel, 0, 0);
            mainLayout.Controls.Add(rightPanel, 1, 0);
            this.Controls.Add(mainLayout);
        }

        private void AddSection(TreeNode parent)
        {
            var section = new BotSection();
            TreeNode node = new TreeNode("Новый раздел") { Tag = section };
            if (parent == null) treeView.Nodes.Add(node);
            else parent.Nodes.Add(node);
            treeView.SelectedNode = node;
            node.Expand();
        }

        private void TreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(treeView.SelectedNode != null)
                treeView.SelectedNode.BackColor = Color.Empty;
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedSection = (BotSection)e.Node.Tag;
            txtTitleRu.Text = selectedSection.Titles["ru"];
            txtTitleUa.Text = selectedSection.Titles["ua"];
            txtContentRu.Text = selectedSection.Content["ru"];
            txtContentUa.Text = selectedSection.Content["ua"];
            treeView.SelectedNode.BackColor = Color.AliceBlue;
        }

        private void SyncData(object sender, EventArgs e)
        {
            if (selectedSection == null) return;
            selectedSection.Titles["ru"] = txtTitleRu.Text;
            selectedSection.Titles["ua"] = txtTitleUa.Text;
            selectedSection.Content["ru"] = txtContentRu.Text;
            selectedSection.Content["ua"] = txtContentUa.Text;
            if (treeView.SelectedNode != null)
                treeView.SelectedNode.Text = string.IsNullOrWhiteSpace(txtTitleRu.Text) ? "Новый раздел" : txtTitleRu.Text;
        }

        private void ClearFields()
        {
            txtTitleRu.Clear(); txtTitleUa.Clear(); txtContentRu.Clear(); txtContentUa.Clear();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var config = new BotConfig();
            foreach (TreeNode node in treeView.Nodes)
            {
                config.Sections.Add(MapNodeToSection(node));
            }

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            
            SaveFileDialog sfd = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "bot_config.json" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, json);
                MessageBox.Show("Конфигурация успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private BotSection MapNodeToSection(TreeNode node)
        {
            var section = (BotSection)node.Tag;
            section.SubSections.Clear();
            foreach (TreeNode child in node.Nodes)
            {
                section.SubSections.Add(MapNodeToSection(child));
            }
            return section;
        }
    }
}
