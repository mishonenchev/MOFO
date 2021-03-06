﻿using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IFileService
    {
        void Remove(File file);
        string NewDownloadCode();
        File GetFileByDownloadCode(string downloadCode);
        IEnumerable<File> GetFilesByUserSession(Session session);
    }
}
