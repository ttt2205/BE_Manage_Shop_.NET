using Manage_Store.Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Manage_Store.Services.Documents
{
    public class InventoryPdfDocument : IDocument
    {
        private readonly IEnumerable<Inventory> _inventoryList;

        public InventoryPdfDocument(IEnumerable<Inventory> inventoryList)
        {
            _inventoryList = inventoryList;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(30);

                    // --- HEADER ---
                    page.Header()
                        .Height(80)
                        .Background(Colors.Grey.Lighten2)
                        .AlignCenter()
                        .Text("Báo cáo Tồn kho")
                        .SemiBold().FontSize(24);

                    // --- CONTENT (Nội dung) ---
                    page.Content()
                        .PaddingVertical(10)
                        .Column(col => 
                        {
                            col.Spacing(10); 

                            col.Item().Text($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}");
                            
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); 
                                    columns.RelativeColumn(4); 
                                    columns.RelativeColumn(2); 
                                    columns.RelativeColumn(3); 
                                });

                                table.Header(header =>
                                {
                                    static IContainer CellStyle(IContainer c) => c.Background(Colors.Grey.Darken2).Padding(5);

                                    header.Cell().Element(CellStyle).Text("Mã SP").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Tên Sản phẩm").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Số lượng").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Cập nhật").FontColor(Colors.White);
                                });

                                foreach (var item in _inventoryList)
                                {
                                    static IContainer CellStyle(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);

                                    table.Cell().Element(CellStyle).Text(item.ProductId);
                                    table.Cell().Element(CellStyle).Text(item.Product?.ProductName ?? "N/A"); 
                                    table.Cell().Element(CellStyle).Text(item.Quantity);
                                    table.Cell().Element(CellStyle).Text(item.UpdatedAt.ToString("dd/MM/yyyy"));
                                }
                            });

                            // Thêm tổng số
                            col.Item().AlignRight().Text($"Tổng cộng: {_inventoryList.Count()} mặt hàng").Bold();
                        });


                    // --- FOOTER ---
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber(); 
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
        }
    }
}