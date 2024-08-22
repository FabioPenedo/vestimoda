using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VestiModa.Models;

namespace VestiModa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminImagesController : Controller
    {
        private readonly ConfigurationImages _myConfig;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminImagesController(IWebHostEnvironment hostEnvironment, IOptions<ConfigurationImages> myConfiguration)
        {
            _webHostEnvironment = hostEnvironment;
            _myConfig = myConfiguration.Value;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || !files.Any())
            {
                ViewData["Erro"] = "Arquivo(s) não selecionado(s)";
                return View(ViewData);
            }

            if (files.Count > 10)
            {
                ViewData["Erro"] = "Quantidade de arquivos excedeu o limite";
                return View(ViewData);
            }

            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png" };
            var filePathsName = new List<string>();
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);

            foreach (var formFile in files)
            {
                var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                if (allowedExtensions.Contains(extension))
                {
                    var fileNameWithPath = Path.Combine(filePath, formFile.FileName);
                    filePathsName.Add(fileNameWithPath);

                    await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                    await formFile.CopyToAsync(stream);
                }
            }

            long size = files.Sum(f => f.Length);

            ViewData["Resultado"] = $"{files.Count} arquivo(s) foi(ram) enviado(s) ao servidor, " +
                                    $"com tamanho total de: {size} bytes";
            return View(ViewData);
        }

        public IActionResult GetImages()
        {
            FileManagerModel model = new();

            string userImagesPath = Path.Combine(_webHostEnvironment.WebRootPath,
                _myConfig.NomePastaImagensProdutos);

            DirectoryInfo dir = new DirectoryInfo(userImagesPath);

            FileInfo[] files = dir.GetFiles();

            model.PathImagesProduct = _myConfig.NomePastaImagensProdutos;

            if (files.Length == 0)
            {
                ViewData["Erro"] = $"Nenhum arquivo encontrado na pasta {_myConfig.NomePastaImagensProdutos}";
                model.Files = null;
                return View(model);
            }

            model.Files = files;

            return View(model);
        }

        public IActionResult DeleteFile(string fname)
        {
            string imageDelete = Path.Combine(_webHostEnvironment.WebRootPath,
               _myConfig.NomePastaImagensProdutos + "\\", fname);

            if (System.IO.File.Exists(imageDelete))
            {
                System.IO.File.Delete(imageDelete);
                ViewData["Deletado"] = "Arquivo deletado com sucesso";
            }

            return View(nameof(Index));
        }
    }
}
