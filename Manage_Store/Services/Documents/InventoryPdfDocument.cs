using Manage_Store.Models.Dtos; // Sử dụng Namespace chứa InventoryDto
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manage_Store.Services.Documents
{
    public class InventoryPdfDocument : IDocument
    {
        // 1. Sửa kiểu dữ liệu nhận vào là DTO
        private readonly IEnumerable<InventoryDto> _inventoryList;

        public InventoryPdfDocument(IEnumerable<InventoryDto> inventoryList)
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
                                // Định nghĩa độ rộng cột
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); // Mã SP
                                    columns.RelativeColumn(4); // Tên SP
                                    columns.RelativeColumn(2); // Số lượng
                                    columns.RelativeColumn(3); // Ngày cập nhật
                                });

                                // Header của bảng
                                table.Header(header =>
                                {
                                    static IContainer CellStyle(IContainer c) => c.Background(Colors.Grey.Darken2).Padding(5);

                                    header.Cell().Element(CellStyle).Text("Mã SP").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Tên Sản phẩm").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Số lượng").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Cập nhật").FontColor(Colors.White);
                                });

                                // Dữ liệu bảng
                                foreach (var item in _inventoryList)
                                {
                                    static IContainer CellStyle(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);

                                    table.Cell().Element(CellStyle).Text(item.ProductId.ToString());

                                    // 2. SỬA QUAN TRỌNG: Dùng item.ProductName trực tiếp từ DTO
                                    table.Cell().Element(CellStyle).Text(string.IsNullOrEmpty(item.ProductName) ? "N/A" : item.ProductName);

                                    table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
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