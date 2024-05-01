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
        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            MarkdownViewer.Markdown = TextEditor.Text;
        }

        #endregion

        #region Menu

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
            if (TextEditor.LineCount > 1)
                TextEditor.TextArea.PerformTextInput("\n");

            TextEditor.Text = TextEditor.Text.Insert(TextEditor.SelectionStart, "# ");
        }

        private void CreateSubtext_Click(object sender, RoutedEventArgs e)
        {
            if (TextEditor.LineCount > 1)
                TextEditor.TextArea.PerformTextInput("\n");

            TextEditor.Text = TextEditor.Text.Insert(TextEditor.SelectionStart, "### ");
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

        #endregion

        private void SubmitToAI_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.TextArea.PerformTextInput(Gemini.SendToGemini(AIPromptBox.Text, TextEditor.SelectedText));
        }
    }
}
