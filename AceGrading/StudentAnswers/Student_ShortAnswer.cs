using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace AceGrading
{
    public class Student_ShortAnswer : Student_Answer
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
                catch { return new FlowDocument(); }
            }
        }

        public Student_ShortAnswer()
        {
            Answer = null;
        }
    }
}
