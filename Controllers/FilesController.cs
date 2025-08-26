using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    /// <summary>
    /// Controller para gerenciar arquivos no Amazon S3.
    /// Permite upload, download e exclusão de arquivos em buckets dinâmicos.
    /// </summary>
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;

        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }

        /// <summary>
        /// Faz upload de um arquivo para o bucket S3 especificado.
        /// Cria o bucket se não existir.
        /// </summary>
        /// <param name="bucketName">Nome do bucket.</param>
        /// <param name="fileName">Nome do arquivo a ser criado.</param>
        /// <param name="file">Arquivo enviado pelo cliente.</param>
        /// <returns>Mensagem de sucesso.</returns>
        [HttpPost("Add/{fileName}/{bucketName}")]
        public async Task<IActionResult> UploadFile(string bucketName, string fileName, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(bucketName))
                    return BadRequest(new { Message = "Bucket inválido." });

                if (string.IsNullOrEmpty(fileName))
                    return BadRequest(new { Message = "FileName inválido." });

                if (file == null)
                    return BadRequest(new { Message = "Arquivo inválido." });

                var fileExtension = Path.GetExtension(file.FileName);
                using var stream = file.OpenReadStream();
                var key = await _filesService.UploadFileAsync(bucketName, fileName, stream, fileExtension);
                return Ok(key);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retorna um arquivo do bucket S3 especificado.
        /// </summary>
        /// <param name="bucketName">Nome do bucket.</param>
        /// <param name="fileName">Nome do arquivo a ser baixado.</param>
        /// <returns>Arquivo solicitado.</returns>
        [HttpGet("Get-File/{fileName}/{bucketName}")]
        public async Task<IActionResult> GetFile(string bucketName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(bucketName))
                    return BadRequest(new { Message = "Bucket inválido." });

                if (string.IsNullOrEmpty(fileName))
                    return BadRequest(new { Message = "FileName inválido." });

                var stream = await _filesService.GetFileAsync(bucketName, fileName);
                return File(stream, "application/octet-stream", fileName);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deleta um arquivo do bucket S3 especificado.
        /// </summary>
        /// <param name="bucketName">Nome do bucket.</param>
        /// <param name="fileName">Nome do arquivo a ser deletado.</param>
        /// <returns>Mensagem de sucesso.</returns>
        [HttpDelete("Delete/{fileName}/{bucketName}")]
        public async Task<IActionResult> DeleteFile(string bucketName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(bucketName))
                    return BadRequest(new { Message = "Bucket inválido." });

                if (string.IsNullOrEmpty(fileName))
                    return BadRequest(new { Message = "FileName inválido." });

                await _filesService.DeleteFileAsync(bucketName, fileName);
                return Ok(new { Message = "File deleted successfully!" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
