using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCode.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly QRModel _qRModel;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, QRModel qRModel)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _qRModel = qRModel;
        }

        public IActionResult Index()
        {
            return View(_qRModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(QRModel model)
        {
            if (ModelState.IsValid)
            {
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode(model.QrCodeContent, QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode qRCode = new QRCoder.QRCode(qrCodeData);
                Bitmap qrCodeImage = qRCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(Path.Combine(_webHostEnvironment.WebRootPath, "images", "Logo.png")), 16, 6);//16 logo size
                string qrFileName = Guid.NewGuid().ToString() + ".png";
                FileStream fs = System.IO.File.Open(Path.Combine(_webHostEnvironment.WebRootPath, "images", qrFileName), FileMode.Create);
                qrCodeImage.Save(fs, ImageFormat.Png);
                fs.Close();
                _qRModel.QrCodePath = qrFileName;
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
