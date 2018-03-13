using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEC_H2_Dating
{
    public static class Extensions
    {
        public static DataSet ToDataSet(this string input)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = dataSet.Tables.Add();
            dataTable.Columns.Add();
            dataTable.Rows.Add(input);
            return dataSet;
        }
    }
}
