using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using ShopTARgv21.Core.Domain;
using ShopTARgv21.Data;

namespace Shop.ApplicationServices
{
    public class FileServices : IFileServices
    {
        private readonly ShopDbContext _context;

        public FileServices
            (
                ShopDbContext context
            )
        {
            _context = context;
        }
                            
        public void UploadFileToDatabase(CarDto dto, Car domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var photo in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = photo.FileName,
                            CarId = domain.Id,
                        };

                        photo.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabase.Add(files);
                    }
                }
            }
        }

        public async Task<FileToDatabase> RemoveImage(FileToDatabaseDto dto)
        {
            var imageId = await _context.FileToDatabase
                .Where(x => x.Id == dto.Id)
                .FirstOrDefaultAsync();

            _context.FileToDatabase.Remove(imageId);
            await _context.SaveChangesAsync();

            return imageId;
        }
        public async Task<List<FileToDatabase>> RemoveImagesFromDatabase(FileToDatabaseDto[] dto)
        {
            foreach (var dtos in dto)
            {
                var photoId = await _context.FileToDatabase
                    .Where(x => x.Id == dtos.Id)
                    .FirstOrDefaultAsync();

                _context.FileToDatabase.Remove(photoId);
                await _context.SaveChangesAsync();
            }
            return null;
        }
    }
}
