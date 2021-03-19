﻿using System.Collections.Generic;

namespace MyIdentity.API.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        void SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}