using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZCode.Debug
{
    public class Debugger
    {
        /// <summary>
        /// Breakpoint Array <seealso cref="Breakpoint"/>
        /// </summary>
        public Breakpoint[] Breakpoints { get; set; }
        /// <summary>
        /// EZCode Instance
        /// </summary>
        public EzCode ezcode;
        /// <summary>
        /// Currently stopped
        /// </summary>
        public bool Stopped = false;
        /// <summary>
        /// <see cref="NextSegment(bool)"/> just ran
        /// </summary>
        public bool NextSegmentClicked = false;
        /// <summary>
        /// Current Line 
        /// </summary>
        public string Line = "";
        /// <summary>
        /// Textbox for highlighting
        /// </summary>
        public TextBox HighlightTextbox;
        public bool IsPlaying { get => ezcode != null ? ezcode.playing : false; }
        /// <summary>
        /// Debug Manager for EZCode
        /// </summary>
        /// <param name="ezcode">EZCode Instance</param>
        /// <param name="textBox">Textbox for highlighting (optional)</param>
        public Debugger(EzCode ezcode, TextBox? textBox = null)
        {
            HighlightTextbox = textBox ?? new TextBox();
            this.ezcode = ezcode;
        }
        /// <summary>
        /// Debug Manager for EZCode
        /// </summary>
        /// <param name="ezcode">EZCode Instance</param>
        /// <param name="breakpoints">Breakpoint Array</param>
        /// <param name="textBox">Textbox for highlighting (optional)</param>
        public Debugger(EzCode ezcode, Breakpoint[] breakpoints, TextBox? textBox = null)
        {
            HighlightTextbox = textBox ?? new TextBox();
            this.ezcode = ezcode;
            Breakpoints = breakpoints;
        }
        /// <summary>
        /// Last Breakpoint Hit. May be null
        /// </summary>
        public Breakpoint? LastBreapoint { get; private set; }

        public void Hit(string line)
        {
            try
            {
                Line = line.Trim();
                Stopped = NextSegmentClicked ? true : false;
                NextSegmentClicked = false;

                if (Breakpoints.Any(x => ezcode.ScriptDirectory == x.FilePath))
                {
                    Breakpoint? breakpoint = Breakpoints.Where(x => ezcode.codeLine == x.Segment).First();
                    if (breakpoint != null && breakpoint.Enabled)
                    {
                        LastBreapoint = breakpoint;
                        if (breakpoint.RemoveOnFirstHit) Breakpoints.ToList().Remove(breakpoint);
                        Stopped = true;
                    }
                }

                throw new Exception();
            }
            catch
            {
                if (Stopped)
                {
                    HighlightTextbox = TextboxHighlight();
                }
            }
        }

        public async void StartDebugSession(string code)
        {
            await ezcode.Play(code, debugger: this);
        }

        public void StopDebugSession()
        {
            ezcode.Stop();
        }

        public void NextSegment(bool allow_pause = true)
        {
            NextSegmentClicked = true;
            Stopped = !Stopped && !allow_pause ? Stopped : false;
        }

        public void NextBreakpoint()
        {
            Stopped = false;
        }

        public TextBox TextboxHighlight(TextBox? textBox = null)
        {
            textBox ??= HighlightTextbox;
            textBox ??= new TextBox();
            
            string[] tb_lines = textBox.Text.Split(Environment.NewLine);

            int occurrences = CountOccurrences(tb_lines, Line);

            if (occurrences == 1)
            {
                int line = tb_lines.ToList().IndexOf(Line);
                int chars = 0;
                for (int i = 0; i < line; i++)
                {
                    chars += tb_lines[i].Length + 2;
                }

                textBox.Select(chars, Line.Length);
            }
            else if (occurrences > 1)
            {
                int s = 0;
                foreach(string line in tb_lines)
                {
                    s += line.Length;
                    try
                    {
                        if (line.StartsWith("method") && line.Contains($" {LastBreapoint.Method}"))
                        {
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }


                int startIndex = 0;

                while (startIndex < textBox.TextLength)
                {
                    int index = textBox.Text.IndexOf(Line, startIndex, StringComparison.OrdinalIgnoreCase);

                    if (index == -1)
                        break;

                    textBox.Select(index, Line.Length);

                    startIndex = index + Line.Length;
                }
            }
            return textBox;
        }
        static int CountOccurrences(string[] array, string target)
        {
            int count = 0;

            foreach (string item in array)
            {
                if (item == target)
                {
                    count++;
                }
            }

            return count;
        }

        public void Dispose()
        {
            this?.Dispose();
            ezcode = null;
            Breakpoints = null;
        }
    }
    public class Breakpoint
    {
        public bool Enabled { get; set; } = true;
        public bool RemoveOnFirstHit { get; set; } = false;
        public string FilePath { get; set; }
        public string Method { get; set; }
        public int Segment { get; set; }
        public Breakpoint() { }
        public Breakpoint(int segment) => Segment = segment;
        public Breakpoint(string file) => FilePath = file;
        public Breakpoint(bool remove) => RemoveOnFirstHit = remove;
        public Breakpoint(string file, int segment, bool remove, string method, bool enabled)
        {
            Segment = segment;
            FilePath = file;
            RemoveOnFirstHit = remove;
            Method = method;
            Enabled = enabled;
        }
    }
}
