using Shop.Core.Domain;
using Shop.Core.Dto;
using ShopTARgv21.Core.Domain;
using ShopTARgv21.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IFileServices : IApplicationServices
    {
        void UploadFileToDatabase(CarDto dto, Car domain);
        Task<FileToDatabase> RemoveImage(FileToDatabaseDto dto);
        Task<List<FileToDatabase>> RemoveImagesFromDatabase(FileToDatabaseDto[] dto);
    }
}
