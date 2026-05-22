using CompaniesApi.Models;
using CompaniesApi.Settings;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace CompaniesApi.Services
{
    public class CompanyService
    {
        private IMongoCollection<Company> _company;

        private string Collection_ = "Empresas";

        public CompanyService(IMongoDbConfig settings)
        {
            var conexion_ = new MongoClient(settings.Host);

            var database_ = conexion_.GetDatabase(settings.DataBase);

            _company = database_.GetCollection<Company>(Collection_);
        }

        //Obtener todas las empresas
        public async Task<List<Company>> GetAllAsync()
        {
            return await _company.Find(d => true).ToListAsync();
        }

        //Obtener empresa por objectId
        public async Task<Company> GetByIdAsync(string id)
        {
           return await _company.Find(a => a.Id.Equals(id)).FirstOrDefaultAsync();
        }

        //Crear una nueva empresa
        public async Task CreateAsync(Company empresa)
        {
            await _company.InsertOneAsync(empresa);
        }

        //Actualizar una empresa
        public async Task UpdateAsync(string id, Company empresa)
        {
            await _company.ReplaceOneAsync(a => a.Id.Equals(id), empresa);
        }

        //Desactivar una empresa
        public async Task DeactiveCompany(string id)
        {
            var update = Builders<Company>.Update.Set(a => a.Status, 0);

            await _company.UpdateOneAsync(a => a.Id.Equals(id), update);
        }

        //Activar empresa
        public async Task ActiveCompany(string id)
        {
            var update = Builders<Company>.Update.Set(a => a.Status, 1);

            await _company.UpdateOneAsync(a => a.Id.Equals(id), update);
        }
    }
}
