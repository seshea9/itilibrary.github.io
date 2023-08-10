using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;

namespace AutoRunSetTime.NewFolder
{
    public class TimeHostedService : IHostedService
    {
        ITI_LibraySystemContext db;
        private int executionCount = 0;
        private readonly ILogger<TimeHostedService> _logger;
        private Timer _timer;
        private ICategoryService categoryService;
        public TimeHostedService(ILogger<TimeHostedService> logger,ICategoryService categoryService)
        {
            _logger = logger;
            this.categoryService = categoryService;
            this.db = db;
        }
        //public void Dispose()
        //{
        //    _timer?.Dispose();
        //}

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }
        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
         var list=  await categoryService.GetCategoryList();
            Console.WriteLine(list.data.ToString());
            //_logger.LogInformation(
            //    "Timed Hosted Service is working. Count: {Count}", count);
            //await GetCategoryList();
        }

        //public Task<Data> PostCategoryInfo(CategoryModel cate)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Data> GetCategoryInfo(int CateId)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Data> GetCategoryList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? CateNameKh = null, string? CateNameEn = null)
        //{
        //    Data model = new Data();
        //    model.data = db.TblItiCategory.ToList();
        //    return model;
        //}
    }
}
