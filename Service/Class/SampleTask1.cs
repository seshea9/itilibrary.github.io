
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRunSetTime.NewFolder
{
    public class SampleTask1 : BackgroundService
    {
        //private readonly ICategoryService dataService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        
        //public zkemkeeper.CZKEM axCZKEM1;

        public SampleTask1(IServiceScopeFactory serviceScopeFactory) 
        {
            //this.dataService = dataService;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        // protected override string Schedule => "*/1 * * * *";


        protected  async Task<Task> ProcessInScope(IServiceProvider serviceProvider)
        {
            try
            {
                Console.WriteLine("Start");
              //  ICategoryService categoryService = serviceProvider.GetRequiredService<ICategoryService>();

              //await  categoryService.GetCategoryList();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erorr :"+ex.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
