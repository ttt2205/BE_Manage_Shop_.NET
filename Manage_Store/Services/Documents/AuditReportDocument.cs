using Manage_Store.Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;


namespace Manage_Store.Documents
{
    public class AuditReportDocument : IDocument
    {
        private readonly AuditSessions _session;

        public AuditReportDocument(AuditSessions session)
        {
            _session = session;
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
                        .Height(60)
                        .AlignCenter()
                        .Text($"PHIẾU KIỂM KÊ KHO #{_session.Id}")
                        .SemiBold().FontSize(20);

                    // --- CONTENT ---
                    page.Content()
                        .Column(vstack => 
                        {
                            vstack.Spacing(15);

                            vstack.Item().Border(1).Padding(10).Column(infoStack =>
                            {
                                infoStack.Spacing(5);
                                infoStack.Item().Text(txt =>
                                {
                                    txt.Span("Người kiểm kê: ").SemiBold();
                                    txt.Span($"{_session.User?.FullName ?? "N/A"} (ID: {_session.UserId})");
                                });
                                infoStack.Item().Text(txt =>
                                {
                                    txt.Span("Ngày bắt đầu: ").SemiBold();
                                    txt.Span($"{_session.StartDate:dd/MM/yyyy HH:mm}");
                                });
                                infoStack.Item().Text(txt =>
                                {
                                    txt.Span("Ngày hoàn tất: ").SemiBold();
                                    txt.Span($"{_session.EndDate:dd/MM/yyyy HH:mm}");
                                });
                                infoStack.Item().Text(txt =>
                                {
                                    txt.Span("Trạng thái: ").SemiBold();
                                    txt.Span(_session.Status.ToUpper());
                                });
                                infoStack.Item().Text(txt =>
                                {
                                    txt.Span("Ghi chú phiên: ").SemiBold();
                                    txt.Span(_session.Note);
                                });
                            });
                            
                            vstack.Item().Table(table =>
                            {
                               
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30);  
                                    columns.RelativeColumn(1.5f); 
                                    columns.RelativeColumn(4);   
                                    columns.RelativeColumn(1);  
                                    columns.RelativeColumn(1);  
                                    columns.RelativeColumn(1);   
                                    columns.RelativeColumn(2);  
                                });

                                table.Header(header =>
                                {
                                    static IContainer CellStyle(IContainer c) => c.Background(Colors.Grey.Darken2).Padding(4);

                                    header.Cell().Element(CellStyle).Text("STT").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Mã SP").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Tên Sản Phẩm").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Hệ thống").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Thực tế").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Lệch").FontColor(Colors.White);
                                    header.Cell().Element(CellStyle).Text("Ghi chú").FontColor(Colors.White);
                                });

                                int index = 1;
                                foreach (var item in _session.AuditItems)
                                {
                                    static IContainer CellStyle(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4);
                                    
                                    table.Cell().Element(CellStyle).Text(index++);
                                    table.Cell().Element(CellStyle).Text(item.ProductId);
                                    table.Cell().Element(CellStyle).Text(item.Product?.ProductName ?? "N/A");
                                    table.Cell().Element(CellStyle).Text(item.SystemQuantity);
                                    table.Cell().Element(CellStyle).Text(item.ActualQuantity);

                                    IContainer diffCell = table.Cell().Element(CellStyle).AlignCenter();
                                    if (item.Difference > 0)
                                        diffCell.Text($"+{item.Difference}").FontColor(Colors.Green.Medium).Bold();
                                    else if (item.Difference < 0)
                                        diffCell.Text($"{item.Difference}").FontColor(Colors.Red.Medium).Bold();
                                    else
                                        diffCell.Text("0");
                                    
                                    table.Cell().Element(CellStyle).Text(item.Note);
                                }
                            });
                        });
                    
                    // --- FOOTER ---
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber();
                        });
                });
        }
    }
}