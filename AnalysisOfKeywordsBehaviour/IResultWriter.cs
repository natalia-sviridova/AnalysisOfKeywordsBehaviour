using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisOfKeywordsBehaviour
{
    interface IResultWriter
    {
        void ExportTables(DataGridView source1, DataGridView source2, DataGridView source3, DataGridView source4, string CorFactor1, DataGridView source5, DataGridView source6, string CorFactor2, DataGridView source7, DataGridView source8, string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8, string str9, string str10, string str11, string str12);
        void ExportField(List<List<string>> field);
        void ExportText(string text);
    }
}
