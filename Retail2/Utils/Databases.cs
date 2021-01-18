using Retail2.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Retail2.Classes.Enum;

namespace Retail2.Utils
{
    class Databases
    {
        private static Random r = new Random();

        public static DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
        {
            try
            {
                ArrayList UniqueRecords = new ArrayList();
                ArrayList DuplicateRecords = new ArrayList();

                // Check if records is already added to UniqueRecords otherwise,
                // Add the records to DuplicateRecords
                foreach (DataRow dRow in table.Rows)
                {
                    if (UniqueRecords.Contains(dRow[DistinctColumn]))
                        DuplicateRecords.Add(dRow);
                    else
                        UniqueRecords.Add(dRow[DistinctColumn]);
                }

                // Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow dRow in DuplicateRecords)
                {
                    table.Rows.Remove(dRow);
                }

                // Return the clean DataTable which contains unique records.
                return table;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Size getSize(String str)
        {
            string[] s = str.Split(';');
            int i = Int32.Parse(s[0]);
            int i2 = Int32.Parse(s[1]);
            return new Size(i, i2);
        }

        public static Point getLocation(String str)
        {
            string[] s = str.Split(';');
            int i = Int32.Parse(s[0]);
            int i2 = Int32.Parse(s[1]);
            return new Point(i, i2);
        }

        public static string getIdentifier(IdentifierType t)
        {
            int i = 3;
            if (t == IdentifierType.ORDER)
            {
                i = 20;
            }
            if (t == IdentifierType.PRODUCT)
            {
                i = 10;
            }
            if (t == IdentifierType.USER)
            {
                i = 5;
            }
            if (t == IdentifierType.CATEGORY)
            {
                i = 4;
            }
            if (t == IdentifierType.PROFILE)
            {
                i = 15;
            }
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZzxcvbnmalskdjfhgpqowieuryt0123456789";
            return new string(Enumerable.Repeat(chars, i)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        public static string compactData(DataTable dataTable)
        {
            string data = string.Empty;
            int rowsCount = dataTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                DataRow row = dataTable.Rows[i];
                int columnsCount = dataTable.Columns.Count;
                if (row.RowState != DataRowState.Deleted)
                {
                    for (int j = 0; j < columnsCount; j++)
                    {
                        data += dataTable.Columns[j].ColumnName + "~" + row[j];
                        if (j == columnsCount - 1)
                        {
                            if (i != (rowsCount - 1))
                                data += "$";
                        }
                        else
                            data += "|";
                    }
                }
            }
            return data;
        }

        public static DataTable uncompactTable(string data)
        {
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;
            foreach (string row in data.Split('$'))
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (string cell in row.Split('|'))
                {
                    if (string.IsNullOrEmpty(cell) == false)
                    {
                        string[] keyValue = cell.Split('~');
                        if (!columnsAdded)
                        {
                            DataColumn dataColumn = new DataColumn(keyValue[0]);
                            dataTable.Columns.Add(dataColumn);
                        }
                        dataRow[keyValue[0]] = keyValue[1];
                    }
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        static Dictionary<String, Image> dic = new Dictionary<string, Image>();

        public static Image getImage(String ident)
        {
            if (dic.ContainsKey(ident))
            {
                return dic[ident];
            }
            else
            {
                String s = SettingsManager.getDataPath() + @"\Images\Products\" + ident + ".jpg";
                if (File.Exists(s))
                {
                    Image i = Image.FromFile(SettingsManager.getDataPath() + @"\Images\Products\" + ident + ".jpg");
                    dic.Add(ident, i);
                    return i;
                }
                else
                {
                    dic.Add(ident, Retail2.Properties.Resources.noimage);
                    return Retail2.Properties.Resources.noimage;
                }
            }
        }

        public static String getPaymentString(OrderCheckout o)
        {
            if (o == OrderCheckout.CASH)
            {
                return "Dinheiro";
            }
            if (o == OrderCheckout.CHECK)
            {
                return "Dinheiro";
            }
            if (o == OrderCheckout.CREDIT_CARD)
            {
                return "Cartão De Crédito";
            }
            if (o == OrderCheckout.MBWAY)
            {
                return "MB WAY";
            }
            if (o == OrderCheckout.PAYPAL)
            {
                return "PAYPAL";
            }
            return "";
        }

        public static String compactList(List<String> l)
        {
            return String.Join("§", l.ToArray());
        }

        public static List<string> uncompactList(String s)
        {
            if (s != null)
            {
                return s.Split('§').ToList();
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
