using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fileuploadweb.Models;
using Microsoft.AspNetCore.Authorization;
using fileuploadweb.Negocio.Contrato;
using System.Threading.Tasks;
using fileuploadweb.Models.Dto;
using fileuploadweb.Models.ViewModel;

namespace fileuploadweb.Controllers;

[Authorize]
public class HomeController : Controller
{
    #region Atributos
    private readonly IFile _file;
    #endregion

    public HomeController(IFile file)
    {
        _file = file;
    }

    public async Task<IActionResult> Index()
    {
        var token = Request.Cookies["jwt_token"];

        Console.WriteLine(token);

        if (token == null)
        {
            return Unauthorized();
        }

        var response = await _file.GetFilesAsync<IEnumerable<FileResponseDto>>(token);

        List<UserFilesViewModel> files = new List<UserFilesViewModel>();

        if (response.Count() > 0)
        {
            foreach (var file in response)
            {
                files.Add(new UserFilesViewModel
                {
                    Id = file.id,
                    OriginalName = file.originalName,
                    Path = file.path,
                    SizeInBytes = file.sizeInBytes,
                    MimeType = file.mimeType,
                    UploadedAt = file.uploadedAt
                });
            }
        }

        return View(files);
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
