using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MFilesAdminStudio.VBScriptExplorerModule
{
    public class ColorizeAvalonEdit : DocumentColorizingTransformer
    {
        private readonly SolidColorBrush selectionBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0078D7");
        private readonly SolidColorBrush selectionForeground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");

        public string SelectedText { get; set; }

        public ColorizeAvalonEdit()
        {
            selectionBackground.Opacity = 0.75;
            selectionBackground.Freeze();
            selectionForeground.Freeze();
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (string.IsNullOrWhiteSpace(SelectedText))
            {
                return;
            }

            int lineStartOffset = line.Offset;
            string text = CurrentContext.Document.GetText(line);
            int start = 0;
            int index;

            while ((index = text.IndexOf(SelectedText, start)) >= 0)
            {
                ChangeLinePart(
                    lineStartOffset + index, // startOffset
                    lineStartOffset + index + SelectedText.Length, // endOffset
                    (VisualLineElement element) => {

                        element.TextRunProperties.SetBackgroundBrush(selectionBackground);
                        element.TextRunProperties.SetForegroundBrush(selectionForeground);

                        Typeface tf = element.TextRunProperties.Typeface;
                        // Replace the typeface with a modified version of
                        // the same typeface
                        //element.TextRunProperties.SetTypeface(new Typeface(
                        //        tf.FontFamily,
                        //        FontStyles.Italic,
                        //        FontWeights.Bold,
                        //        tf.Stretch
                        //    ));
                    });
                start = index + 1; // search for next occurrence
            }
        }
    }
}
