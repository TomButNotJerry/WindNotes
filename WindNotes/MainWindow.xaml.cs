using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindNotes.Functions;
using static System.Net.Mime.MediaTypeNames;

namespace WindNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //public FileSystemWatcher TextFileWatcher = new FileSystemWatcher
        //{
        //    Path = $"{Directory.GetCurrentDirectory()}\\scripts",
        //    Filter = "*.*",
        //    EnableRaisingEvents = true,
        //    IncludeSubdirectories = true,
        //};

        public MainWindow()
        {
            InitializeComponent();

            //TextFileWatcher.Created += (s, e) => this.Dispatcher.Invoke(() => RefreshScriptList());

            MarkdownViewer.Markdown = "";
        }

        #region Window Editing

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Markdown Parity
        private void TextEditor_TextChanged(object sender, EventArgs e) => MarkdownViewer.Markdown = TextEditor.Text;

        #endregion

        #region Menu
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem ActivatedBTN)
            {
                PopupMenuShow(ActivatedBTN.Header.ToString());
            }
        }


        void PopupMenuShow(string HeaderName)
        {
            TextOptions.Visibility = Visibility.Collapsed;
            FormatOptions.Visibility = Visibility.Collapsed;
            CodeOptions.Visibility = Visibility.Collapsed;
            AI.Visibility = Visibility.Collapsed;

            switch (HeaderName)
            {
                case "Text":
                    TextOptions.Visibility = Visibility.Visible;
                    Popup.Margin = new Thickness(78, 25, 0, 0);
                    break;
                case "Format":
                    FormatOptions.Visibility = Visibility.Visible;
                    Popup.Margin = new Thickness(122, 25, 0, 0);
                    break;
                case "Code":
                    CodeOptions.Visibility = Visibility.Visible;
                    Popup.Margin = new Thickness(183, 25, 0, 0);
                    break;
                case "AI":
                    AI.Visibility = Visibility.Visible;
                    Popup.Margin = new Thickness(232, 25, 0, 0);
                    break;
            }

            Popup.Visibility = Visibility.Visible;
        }

        private void ExitPopup_Click(object sender, RoutedEventArgs e) => Popup.Visibility = Visibility.Collapsed;


        private void ChangeTextMode_Click(object sender, RoutedEventArgs e)
        {
            if (TextEditor.Visibility == Visibility.Collapsed)
            {
                TextEditor.Visibility = Visibility.Visible;
                MarkdownViewer.Visibility = Visibility.Collapsed;
                ChangeTextMode.Header = "Edit Mode";
            }
            else
            {
                TextEditor.Visibility = Visibility.Collapsed;
                MarkdownViewer.Visibility = Visibility.Visible;
                ChangeTextMode.Header = "View Mode";
            }
        }

        #region Text Options

        private void CreateHeader_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Text = TextEditor.Text.Insert(TextEditor.Document.GetLineByNumber(TextEditor.Document.GetLineByOffset(TextEditor.CaretOffset).LineNumber).Offset, "# ");
        }

        private void CreateSubtext_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Text = TextEditor.Text.Insert(TextEditor.Document.GetLineByNumber(TextEditor.Document.GetLineByOffset(TextEditor.CaretOffset).LineNumber).Offset, "### ");
        }

        #endregion

        #region Format Options

        private void AddFormatToString(string ReplacingString)
        {
            if (TextEditor.SelectedText.Length > 0)
            {
                TextEditor.Text = TextEditor.Text.Substring(0, TextEditor.SelectionStart) +
                    ReplacingString +
                    TextEditor.Text.Substring(TextEditor.SelectionStart + TextEditor.SelectionLength);
            }
        }

        private void BoldText_Click(object sender, RoutedEventArgs e) => AddFormatToString($"**{TextEditor.SelectedText}**");

        private void ItalicText_Click(object sender, RoutedEventArgs e) => AddFormatToString($"*{TextEditor.SelectedText}*");

        private void BoldItalicText_Click(object sender, RoutedEventArgs e) => AddFormatToString($"***{TextEditor.SelectedText}***");

        #endregion

        #region Code Options

        private void CodeFormat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button ActivatedBTN)
            {
                AddFormatToString($"```{ActivatedBTN.Content}\n{TextEditor.SelectedText}\n```");
                Debug.WriteLine(ActivatedBTN.Content);

            }
        }

        #endregion

        #region Gemini Integration
        private void SubmitToAI_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(TextEditor.SelectedText);
            AddFormatToString($"{TextEditor.SelectedText}\n\n{Gemini.SendToGemini(AIPromptBox.Text, TextEditor.SelectedText)}");
        }

        #endregion

        #endregion
    }
}
