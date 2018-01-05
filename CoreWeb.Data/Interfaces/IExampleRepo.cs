using CoreWeb.Data.Models;
using CoreWeb.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Data.Interfaces
{
    public interface IExampleRepo
    {
        Task<spGetOneExample> spGetOneExampleAsync(int Id);
        Task<IEnumerable<spGetManyExamples>> spGetManyExamplesAsync();
        Task<int> InsertExampleAsync(ExampleViewModel vm, string User);
        Task<int> UpdateExampleAsync(int Id, ExampleViewModel vm, string User);
        Task<int> DeleteExampleAsync(int Id, string User);
    }
}
