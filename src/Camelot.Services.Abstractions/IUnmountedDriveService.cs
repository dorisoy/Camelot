using System.Collections.Generic;
using Camelot.Services.Abstractions.Models;

namespace Camelot.Services.Abstractions
{
    public interface IUnmountedDriveService
    {
        IEnumerable<UnmountedDriveModel> GetUnmountedDrives();
    }
}