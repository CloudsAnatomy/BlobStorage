using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;

namespace UploadFile.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(SingleFileModel model)
        {
            //if (ModelState.IsValid)
            //{
                model.IsResponse = true;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.File.FileName);
                string fileName = model.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

            var blobstorageconnectionstring = "DefaultEndpointsProtocol=https;AccountName=terraformstatefile01;AccountKey=bBmuR3u569QbltpFjAp5OlDIphL+mA4Judys2aM/zY2RrTZmPipLeSzJo2B+qR91wxrh/RV9+qjY+AStn9F8wg==;EndpointSuffix=core.windows.net";
            var ContainerName = "fileupload";
            var container = new BlobContainerClient(blobstorageconnectionstring, ContainerName);
            var blob = container.GetBlobClient(fileName);
          

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                    await blob.UploadAsync(stream);
            }
                model.IsSuccess = true;
                model.Message = "File upload successfully";
            //}
            return View("Index", model);
        }
    }
}
