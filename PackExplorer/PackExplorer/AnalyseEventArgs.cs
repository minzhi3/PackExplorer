using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PackExplorer
{
    public class AnalyseEventArgs:EventArgs
    {
        public readonly Stream sm;
        public AnalyseEventArgs(Stream stream)
        {
            sm = stream;
        }
    }
}
