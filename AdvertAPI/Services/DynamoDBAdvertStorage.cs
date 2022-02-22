using AdvertAPI.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;

namespace AdvertAPI.Services
{
    public class DynamoDBAdvertStorage: IAdvertStorageService
    {
        public readonly IMapper _mapper;
        public readonly AmazonDynamoDBClient _client;
        public readonly DynamoDBContext _context;

        public DynamoDBAdvertStorage(IMapper mapper, AmazonDynamoDBClient client )
        {
            _mapper = mapper;
            _client = client;
            _context = new DynamoDBContext(_client);
        }

        public async Task<string> AddAsync(AdvertModel advert)
        {
            var dbModel = _mapper.Map<AdvertDbModel>(advert);

            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.Created = DateTime.UtcNow;
            dbModel.Status = AdvertStatus.Pending;

            //var table = await _client.DescribeTableAsync("Adverts");
            await _context.SaveAsync(dbModel);

            return dbModel.Id;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertModel confirmation)
        {
            var record = await _context.LoadAsync<AdvertDbModel>(confirmation.Id);
            if (record == null)
            {
                throw new KeyNotFoundException($"Record with id ={confirmation.Id} not found");
            }
            if (confirmation.Status == AdvertStatus.Active)
            {
                //record.FilePath = confirmation.FilePath;
                record.Status = AdvertStatus.Active;
                await _context.SaveAsync(record);
                return true;
            }
            else
            {
                await _context.DeleteAsync(record);
                return true;
            }
            return false;
        }

        public async Task<List<AdvertModel>> GetAllAsync()
        {
            var scanResult =
                    await _context.ScanAsync<AdvertDbModel>(new List<ScanCondition>()).GetNextSetAsync();
            return scanResult.Select(item => _mapper.Map<AdvertModel>(item)).ToList();
        }

        public async Task<AdvertModel?> GetByIdAsync(string id)
        {
            var dbModel = await _context.LoadAsync<AdvertDbModel>(id);
            if (dbModel != null) return _mapper.Map<AdvertModel>(dbModel);
            
            throw new KeyNotFoundException();
        }
    }
}
