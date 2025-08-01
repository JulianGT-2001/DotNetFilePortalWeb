using fileuploadweb.Negocio.Contrato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fileuploadweb.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        #region Atributos
        private readonly IFile _file;
        #endregion

        #region Constructor
        public FileController(IFile file)
        {
            _file = file;
        }
        #endregion

        #region Metodos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(FormFileCollection? files)
        {
            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un archivo.");
                return RedirectToAction("Index", "Home");
            }

            string? jwtToken = Request.Cookies["jwt_token"];

            if (string.IsNullOrEmpty(jwtToken))
                return Unauthorized();

            foreach (var file in files)
            {
                if (file.ContentType != "application/pdf")
                    ModelState.AddModelError(string.Empty, "Solo se permiten archivos PDF.");

                if (file.Length >= 524288000)
                    ModelState.AddModelError(string.Empty, "El archivo debe ser menor a 500 KB.");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            var result = await _file.AddFileAsync<bool>(files, jwtToken);

            if (!result)
                return NotFound();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? guid)
        {
            if (string.IsNullOrEmpty(guid))
                return RedirectToAction("Index", "Home");

            string? jwtToken = Request.Cookies["jwt_token"];

            if (string.IsNullOrEmpty(jwtToken))
                return Unauthorized();

            var result = await _file.DeleteFilesAsync<string, bool>(guid, jwtToken);

            if (!result)
                return NotFound();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetFileContent(string? guid, string? fileNameParam)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrWhiteSpace(fileNameParam))
                return RedirectToAction("Index", "Home");
        
            string? jwtToken = Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(jwtToken))
                return Unauthorized();
        
            var base64Content = await _file.GetFileContentAsync<string, string>(guid, jwtToken);
        
            if (string.IsNullOrEmpty(base64Content))
                return NotFound();
        
            // Decodifica el base64 a bytes
            byte[] fileBytes = Convert.FromBase64String(base64Content);
        
            // Opcional: define el nombre y tipo del archivo
            string fileName = fileNameParam;
            string contentType = "application/pdf";
        
            // Retorna el archivo para descarga
            return File(fileBytes, contentType, fileName);
        }
        #endregion
    }
}