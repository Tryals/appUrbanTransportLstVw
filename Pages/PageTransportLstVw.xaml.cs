using appUrbanTransport.BD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace appUrbanTransport.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageTransportLstVw.xaml
    /// </summary>
    public partial class PageTransportLstVw : Page
    {
        public PageTransportLstVw()
        {
            InitializeComponent();
            var currentUser = UrbanTransportEntities.GetContext().Transport.ToList();
            LViewUser.ItemsSource = currentUser;
            DataContext = LViewUser;
            CmbFiltr.Items.Add("Весь транспорт");
            foreach (var item in UrbanTransportEntities.GetContext().Transport.
                Select(x => x.name).Distinct().ToList())
                CmbFiltr.Items.Add(item);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageAddEditTransport((sender as Button).DataContext as Transport));
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearch.Text;
            if (TxtSearch.Text != null)
            {
                LViewUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.
                    Where(x => x.name.Contains(search)
                    || x.speed_km_h.ToString().Contains(search)).ToList();
            }
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            LViewUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.
                OrderBy(x => x.name).ToList();
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            LViewUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.
                OrderByDescending(x => x.name).ToList();
        }

        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFiltr.SelectedValue.ToString() == "Весь транспорт")
            {
                LViewUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.ToList();
            }
            else
            {
                LViewUser.ItemsSource = UrbanTransportEntities.GetContext().Transport.
                    Where(x => x.name == CmbFiltr.SelectedValue.ToString()).ToList();
            }
        }

        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            //объект Excel
            var app = new Excel.Application();

            //книга 
            Excel.Workbook wb = app.Workbooks.Add();
            //лист
            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            int indexRows = 1;
            //ячейка
            worksheet.Cells[1][indexRows] = "Номер";
            worksheet.Cells[2][indexRows] = "Название";
            worksheet.Cells[3][indexRows] = "Скорость";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewUser.Items;
            //цикл по данным из списка для печати
            foreach (Transport item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.name;
                worksheet.Cells[3][indexRows + 1] = item.speed_km_h;


                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[5][indexRows + 1]];
            range.ColumnWidth = 20; //ширина столбцов
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//выравнивание по левому краю

            //показать Excel
            app.Visible = true;
        }

        private void BtnSaveToExcelTemplate_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Шаблон.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[4, 2] = DateTime.Now.ToString();
            ws.Cells[4, 5] = 7;
            int indexRows = 6;
            //ячейка
            ws.Cells[1][indexRows] = "Номер";
            ws.Cells[2][indexRows] = "Название";
            ws.Cells[3][indexRows] = "Скорость";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewUser.Items;
            //цикл по данным из списка для печати
            foreach (Transport item in printItems)
            {
                ws.Cells[1][indexRows + 1] = indexRows;
                ws.Cells[2][indexRows + 1] = item.name;
                ws.Cells[3][indexRows + 1] = item.speed_km_h;

                indexRows++;
            }
            ws.Cells[indexRows + 2, 3] = "Подпись";
            ws.Cells[indexRows + 2, 5] = "Зинченко А.А.";
            excelApp.Visible = true;
        }

        private void BtnSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = UrbanTransportEntities.GetContext().Transport.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Транспорт";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 2);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Название";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Скорость";
            //cellRange = paymentsTable.Cell(1, 7).Range;
            //cellRange.Text = "Фото";
            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allEmployees.Count(); i++)
            {
                var currentEmployee = allEmployees[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentEmployee.name;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentEmployee.speed_km_h.ToString();
            }


            application.Visible = true;

            document.SaveAs2(@"D:\ИСП21.1А\Зинченко Хромов\appUrbanTransportLst\bin\Debug\Word.docx");
        }

        private void BtnSaveToPDF_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = UrbanTransportEntities.GetContext().Transport.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Транспорт";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 2);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Название";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Скорость";

            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allEmployees.Count(); i++)
            {
                var currentEmployee = allEmployees[i];

                //cellRange = paymentsTable.Cell(i + 2, 1).Range;
                //Word.InlineShape imageShape = cellRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory
                //    + "..\\..\\" + currentEmployee.photo);
                //imageShape.Width = imageShape.Height = 40;
                //cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentEmployee.name;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentEmployee.speed_km_h.ToString();
            }


            application.Visible = true;

            document.SaveAs2(@"D:\ИСП21.1А\Зинченко Хромов\appUrbanTransportLst\bin\Debug\PDF.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void BtnGoToDiagram_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageDiagram());
        }
    }
}
