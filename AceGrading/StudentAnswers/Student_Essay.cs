using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace AceGrading
{
    public class Student_Essay : Student_Answer
    {
        public string Answer { get; set; }

        public string RichTextAnswer { get; set; }

        public FlowDocument RichTextAnswer_Document
        {
            get
            {
                try
                {
                    FlowDocument flowDoc = new FlowDocument();
                    MemoryStream memory = new MemoryStream(Encoding.ASCII.GetBytes(this.RichTextAnswer));
                    TextRange textRange = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
                    textRange.Load(memory, DataFormats.Rtf);
                    return flowDoc;
                }
                catch { return null; } 
            }
        }

        public Student_Essay()
        {
            Answer = null;
        }

    }
}
