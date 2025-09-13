using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDiniz.Application.Interfaces
{
    public interface IObjectStorage
    {
        Task<string> UploadCnhImageAsync(string riderIdentifier, byte[] imageBytes, string contentType, CancellationToken cancellationToken);
    }
}
