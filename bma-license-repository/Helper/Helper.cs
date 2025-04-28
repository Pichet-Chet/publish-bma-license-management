
using System.Data;
using bma_license_repository.Dto;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace bma_license_repository.Helper
{
    public static class Helper
    {
        public static Guid GenerateGuid()
        {
            Guid guid = Guid.NewGuid();

            return guid;
        }

        public static string GenerateShortGuid()
        {
            Guid guid = Guid.NewGuid();

            string shortGuid = guid.ToString().Replace("-", "").Substring(0, 6);

            return shortGuid;
        }

        public static Guid ToGuid(this string value)
        {
            return new Guid(value);
        }

        public static decimal ToDecimal(this object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

        public static short ToShort(this object value)
        {
            try
            {
                return short.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static int ToInt(this object value)
        {
            try
            {
                return int.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static DataTable ReadExcelFlie(string filePath)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();

            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                int MaxCell = 0;
                foreach (IXLRow row in workSheet.Rows())
                {
                    var rowCellCount = row.Cells().Count();
                    if (rowCellCount > MaxCell)
                    {
                        MaxCell = rowCellCount;
                    }
                }
                for (int i = 1; i <= MaxCell; i++)
                {
                    dt.Columns.Add("column" + i);
                }
                //Loop through the Worksheet rows.
                bool firstRow = false;
                foreach (IXLRow row in workSheet.Rows())
                {

                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {

                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
            }
            return dt;
        }

        public static DataTable ReadExcelFileLicense(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                // Get the first worksheet
                var worksheet = workbook.Worksheet(1);

                // Find the maximum number of columns in the worksheet
                int maxColumns = worksheet.FirstRowUsed()
                                           .CellsUsed()
                                           .Count();

                // Add columns to the DataTable
                for (int col = 1; col <= maxColumns; col++)
                {
                    dataTable.Columns.Add($"Column{col}");
                }

                // Populate DataTable with data from the worksheet
                foreach (var row in worksheet.RowsUsed())
                {
                    var dataRow = dataTable.NewRow();
                    for (int col = 1; col <= maxColumns; col++)
                    {
                        dataRow[col - 1] = row.Cell(col).GetValue<string>();
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        public static string ToDateTimeString(this DateTime value, string formatDatetime)
        {
            return value.ToString(formatDatetime);
        }

        public static string ToDateTimeString(this DateTime? value, string formatDatetime)
        {
            if (value.HasValue)
            {
                return value.Value.ToString(formatDatetime);
            }
            return string.Empty;
        }

        public static DateTime ToDateTime(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return DateTime.Parse(value);
            }
            return DateTime.MinValue;
        }

        public static DateTime? ToDateTimeNull(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return DateTime.Parse(value);
            }
            return null;
        }

        public static string GetSortName(SysOrgranize sysOrgranize, string columnName)
        {
            if (string.IsNullOrEmpty(columnName) || sysOrgranize == null)
            {
                return columnName;
            }

            return typeof(SysOrgranize)
                .GetProperties()
                .FirstOrDefault(prop => columnName.Contains(prop.Name))
                ?.Name ?? columnName;
        }

        public static DataTable? ReadToExcel(this string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                DataTable dataTable = new DataTable();

                using (var workbook = new XLWorkbook(filePath))
                {
                    // Get the first worksheet
                    var worksheet = workbook.Worksheet(1);

                    // Find the maximum number of columns in the worksheet
                    int maxColumns = worksheet.FirstRowUsed()
                                               .CellsUsed()
                                               .Count();

                    // Add columns to the DataTable
                    for (int col = 1; col <= maxColumns; col++)
                    {
                        dataTable.Columns.Add($"Column{col}");
                    }

                    // Populate DataTable with data from the worksheet
                    foreach (var row in worksheet.RowsUsed())
                    {
                        var dataRow = dataTable.NewRow();
                        for (int col = 1; col <= maxColumns; col++)
                        {
                            dataRow[col - 1] = row.Cell(col).GetValue<string>();
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
            else
            {
                return null;
            }
        }

        public static DataTable? ReadToExcel(this string filePath, int skip)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                DataTable dataTable = new DataTable();

                using (var workbook = new XLWorkbook(filePath))
                {
                    // Get the first worksheet
                    var worksheet = workbook.Worksheet(1);

                    // Find the maximum number of columns in the worksheet
                    int maxColumns = worksheet.Row(skip)
                                               .CellsUsed()
                                               .Count();

                    // Add columns to the DataTable
                    for (int col = 1; col <= maxColumns; col++)
                    {
                        dataTable.Columns.Add($"Column{col}");
                    }

                    // Populate DataTable with data from the worksheet
                    foreach (var row in worksheet.RowsUsed())
                    {
                        var dataRow = dataTable.NewRow();
                        for (int col = 1; col <= maxColumns; col++)
                        {
                            dataRow[col - 1] = row.Cell(col).GetValue<string>();
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
            else
            {
                return null;
            }
        }

        public static string UploadFile(this IFormFile file, string path)
        {
            string result = string.Empty;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (file.Length > 0)
            {
                var fileName = file.FileName;
                string filePath = Path.Combine(path, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                result = filePath;
            }
            return result;
        }
    }
}
