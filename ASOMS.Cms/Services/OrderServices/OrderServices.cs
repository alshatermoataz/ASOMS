using ASOMS.Cms.Services.Interfaces;
using ASOMS.Cms.Services.OrdersInterface;
using ASOMS.DAL.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ASOMS.Cms.Services.OrderServices
{
    public class OrderServices () : IOrderService
    {


            public async Task<byte[]> Generate(List<Order> orders)
            {
                using var stream = new MemoryStream();
                var document = new PdfDocument();

                foreach (var order in orders)
                {
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    var font = new XFont("Verdana", 12, XFontStyle.Regular);

                    double y = 40;

                    gfx.DrawString($"Invoice for Order #{order.Id.ToString().Substring(0, 8).ToUpper()}", font, XBrushes.Black, new XRect(0, y, page.Width, 0), XStringFormats.TopCenter);
                    y += 30;
                    gfx.DrawString($"Customer: {order.User.FullName}", font, XBrushes.Black, new XPoint(40, y));
                    y += 20;
                    gfx.DrawString($"Date: {order.CreatedAt:yyyy-MM-dd}", font, XBrushes.Black, new XPoint(40, y));
                    y += 30;

                    gfx.DrawString($"Items:", font, XBrushes.Black, new XPoint(40, y));
                    y += 20;

                    foreach (var item in order.Items)
                    {
                        string line = $"{item.Product?.Name ?? "Product"} - {item.Quantity} x RM{item.Price:0.00} = RM{(item.Quantity * item.Price):0.00}";
                        gfx.DrawString(line, font, XBrushes.Black, new XPoint(60, y));
                        y += 20;
                    }

                    y += 10;
                    gfx.DrawString($"Total: RM{order.TotalAmount:0.00}", font, XBrushes.Black, new XPoint(40, y));
                }

                document.Save(stream);
                return stream.ToArray();
            }
        

    }
}
