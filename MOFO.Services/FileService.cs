using MOFO.Database.Contracts;
using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services
{
    public class FileService: IFileService
    {
        private readonly IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public void Remove(File file)
        {
            _fileRepository.Remove(file);
            _fileRepository.SaveChanges();
        }
        public string NewDownloadCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = "";
            Random rn = new Random();
            for (int i = 0; i < 12; i++)
            {
                result += chars[rn.Next(0, chars.Length - 1)];
            }
            if (_fileRepository.Where(x => x.DownloadCode == result).Count() == 0)
            {
                return result;
            }
            else return NewDownloadCode();
        }
        public File GetFileByDownloadCode(string downloadCode)
        {
            return _fileRepository.Where(x => x.DownloadCode == downloadCode).FirstOrDefault();
        }
        public IEnumerable<File> GetFilesByUserSession(Session session)
        {
            return _fileRepository.WhereIncludeAll(x => x.User.Session.Id == session.Id).ToList();
        }
    }
}
