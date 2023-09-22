
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Stripe.Checkout;

namespace Web.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ILogger<CheckOutController> _logger;

        public CheckOutController(ILogger<CheckOutController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();

            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Some Product",
                    Rate = 1500,
                    Quantity = 2,
                    ImagePath = "img/kandinsky-download-1695369327830.png"
                },
                 new ProductEntity
                {
                    Product = "Some Test",
                    Rate = 1700,
                    Quantity = 7,
                    ImagePath = "img/kandinsky-download-1695369579241.png"
                },
                 new ProductEntity
                {
                    Product = "Test Product",
                    Rate = 2332,
                    Quantity = 23,
                    ImagePath = "img/kandinsky-download-1695369768994.png"
                }
            };
            return View(productList);
        }


        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();

            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();

                return View("Success");
            }
            return View("Login");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>();

            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Some Product",
                    Rate = 1500,
                    Quantity = 2,
                    ImagePath = "img/kandinsky-download-1695369327830.png"
                },
                 new ProductEntity
                {
                    Product = "Some Test",
                    Rate = 1700,
                    Quantity = 7,
                    ImagePath = "img/kandinsky-download-1695369579241.png"
                },
                 new ProductEntity
                {
                    Product = "Test Product",
                    Rate = 2332,
                    Quantity = 23,
                    ImagePath = "img/kandinsky-download-1695369768994.png"
                }
            };

            var domain = "http://localhost:7176/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + $"CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "test@mail.com"
            };

            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}