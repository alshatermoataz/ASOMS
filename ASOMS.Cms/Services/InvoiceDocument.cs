using ASOMS.DAL.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ASOMS.Cms.Services
{
    public class InvoiceDocument : IDocument
    {
        public List<Order> Orders { get; set; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Roboto"));

                page.Header().Text("Invoice Summary").FontSize(20).Bold();

                page.Content().Column(col =>
                {
                    foreach (var order in Orders)
                    {
                        col.Item().Text($"Order ID: {order.Id}");
                        col.Item().Text($"Customer: {order.User.FullName} - {order.User.Email}");
                        col.Item().Text($"Total Items: {order.Items.Count}");

                        foreach (var item in order.Items)
                        {
                            col.Item().Text($"- {item.Product.Name} x{item.Quantity}");
                        }

                        col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
                    }
                });
            });
        }
    }

}
