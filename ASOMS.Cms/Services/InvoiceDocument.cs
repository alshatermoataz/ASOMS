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
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(col =>
                {
                    foreach (var order in Orders)
                    {
                        col.Item().Element(c => ComposeOrder(c, order));
                        col.Item().PaddingVertical(10).LineHorizontal(0.5f);
                    }
                });
            });
        }

        private void ComposeOrder(IContainer container, Order order)
        {
            container.Column(col =>
            {
                col.Item().Text($"Invoice for Order #{order.Id.ToString()[..8].ToUpper()}")
                    .FontSize(16).Bold().AlignCenter();
                col.Item().Text($"Customer: {order.User.FullName}");
                col.Item().Text($"Date: {order.CreatedAt:yyyy-MM-dd}");
                col.Item().Text(" ");

                col.Item().Text("Items:").Bold();

                foreach (var item in order.Items)
                {
                    var name = item.Product?.Name ?? "Product";
                    var line = $"{name} - {item.Quantity} x RM{item.Price:0.00} = RM{item.Quantity * item.Price:0.00}";
                    col.Item().Text(line);
                }

                col.Item().PaddingTop(10).Text($"Total: RM{order.TotalAmount:0.00}").Bold();
            });
        }
    }

}
