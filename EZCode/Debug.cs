using static EZCodeLanguage.Parser;

namespace EZCodeLanguage
{
    public static class Debug
    {
        public class Breakpoint
        {
            public Line Line { get; set; }
            public bool Enabled { get; set; }
            public bool DisableWhenHit { get; set; }
            public string? EZCodeConditionToHit { get; set; }
            public string? EZCodeActionWhenHit { get; set; }
            public Breakpoint? EnabledWhenBreakpointIsHit { get; set; }
            public bool Hit { get; set; }
            public Breakpoint(Line line, bool enabled = true, bool disableWhenHit = false,
                string? ezcodeConditionToHit = null, string? ezcodeActionWhenHit = null, 
                Breakpoint? enabledWhenBreakpointIsHit = null)
            {
                Line = line;
                Enabled = enabled;
                DisableWhenHit = disableWhenHit;
                EZCodeConditionToHit = ezcodeConditionToHit;
                EZCodeActionWhenHit = ezcodeActionWhenHit;
                EnabledWhenBreakpointIsHit = enabledWhenBreakpointIsHit;
            }
        }
        public static bool IsHit(Line line, Breakpoint[] breakpoints, Interpreter interpreter)
        {
            var point = breakpoints.FirstOrDefault(x => x.Line.FilePath == line.FilePath && x.Line.CodeLine == line.CodeLine);
            bool hit = point != null && point.Enabled;

            if (hit && point != null)
            {
                if (point.EZCodeConditionToHit != null)
                {
                    Parser parser = new Parser(point.EZCodeConditionToHit, "Debugger");
                    var lines = parser.Parse();
                    object result = interpreter.GetValue(lines);
                    if (result != null)
                    {
                        if (bool.TryParse(result.ToString(), out bool res))
                        {
                            hit = res;
                        }
                        else
                        {
                            throw new Exception($"Error From Debugger in line \"{line.CodeLine}\" The condition does not return boolean");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error From Debugger in line \"{line.CodeLine}\" The condition returns null instead of boolean");
                    }
                }
                if (hit && point.EZCodeActionWhenHit != null)
                {
                    Parser parser = new Parser(point.EZCodeActionWhenHit, "Debugger");
                    var lines = parser.Parse();
                    foreach (var l in lines)
                    {
                        interpreter.SingleLine(l);
                    }
                }
                if (point.EnabledWhenBreakpointIsHit != null)
                {
                    point.EnabledWhenBreakpointIsHit.Enabled = true;
                }
                if (point.DisableWhenHit)
                {
                    point.Enabled = false;
                }
                point.Hit = hit;
            }

            return hit;
        }
    }
}
