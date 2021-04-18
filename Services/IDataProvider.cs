using System.Collections.Generic;

namespace ClientsExercise.Services
{
    public interface IDataProvider
    {
        public IList<Models.Client> GetList();
        public Models.Client CreateClient(Models.Client model);

        public Models.Client UpdateClient(Models.Client model);

        public bool DeleteClient(int id);
    }
}
