using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetWalksByWalkerId(int walkerId);
        List<Walk> GetRecentWalksByWalkerId(int walkerId);
    }
}