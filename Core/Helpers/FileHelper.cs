using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public class FileHelper
    {
        private static readonly string _currentDirectory = Environment.CurrentDirectory + "\\wwwroot\\";
        private static readonly string _folderName = "Images\\";
        public static IDataResult<string> Upload(IFormFile file)
        {
            var fileExists = CheckFileExists(file);
            if (fileExists.Message != null)
            {
                return new ErrorDataResult<string>(fileExists.Message);
            }

            var type = Path.GetExtension(file.FileName);
            var typeValid = CheckFileTypeValid(type);
            var randomName = Guid.NewGuid().ToString();

            if (typeValid.Message != null)
            {
                return new ErrorDataResult<string>(typeValid.Message);
            }

            CheckDirectoryExists(_currentDirectory + _folderName);
            CreateImageFile(_currentDirectory + _folderName + randomName + type, file);
            return new SuccessDataResult<string>(randomName+type, (_folderName + randomName + type).Replace("\\", "/"));
        }
        public static IDataResult<string> Update(IFormFile newFile, string oldImagePath)
        {
            var fileExists = CheckFileExists(newFile);
            if (fileExists.Message != null)
            {
                return new ErrorDataResult<string>(null!,fileExists.Message);
            }

            var type = Path.GetExtension(newFile.FileName);
            var typeValid = CheckFileTypeValid(type);
            var randomName = Guid.NewGuid().ToString();

            if (typeValid.Message != null)
            {
                return new ErrorDataResult<string>(null!, typeValid.Message);
            }

            var x = (_currentDirectory + oldImagePath).Replace("/", "\\");
            DeleteOldImageFile((_currentDirectory+_folderName + oldImagePath).Replace("/", "\\"));
            CheckDirectoryExists(_currentDirectory + _folderName);
            CreateImageFile(_currentDirectory + _folderName + randomName + type, newFile);
            return new SuccessDataResult<string>(randomName + type, (_folderName + randomName + type).Replace("\\", "/"));
        }

        public static IResult Delete(string path)
        {
            DeleteOldImageFile((_currentDirectory + path).Replace("/", "\\"));
            return new SuccessResult();
        }

        private static IResult CheckFileExists(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Dosya mevcut değil.");
        }

        private static IResult CheckFileTypeValid(string type)
        {
            if (type != ".jpeg" && type != ".png" && type != ".jpg")
            {
                return new ErrorResult("Geçersiz dosya uzantısı.");
            }
            return new SuccessResult();
        }

        private static void CheckDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void CreateImageFile(string directory, IFormFile file)
        {
            using FileStream fs = File.Create(directory);
            file.CopyTo(fs);
            fs.Flush();
        }

        private static void DeleteOldImageFile(string directory)
        {
            if (File.Exists(directory.Replace("/", "\\")))
            {
                File.Delete(directory.Replace("/", "\\"));
            }
        }
    }
}

